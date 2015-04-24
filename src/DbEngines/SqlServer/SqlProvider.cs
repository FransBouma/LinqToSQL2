using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Visitor;
using System.Data.Linq.Provider.Visitors;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Runtime.CompilerServices;


namespace System.Data.Linq.DbEngines.SqlServer
{
	using System.Data.Linq.Provider.Common;
	using System.Data.Linq.Provider.Interfaces;
	using System.Data.Linq.Provider.NodeTypes;
	using System.Data.SqlClient;


	[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Unknown reason.")]
	public class SqlProvider : IReaderProvider, IConnectionUser
	{
		#region Class Members
		private IDataServices _services;
		private ConnectionManager _conManager;
		private TypeSystemProvider _typeProvider;
		private SqlFactory _sqlFactory;
		private Translator _translator;
		private IObjectReaderCompiler _readerCompiler;
		private bool _disposed;
		private int _commandTimeout;
		private TextWriter _log;
		private string _dbName = string.Empty;

		// stats and flags
		private int _queryCount;
		private bool _checkQueries;
		private OptimizationFlags _optimizationFlags = OptimizationFlags.All;
		private bool _enableCacheLookup = true;
		private SqlServerProviderMode _mode;
		private bool _deleted;
		#endregion

		#region Constants
		const string SqlCeProviderInvariantName = "System.Data.SqlServerCe.3.5";
		const string SqlCeDataReaderTypeName = "System.Data.SqlServerCe.SqlCeDataReader";
		const string SqlCeConnectionTypeName = "System.Data.SqlServerCe.SqlCeConnection";
		const string SqlCeTransactionTypeName = "System.Data.SqlServerCe.SqlCeTransaction";
		#endregion

		public SqlProvider()
		{
			_mode = SqlServerProviderMode.Sql2008;
		}

		internal SqlProvider(SqlServerProviderMode mode)
		{
			_mode = mode;
		}

		private void CheckInitialized()
		{
			if(_services == null)
			{
				throw Error.ContextNotInitialized();
			}
		}
		private void CheckNotDeleted()
		{
			if(_deleted)
			{
				throw Error.DatabaseDeleteThroughContext();
			}
		}

		[ResourceExposure(ResourceScope.Machine)] // connection parameter may refer to filenames.
		void IProvider.Initialize(IDataServices dataServices, object connection)
		{
			if(dataServices == null)
			{
				throw Error.ArgumentNull("dataServices");
			}
			_services = dataServices;

			DbConnection con;
			DbTransaction tx = null;

			string fileOrServerOrConnectionString = connection as string;
			if(fileOrServerOrConnectionString != null)
			{
				string connectionString = this.GetConnectionString(fileOrServerOrConnectionString);
				_dbName = this.GetDatabaseName(connectionString);
				if(_dbName.EndsWith(".sdf", StringComparison.OrdinalIgnoreCase))
				{
					_mode = SqlServerProviderMode.SqlCE;
				}
				if(_mode == SqlServerProviderMode.SqlCE)
				{
					DbProviderFactory factory = SqlProvider.GetProvider(SqlCeProviderInvariantName);
					if(factory == null)
					{
						throw Error.ProviderNotInstalled(_dbName, SqlCeProviderInvariantName);
					}
					con = factory.CreateConnection();
				}
				else
				{
					con = new SqlConnection();
				}
				con.ConnectionString = connectionString;
			}
			else
			{
				// We only support SqlTransaction and SqlCeTransaction
				tx = connection as SqlTransaction;
				if(tx == null)
				{
					// See if it's a SqlCeTransaction
					if(connection.GetType().FullName == SqlCeTransactionTypeName)
					{
						tx = connection as DbTransaction;
					}
				}
				if(tx != null)
				{
					connection = tx.Connection;
				}
				con = connection as DbConnection;
				if(con == null)
				{
					throw Error.InvalidConnectionArgument("connection");
				}
				if(con.GetType().FullName == SqlCeConnectionTypeName)
				{
					_mode = SqlServerProviderMode.SqlCE;
				}
				_dbName = this.GetDatabaseName(con.ConnectionString);
			}

			// initialize to the default command timeout
			using(DbCommand c = con.CreateCommand())
			{
				_commandTimeout = c.CommandTimeout;
			}

			int maxUsersPerConnection = 1;
			if(con.ConnectionString.IndexOf("MultipleActiveResultSets", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
				builder.ConnectionString = con.ConnectionString;
				if(string.Compare((string)builder["MultipleActiveResultSets"], "true", StringComparison.OrdinalIgnoreCase) == 0)
				{
					maxUsersPerConnection = 10;
				}
			}

			// If fileOrServerOrConnectionString != null, that means we just created the connection instance and we have to tell
			// the SqlConnectionManager that it should dispose the connection when the context is disposed. Otherwise the user owns
			// the connection and should dispose of it themselves.
			_conManager = new ConnectionManager(this, con, maxUsersPerConnection, fileOrServerOrConnectionString != null /*disposeConnection*/);
			if(tx != null)
			{
				_conManager.Transaction = tx;
			}

#if DEBUG
			SqlNode.Formatter = new SqlFormatter();
#endif


			Type readerType;
			if(_mode == SqlServerProviderMode.SqlCE)
			{
				readerType = con.GetType().Module.GetType(SqlCeDataReaderTypeName);
			}
			else if(con is SqlConnection)
			{
				readerType = typeof(SqlDataReader);
			}
			else
			{
				readerType = typeof(DbDataReader);
			}
			_readerCompiler = new ObjectReaderCompiler(readerType, _services);
		}

		private static DbProviderFactory GetProvider(string providerName)
		{
			bool hasProvider =
				DbProviderFactories.GetFactoryClasses().Rows.OfType<DataRow>()
				.Select(r => (string)r["InvariantName"])
				.Contains(providerName, StringComparer.OrdinalIgnoreCase);
			if(hasProvider)
			{
				return DbProviderFactories.GetFactory(providerName);
			}
			return null;
		}

		#region Dispose\Finalize
		public void Dispose()
		{
			_disposed = true;
			Dispose(true);
			// Technically, calling GC.SuppressFinalize is not required because the class does not
			// have a finalizer, but it does no harm, protects against the case where a finalizer is added
			// in the future, and prevents an FxCop warning.
			GC.SuppressFinalize(this);
		}

		// Not implementing finalizer here because there are no unmanaged resources
		// to release. See http://msdnwiki.microsoft.com/en-us/mtpswiki/12afb1ea-3a17-4a3f-a1f0-fcdb853e2359.aspx

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose(bool disposing)
		{
			// Implemented but empty so that derived contexts can implement
			// a finalizer that potentially cleans up unmanaged resources.
			if(disposing)
			{
				_services = null;
				if(_conManager != null)
				{
					_conManager.DisposeConnection();
				}
				_conManager = null;
				_typeProvider = null;
				_sqlFactory = null;
				_translator = null;
				_readerCompiler = null;
				_log = null;
			}
		}

		internal void CheckDispose()
		{
			if(_disposed)
			{
				throw Error.ProviderCannotBeUsedAfterDispose();
			}
		}
		#endregion

		private string GetConnectionString(string fileOrServerOrConnectionString)
		{
			if(fileOrServerOrConnectionString.IndexOf('=') >= 0)
			{
				return fileOrServerOrConnectionString;
			}
			DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
			if(fileOrServerOrConnectionString.EndsWith(".mdf", StringComparison.OrdinalIgnoreCase))
			{
				// if just a database file is specified, default to local SqlExpress instance
				builder.Add("AttachDBFileName", fileOrServerOrConnectionString);
				builder.Add("Server", "localhost\\sqlexpress");
				builder.Add("Integrated Security", "SSPI");
				builder.Add("User Instance", "true");
				builder.Add("MultipleActiveResultSets", "true");
			}
			else if(fileOrServerOrConnectionString.EndsWith(".sdf", StringComparison.OrdinalIgnoreCase))
			{
				// A SqlCE database file has been specified
				builder.Add("Data Source", fileOrServerOrConnectionString);
			}
			else
			{
				builder.Add("Server", fileOrServerOrConnectionString);
				builder.Add("Database", _services.Model.DatabaseName);
				builder.Add("Integrated Security", "SSPI");
			}
			return builder.ToString();
		}

		private string GetDatabaseName(string constr)
		{
			DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
			builder.ConnectionString = constr;

			if(builder.ContainsKey("Initial Catalog"))
			{
				return (string)builder["Initial Catalog"];
			}
			else if(builder.ContainsKey("Database"))
			{
				return (string)builder["Database"];
			}
			else if(builder.ContainsKey("AttachDBFileName"))
			{
				return (string)builder["AttachDBFileName"];
			}
			else if(builder.ContainsKey("Data Source")
				&& ((string)builder["Data Source"]).EndsWith(".sdf", StringComparison.OrdinalIgnoreCase))
			{
				return (string)builder["Data Source"];
			}
			else
			{
				return _services.Model.DatabaseName;
			}
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		[ResourceExposure(ResourceScope.None)] // Exposure is via other methods that set dbName.
		[ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)] // File.Exists method call.
		void IProvider.CreateDatabase()
		{
			this.CheckDispose();
			this.CheckInitialized();
			// Don't need to call CheckNotDeleted() here since we allow CreateDatabase after DeleteDatabase
			// Don't need to call InitializeProviderMode() here since we don't need to know the provider to do this.
			string catalog = null;
			string filename = null;

			DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
			builder.ConnectionString = _conManager.Connection.ConnectionString;

			if(_conManager.Connection.State == ConnectionState.Closed)
			{
				if(_mode == SqlServerProviderMode.SqlCE)
				{
					if(!File.Exists(_dbName))
					{
						Type engineType = _conManager.Connection.GetType().Module.GetType("System.Data.SqlServerCe.SqlCeEngine");
						object engine = Activator.CreateInstance(engineType, new object[] { builder.ToString() });
						try
						{
							engineType.InvokeMember("CreateDatabase", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, engine, new object[] { }, CultureInfo.InvariantCulture);
						}
						catch(TargetInvocationException tie)
						{
							throw tie.InnerException;
						}
						finally
						{
							IDisposable disp = engine as IDisposable;
							if(disp != null)
							{
								disp.Dispose();
							}
						}
					}
					else
					{
						throw Error.CreateDatabaseFailedBecauseSqlCEDatabaseAlreadyExists(_dbName);
					}
				}
				else
				{
					// get connection string w/o reference to new catalog
					object val;
					if(builder.TryGetValue("Initial Catalog", out val))
					{
						catalog = val.ToString();
						builder.Remove("Initial Catalog");
					}
					if(builder.TryGetValue("Database", out val))
					{
						catalog = val.ToString();
						builder.Remove("Database");
					}
					if(builder.TryGetValue("AttachDBFileName", out val))
					{
						filename = val.ToString();
						builder.Remove("AttachDBFileName");
					}
				}
				_conManager.Connection.ConnectionString = builder.ToString();
			}
			else
			{
				if(_mode == SqlServerProviderMode.SqlCE)
				{
					if(File.Exists(_dbName))
					{
						throw Error.CreateDatabaseFailedBecauseSqlCEDatabaseAlreadyExists(_dbName);
					}
				}
				object val;
				if(builder.TryGetValue("Initial Catalog", out val))
				{
					catalog = val.ToString();
				}
				if(builder.TryGetValue("Database", out val))
				{
					catalog = val.ToString();
				}
				if(builder.TryGetValue("AttachDBFileName", out val))
				{
					filename = val.ToString();
				}
			}

			if(String.IsNullOrEmpty(catalog))
			{
				if(!String.IsNullOrEmpty(filename))
				{
					catalog = Path.GetFullPath(filename);
				}
				else if(!String.IsNullOrEmpty(_dbName))
				{
					catalog = _dbName;
				}
				else
				{
					throw Error.CouldNotDetermineCatalogName();
				}
			}

			_conManager.UseConnection(this);
			_conManager.AutoClose = false;

			try
			{
				if(_services.Model.GetTables().FirstOrDefault() == null)
				{
					// we have no tables to create
					throw Error.CreateDatabaseFailedBecauseOfContextWithNoTables(_services.Model.DatabaseName);
				}

				_deleted = false;

				// create database
				if(_mode == SqlServerProviderMode.SqlCE)
				{

					// create tables
					foreach(MetaTable table in _services.Model.GetTables())
					{
						string command = SqlBuilder.GetCreateTableCommand(table);
						if(!String.IsNullOrEmpty(command))
						{
							this.ExecuteCommand(command);
						}
					}
					// create all foreign keys after all tables are defined
					foreach(MetaTable table in _services.Model.GetTables())
					{
						foreach(string command in SqlBuilder.GetCreateForeignKeyCommands(table))
						{
							if(!String.IsNullOrEmpty(command))
							{
								this.ExecuteCommand(command);
							}
						}
					}
				}
				else
				{
					string createdb = SqlBuilder.GetCreateDatabaseCommand(catalog, filename, Path.ChangeExtension(filename, ".ldf"));
					this.ExecuteCommand(createdb);
					_conManager.Connection.ChangeDatabase(catalog);

					// create the schemas that our tables will need
					// cannot be batched together with the rest of the CREATE TABLES
					if(_mode == SqlServerProviderMode.Sql2005 || _mode == SqlServerProviderMode.Sql2008)
					{
						HashSet<string> schemaCommands = new HashSet<string>();

						foreach(MetaTable table in _services.Model.GetTables())
						{
							string schemaCommand = SqlBuilder.GetCreateSchemaForTableCommand(table);
							if(!string.IsNullOrEmpty(schemaCommand))
							{
								schemaCommands.Add(schemaCommand);
							}
						}

						foreach(string schemaCommand in schemaCommands)
						{
							this.ExecuteCommand(schemaCommand);
						}
					}

					StringBuilder sb = new StringBuilder();

					// create tables
					foreach(MetaTable table in _services.Model.GetTables())
					{
						string createTable = SqlBuilder.GetCreateTableCommand(table);
						if(!string.IsNullOrEmpty(createTable))
						{
							sb.AppendLine(createTable);
						}
					}

					// create all foreign keys after all tables are defined
					foreach(MetaTable table in _services.Model.GetTables())
					{
						foreach(string createFK in SqlBuilder.GetCreateForeignKeyCommands(table))
						{
							if(!string.IsNullOrEmpty(createFK))
							{
								sb.AppendLine(createFK);
							}
						}
					}

					if(sb.Length > 0)
					{
						// must be on when creating indexes on computed columns
						sb.Insert(0, "SET ARITHABORT ON" + Environment.NewLine);
						this.ExecuteCommand(sb.ToString());
					}
				}
			}
			finally
			{
				_conManager.ReleaseConnection(this);
				if(_conManager.Connection is SqlConnection)
				{
					SqlConnection.ClearAllPools();
				}
			}
		}

		[ResourceExposure(ResourceScope.None)] // Exposure is via other methods that set dbName.
		[ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)] // File.Delete method call.
		void IProvider.DeleteDatabase()
		{
			this.CheckDispose();
			this.CheckInitialized();
			// Don't need to call InitializeProviderMode() here since we don't need to know the provider to do this.
			if(_deleted)
			{
				// 2nd delete is no-op.
				return;
			}

			if(_mode == SqlServerProviderMode.SqlCE)
			{
				((IProvider)this).ClearConnection();
				System.Diagnostics.Debug.Assert(_conManager.Connection.State == ConnectionState.Closed);
				File.Delete(_dbName);
				_deleted = true;
			}
			else
			{
				string holdConnStr = _conManager.Connection.ConnectionString;
				DbConnection con = _conManager.UseConnection(this);
				try
				{
					con.ChangeDatabase("master");
					if(con is SqlConnection)
					{
						SqlConnection.ClearAllPools();
					}
					if(_log != null)
					{
						_log.WriteLine(Strings.LogAttemptingToDeleteDatabase(_dbName));
					}
					this.ExecuteCommand(SqlBuilder.GetDropDatabaseCommand(_dbName));
					_deleted = true;
				}
				finally
				{
					_conManager.ReleaseConnection(this);
					if(_conManager.Connection.State == ConnectionState.Closed &&
						string.Compare(_conManager.Connection.ConnectionString, holdConnStr, StringComparison.Ordinal) != 0)
					{
						// Credential information may have been stripped from the connection
						// string as a result of opening the connection. Restore the full
						// connection string.
						_conManager.Connection.ConnectionString = holdConnStr;
					}
				}
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[....]: Code needs to return false regarless of exception.")]
		[ResourceExposure(ResourceScope.None)] // Exposure is via other methods that set dbName.
		[ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)] // File.Exists method call.
		bool IProvider.DatabaseExists()
		{
			this.CheckDispose();
			this.CheckInitialized();
			if(_deleted)
			{
				return false;
			}
			// Don't need to call InitializeProviderMode() here since we don't need to know the provider to do this.

			bool exists = false;
			if(_mode == SqlServerProviderMode.SqlCE)
			{
				exists = File.Exists(_dbName);
			}
			else
			{
				string holdConnStr = _conManager.Connection.ConnectionString;
				try
				{
					// If no database name is explicitly specified on the connection,
					// UseConnection will connect to 'Master', which is why after connecting
					// we call ChangeDatabase to verify that the database actually exists.
					_conManager.UseConnection(this);
					_conManager.Connection.ChangeDatabase(_dbName);
					_conManager.ReleaseConnection(this);
					exists = true;
				}
				catch(Exception)
				{
				}
				finally
				{
					if(_conManager.Connection.State == ConnectionState.Closed &&
						string.Compare(_conManager.Connection.ConnectionString, holdConnStr, StringComparison.Ordinal) != 0)
					{
						// Credential information may have been stripped from the connection
						// string as a result of opening the connection. Restore the full
						// connection string.
						_conManager.Connection.ConnectionString = holdConnStr;
					}
				}
			}
			return exists;
		}

		void IConnectionUser.CompleteUse()
		{
		}

		void IProvider.ClearConnection()
		{
			this.CheckDispose();
			this.CheckInitialized();
			_conManager.ClearConnection();
		}

		private void ExecuteCommand(string command)
		{
			if(_log != null)
			{
				_log.WriteLine(command);
				_log.WriteLine();
			}
			IDbCommand cmd = _conManager.Connection.CreateCommand();
			cmd.CommandTimeout = _commandTimeout;
			cmd.Transaction = _conManager.Transaction;
			cmd.CommandText = command;
			cmd.ExecuteNonQuery();
		}

		ICompiledQuery IProvider.Compile(Expression query)
		{
			this.CheckDispose();
			this.CheckInitialized();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			this.InitializeProviderMode();

#warning [FB] POSSIBLE CLONE OF CODE FOUND IN IProvider.Execute
			SqlNodeAnnotations annotations = new SqlNodeAnnotations();
			QueryInfo[] qis = this.BuildQuery(query, annotations);
			CheckSqlCompatibility(qis, annotations);

			LambdaExpression lambda = query as LambdaExpression;
			if(lambda != null)
			{
				query = lambda.Body;
			}

			IObjectReaderFactory factory = null;
			ICompiledSubQuery[] subQueries = null;
			QueryInfo qi = qis[qis.Length - 1];
			if(qi.ResultShape == ResultShape.Singleton)
			{
				subQueries = this.CompileSubQueries(qi.Query);
				factory = this.GetReaderFactory(qi.Query, qi.ResultType);
			}
			else if(qi.ResultShape == ResultShape.Sequence)
			{
				subQueries = this.CompileSubQueries(qi.Query);
				factory = this.GetReaderFactory(qi.Query, TypeSystem.GetElementType(qi.ResultType));
			}

			return new AdoCompiledQuery(this, query, qis, factory, subQueries);
		}

		private ICompiledSubQuery CompileSubQuery(SqlNode query, Type elementType, ReadOnlyCollection<System.Data.Linq.Provider.NodeTypes.SqlParameter> parameters)
		{
			query = SqlDuplicator.Copy(query);
			SqlNodeAnnotations annotations = new SqlNodeAnnotations();

			QueryInfo[] qis = this.BuildQuery(ResultShape.Sequence, TypeSystem.GetSequenceType(elementType), query, parameters, annotations);
			System.Diagnostics.Debug.Assert(qis.Length == 1);
			QueryInfo qi = qis[0];
			ICompiledSubQuery[] subQueries = this.CompileSubQueries(qi.Query);
			IObjectReaderFactory factory = this.GetReaderFactory(qi.Query, elementType);

			CheckSqlCompatibility(qis, annotations);

			return new CompiledSubQuery(qi, factory, parameters, subQueries);
		}

		

		private ICompiledSubQuery[] CompileSubQueries(SqlNode query)
		{
			return new SubQueryCompiler(this).Compile(query);
		}


		/// <summary>
		/// Look for compatibility annotations for the set of providers we
		/// add annotations for.
		/// </summary>
		private void CheckSqlCompatibility(QueryInfo[] queries, SqlNodeAnnotations annotations)
		{
			if(this.Mode == SqlServerProviderMode.Sql2000 ||
				this.Mode == SqlServerProviderMode.SqlCE)
			{
				for(int i = 0, n = queries.Length; i < n; i++)
				{
					SqlServerCompatibilityCheck.ThrowIfUnsupported(queries[i].Query, annotations, this.Mode);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private IExecuteResult GetCachedResult(Expression query)
		{
			object obj = _services.GetCachedObject(query);
			if(obj != null)
			{
				switch(this.GetResultShape(query))
				{
					case ResultShape.Singleton:
						return new ExecuteResult(null, null, null, obj);
					case ResultShape.Sequence:
						return new ExecuteResult(null, null, null,
							Activator.CreateInstance(
								typeof(SequenceOfOne<>).MakeGenericType(TypeSystem.GetElementType(this.GetResultType(query))),
								BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { obj }, null
								));
				}
			}
			return null;
		}

		private MetaFunction GetFunction(Expression query)
		{
			LambdaExpression lambda = query as LambdaExpression;
			if(lambda != null)
			{
				query = lambda.Body;
			}
			MethodCallExpression mc = query as MethodCallExpression;
			if(mc != null && typeof(DataContext).IsAssignableFrom(mc.Method.DeclaringType))
			{
				return _services.Model.GetFunction(mc.Method);
			}
			return null;
		}

		private void LogCommand(TextWriter writer, DbCommand cmd)
		{
			if(writer != null)
			{
				writer.WriteLine(cmd.CommandText);
				foreach(DbParameter p in cmd.Parameters)
				{
					int prec = 0;
					int scale = 0;
					PropertyInfo piPrecision = p.GetType().GetProperty("Precision");
					if(piPrecision != null)
					{
						prec = (int)Convert.ChangeType(piPrecision.GetValue(p, null), typeof(int), CultureInfo.InvariantCulture);
					}
					PropertyInfo piScale = p.GetType().GetProperty("Scale");
					if(piScale != null)
					{
						scale = (int)Convert.ChangeType(piScale.GetValue(p, null), typeof(int), CultureInfo.InvariantCulture);
					}
					var sp = p as System.Data.SqlClient.SqlParameter;
					writer.WriteLine("-- {0}: {1} {2} (Size = {3}; Prec = {4}; Scale = {5}) [{6}]",
						p.ParameterName,
						p.Direction,
						sp == null ? p.DbType.ToString() : sp.SqlDbType.ToString(),
						p.Size.ToString(System.Globalization.CultureInfo.CurrentCulture),
						prec,
						scale,
						sp == null ? p.Value : sp.SqlValue);
				}
#warning [FB] IMPLEMENT FILE VERSION RETRIEVAL HERE AND REPLACE "1.0"
				writer.WriteLine("-- Context: {0}({1}) Model: {2} Build: {3}", this.GetType().Name, this.Mode, _services.Model.GetType().Name, "1.0 (placeholder)");
				writer.WriteLine();
			}
		}

		private void AssignParameters(DbCommand cmd, ReadOnlyCollection<SqlParameterInfo> parms, object[] userArguments, object lastResult)
		{
			if(parms != null)
			{
				foreach(SqlParameterInfo pi in parms)
				{
					DbParameter p = cmd.CreateParameter();
					p.ParameterName = pi.Parameter.Name;
					p.Direction = pi.Parameter.Direction;
					if(pi.Parameter.Direction == ParameterDirection.Input ||
						pi.Parameter.Direction == ParameterDirection.InputOutput)
					{
						object value = pi.Value;
						switch(pi.Type)
						{
							case SqlParameterType.UserArgument:
								try
								{
									value = pi.Accessor.DynamicInvoke(new object[] { userArguments });
								}
								catch(System.Reflection.TargetInvocationException e)
								{
									throw e.InnerException;
								}
								break;
							case SqlParameterType.PreviousResult:
								value = lastResult;
								break;
						}
						_typeProvider.InitializeParameter(pi.Parameter.SqlType, p, value);
					}
					else
					{
						_typeProvider.InitializeParameter(pi.Parameter.SqlType, p, null);
					}
					cmd.Parameters.Add(p);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		IEnumerable IProvider.Translate(Type elementType, DbDataReader reader)
		{
			this.CheckDispose();
			this.CheckInitialized();
			this.InitializeProviderMode();
			if(elementType == null)
			{
				throw Error.ArgumentNull("elementType");
			}
			if(reader == null)
			{
				throw Error.ArgumentNull("reader");
			}
			MetaType rowType = _services.Model.GetMetaType(elementType);
			IObjectReaderFactory factory = this.GetDefaultFactory(rowType);
			IEnumerator e = factory.Create(reader, true, this, null, null, null);
			Type enumerableType = typeof(OneTimeEnumerable<>).MakeGenericType(elementType);
			return (IEnumerable)Activator.CreateInstance(enumerableType, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { e }, null);
		}

		IMultipleResults IProvider.Translate(DbDataReader reader)
		{
			this.CheckDispose();
			this.CheckInitialized();
			this.InitializeProviderMode();
			if(reader == null)
			{
				throw Error.ArgumentNull("reader");
			}
			IObjectReaderSession session = _readerCompiler.CreateSession(reader, this, null, null, null);
			return new MultipleResults(this, null, session, null);
		}

		string IProvider.GetQueryText(Expression query)
		{
			this.CheckDispose();
			this.CheckInitialized();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			this.InitializeProviderMode();
			SqlNodeAnnotations annotations = new SqlNodeAnnotations();
			QueryInfo[] qis = this.BuildQuery(query, annotations);

			StringBuilder sb = new StringBuilder();
			for(int i = 0, n = qis.Length; i < n; i++)
			{
				QueryInfo qi = qis[i];
#if DEBUG
				StringWriter writer = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
				DbCommand cmd = _conManager.Connection.CreateCommand();
				cmd.CommandText = qi.CommandText;
				AssignParameters(cmd, qi.Parameters, null, null);
				LogCommand(writer, cmd);
				sb.Append(writer.ToString());
#else
                sb.Append(qi.CommandText);
                sb.AppendLine();
#endif
			}
			return sb.ToString();
		}


		/// <summary>
		/// Gets the default object reader factory.
		/// </summary>
		/// <param name="rowType">Type of the row.</param>
		/// <returns></returns>
		IObjectReaderFactory IProvider.GetDefaultFactory(MetaType rowType)
		{
			return this.GetDefaultFactory(rowType);
		}


		/// <summary>
		/// Compiles the sub query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		ICompiledSubQuery IProvider.CompileSubQuery(SqlNode query, Type elementType, IReadOnlyCollection<System.Data.Linq.Provider.NodeTypes.SqlParameter> parameters)
		{
			return this.CompileSubQuery(query, elementType, parameters as ReadOnlyCollection<System.Data.Linq.Provider.NodeTypes.SqlParameter>);
		}
		

		/// <summary>
		/// Executes the query specified as a LINQ expression tree.
		/// </summary>
		/// <param name="query"></param>
		/// <returns>
		/// A result object from which you can obtain the return value and output parameters.
		/// </returns>
		IExecuteResult IProvider.Execute(Expression query)
		{
			return this.Execute(query);
		}


		/// <summary>
		/// Executes the specified query. Used from compiled queries. 
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="queryInfo">The query information.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="parentArgs">The parent arguments.</param>
		/// <param name="userArgs">The user arguments.</param>
		/// <param name="subQueries">The sub queries.</param>
		/// <param name="lastResult">The last result.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Unknown reason.")]
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		IExecuteResult IProvider.Execute(Expression query, QueryInfo queryInfo, IObjectReaderFactory factory, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries, object lastResult)
		{
			return this.Execute(query, queryInfo, factory, parentArgs, userArgs, subQueries, lastResult);
		}


		/// <summary>
		/// Executes all queries
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="queryInfos">The query infos.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="userArguments">The user arguments.</param>
		/// <param name="subQueries">The sub queries.</param>
		/// <returns></returns>
		IExecuteResult IProvider.ExecuteAll(Expression query, QueryInfo[] queryInfos, IObjectReaderFactory factory, object[] userArguments, ICompiledSubQuery[] subQueries)
		{
			return this.ExecuteAll(query, queryInfos, factory, userArguments, subQueries);
		}


		/// <summary>
		/// Executes the query specified as a LINQ expression tree.
		/// </summary>
		/// <param name="query"></param>
		/// <returns>
		/// A result object from which you can obtain the return value and output parameters.
		/// </returns>
		private IExecuteResult Execute(Expression query)
		{
			this.CheckDispose();
			this.CheckInitialized();
			this.CheckNotDeleted();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			this.InitializeProviderMode();
			query = Funcletizer.Funcletize(query);

			if(this.EnableCacheLookup)
			{
				IExecuteResult cached = this.GetCachedResult(query);
				if(cached != null)
				{
					return cached;
				}
			}
			SqlNodeAnnotations annotations = new SqlNodeAnnotations();
			QueryInfo[] qis = this.BuildQuery(query, annotations);
			CheckSqlCompatibility(qis, annotations);

			LambdaExpression lambda = query as LambdaExpression;
			if(lambda != null)
			{
				query = lambda.Body;
			}

			IObjectReaderFactory factory = null;
			ICompiledSubQuery[] subQueries = null;
			QueryInfo qi = qis[qis.Length - 1];
			if(qi.ResultShape == ResultShape.Singleton)
			{
				subQueries = this.CompileSubQueries(qi.Query);
				factory = this.GetReaderFactory(qi.Query, qi.ResultType);
			}
			else if(qi.ResultShape == ResultShape.Sequence)
			{
				subQueries = this.CompileSubQueries(qi.Query);
				factory = this.GetReaderFactory(qi.Query, TypeSystem.GetElementType(qi.ResultType));
			}

			IExecuteResult result = this.ExecuteAll(query, qis, factory, null, subQueries);
			return result;
		}


		/// <summary>
		/// Executes the specified query. Used from compiled queries. 
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="queryInfo">The query information.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="parentArgs">The parent arguments.</param>
		/// <param name="userArgs">The user arguments.</param>
		/// <param name="subQueries">The sub queries.</param>
		/// <param name="lastResult">The last result.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Unknown reason.")]
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private IExecuteResult Execute(Expression query, QueryInfo queryInfo, IObjectReaderFactory factory, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries, object lastResult)
		{
			this.InitializeProviderMode();

			DbConnection con = _conManager.UseConnection(this);
			try
			{
				DbCommand cmd = con.CreateCommand();
				cmd.CommandText = queryInfo.CommandText;
				cmd.Transaction = _conManager.Transaction;
				cmd.CommandTimeout = _commandTimeout;
				AssignParameters(cmd, queryInfo.Parameters, userArgs, lastResult);
				LogCommand(_log, cmd);
				_queryCount += 1;

				switch(queryInfo.ResultShape)
				{
					default:
					case ResultShape.Return:
						{
							return new ExecuteResult(cmd, queryInfo.Parameters, null, cmd.ExecuteNonQuery(), true);
						}
					case ResultShape.Singleton:
						{
							DbDataReader reader = cmd.ExecuteReader();
							IObjectReader objReader = factory.Create(reader, true, this, parentArgs, userArgs, subQueries);
							_conManager.UseConnection(objReader.Session);
							try
							{
								IEnumerable sequence = (IEnumerable)Activator.CreateInstance(
									typeof(OneTimeEnumerable<>).MakeGenericType(queryInfo.ResultType),
									BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
									new object[] { objReader }, null
									);
								object value = null;
								MethodCallExpression mce = query as MethodCallExpression;
								MethodInfo sequenceMethod = null;
								if(mce != null && (
									mce.Method.DeclaringType == typeof(Queryable) ||
									mce.Method.DeclaringType == typeof(Enumerable))
									)
								{
									switch(mce.Method.Name)
									{
										case "First":
										case "FirstOrDefault":
										case "SingleOrDefault":
											sequenceMethod = TypeSystem.FindSequenceMethod(mce.Method.Name, sequence);
											break;
										case "Single":
										default:
											sequenceMethod = TypeSystem.FindSequenceMethod("Single", sequence);
											break;
									}
								}
								else
								{
									sequenceMethod = TypeSystem.FindSequenceMethod("SingleOrDefault", sequence);
								}

								// When dynamically invoking the sequence method, we want to
								// return the inner exception if the invocation fails
								if(sequenceMethod != null)
								{
									try
									{
										value = sequenceMethod.Invoke(null, new object[] { sequence });
									}
									catch(TargetInvocationException tie)
									{
										if(tie.InnerException != null)
										{
											throw tie.InnerException;
										}
										throw;
									}
								}

								return new ExecuteResult(cmd, queryInfo.Parameters, objReader.Session, value);
							}
							finally
							{
								objReader.Dispose();
							}
						}
					case ResultShape.Sequence:
						{
							DbDataReader reader = cmd.ExecuteReader();
							IObjectReader objReader = factory.Create(reader, true, this, parentArgs, userArgs, subQueries);
							_conManager.UseConnection(objReader.Session);
							IEnumerable sequence = (IEnumerable)Activator.CreateInstance(
								typeof(OneTimeEnumerable<>).MakeGenericType(TypeSystem.GetElementType(queryInfo.ResultType)),
								BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
								new object[] { objReader }, null
								);
							if(typeof(IQueryable).IsAssignableFrom(queryInfo.ResultType))
							{
								sequence = sequence.AsQueryable();
							}
							ExecuteResult result = new ExecuteResult(cmd, queryInfo.Parameters, objReader.Session);
							MetaFunction function = this.GetFunction(query);
							if(function != null && !function.IsComposable)
							{
								sequence = (IEnumerable)Activator.CreateInstance(
								typeof(SingleResult<>).MakeGenericType(TypeSystem.GetElementType(queryInfo.ResultType)),
								BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
								new object[] { sequence, result, _services.Context }, null
								);
							}
							result.ReturnValue = sequence;
							return result;
						}
					case ResultShape.MultipleResults:
						{
							DbDataReader reader = cmd.ExecuteReader();
							IObjectReaderSession session = _readerCompiler.CreateSession(reader, this, parentArgs, userArgs, subQueries);
							_conManager.UseConnection(session);
							MetaFunction function = this.GetFunction(query);
							ExecuteResult result = new ExecuteResult(cmd, queryInfo.Parameters, session);
							result.ReturnValue = new MultipleResults(this, function, session, result);
							return result;
						}
				}
			}
			finally
			{
				_conManager.ReleaseConnection(this);
			}
		}


		private IExecuteResult ExecuteAll(Expression query, QueryInfo[] queryInfos, IObjectReaderFactory factory, object[] userArguments, ICompiledSubQuery[] subQueries)
		{
			IExecuteResult result = null;
			object lastResult = null;
			for(int i = 0, n = queryInfos.Length; i < n; i++)
			{
				if(i < n - 1)
				{
					result = this.Execute(query, queryInfos[i], null, null, userArguments, subQueries, lastResult);
				}
				else
				{
					result = this.Execute(query, queryInfos[i], factory, null, userArguments, subQueries, lastResult);
				}
				if(queryInfos[i].ResultShape == ResultShape.Return)
				{
					lastResult = result.ReturnValue;
				}
			}
			return result;
		}
		

		DbCommand IProvider.GetCommand(Expression query)
		{
			this.CheckDispose();
			this.CheckInitialized();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			this.InitializeProviderMode();
			SqlNodeAnnotations annotations = new SqlNodeAnnotations();
			QueryInfo[] qis = this.BuildQuery(query, annotations);
			QueryInfo qi = qis[qis.Length - 1];
			DbCommand cmd = _conManager.Connection.CreateCommand();
			cmd.CommandText = qi.CommandText;
			cmd.Transaction = _conManager.Transaction;
			cmd.CommandTimeout = _commandTimeout;
			AssignParameters(cmd, qi.Parameters, null, null);
			return cmd;
		}

		private ResultShape GetResultShape(Expression query)
		{
			LambdaExpression lambda = query as LambdaExpression;
			if(lambda != null)
			{
				query = lambda.Body;
			}

			if(query.Type == typeof(void))
			{
				return ResultShape.Return;
			}
			else if(query.Type == typeof(IMultipleResults))
			{
				return ResultShape.MultipleResults;
			}

			bool isSequence = typeof(IEnumerable).IsAssignableFrom(query.Type);
			ProviderType pt = _typeProvider.From(query.Type);
			bool isScalar = !pt.IsRuntimeOnlyType && !pt.IsApplicationType;
			bool isSingleton = isScalar || !isSequence;

			MethodCallExpression mce = query as MethodCallExpression;
			if(mce != null)
			{
				// query operators
				if(mce.Method.DeclaringType == typeof(Queryable) ||
					mce.Method.DeclaringType == typeof(Enumerable))
				{
					switch(mce.Method.Name)
					{
						// methods known to produce singletons
						case "First":
						case "FirstOrDefault":
						case "Single":
						case "SingleOrDefault":
							isSingleton = true;
							break;
					}
				}
				else if(mce.Method.DeclaringType == typeof(DataContext))
				{
					if(mce.Method.Name == "ExecuteCommand")
					{
						return ResultShape.Return;
					}
				}
				else if(mce.Method.DeclaringType.IsSubclassOf(typeof(DataContext)))
				{
					MetaFunction f = this.GetFunction(query);
					if(f != null)
					{
						if(!f.IsComposable)
						{
							isSingleton = false;
						}
						else if(isScalar)
						{
							isSingleton = true;
						}
					}
				}
				else if(mce.Method.DeclaringType == typeof(DMLMethodPlaceholders) && mce.Method.ReturnType == typeof(int))
				{
					return ResultShape.Return;
				}
			}

			if(isSingleton)
			{
				return ResultShape.Singleton;
			}
			else if(isScalar)
			{
				return ResultShape.Return;
			}
			else
			{
				return ResultShape.Sequence;
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private Type GetResultType(Expression query)
		{
			LambdaExpression lambda = query as LambdaExpression;
			if(lambda != null)
			{
				query = lambda.Body;
			}
			return query.Type;
		}

		internal QueryInfo[] BuildQuery(Expression query, SqlNodeAnnotations annotations)
		{
			this.CheckDispose();

			// apply maximal funcletization
			query = Funcletizer.Funcletize(query);

			// convert query nodes into sql nodes
			QueryConverter converter = new QueryConverter(_services, _typeProvider, _translator, _sqlFactory);
			switch(this.Mode)
			{
				case SqlServerProviderMode.Sql2000:
					converter.ConverterStrategy =
						ConverterStrategy.CanUseScopeIdentity |
						ConverterStrategy.CanUseJoinOn |
						ConverterStrategy.CanUseRowStatus;
					break;
				case SqlServerProviderMode.Sql2005:
				case SqlServerProviderMode.Sql2008:
					converter.ConverterStrategy =
						ConverterStrategy.CanUseScopeIdentity |
						ConverterStrategy.SkipWithRowNumber |
						ConverterStrategy.CanUseRowStatus |
						ConverterStrategy.CanUseJoinOn |
						ConverterStrategy.CanUseOuterApply |
						ConverterStrategy.CanOutputFromInsert;
					break;
				case SqlServerProviderMode.SqlCE:
					converter.ConverterStrategy = ConverterStrategy.CanUseOuterApply;
					// Can't set ConverterStrategy.CanUseJoinOn because scalar subqueries in the ON clause
					// can't be converted into anything.
					break;
			}
			SqlNode node = converter.ConvertOuter(query);

			return this.BuildQuery(this.GetResultShape(query), this.GetResultType(query), node, null, annotations);
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		private QueryInfo[] BuildQuery(ResultShape resultShape, Type resultType, SqlNode node, ReadOnlyCollection<System.Data.Linq.Provider.NodeTypes.SqlParameter> parentParameters, SqlNodeAnnotations annotations)
		{
			System.Diagnostics.Debug.Assert(resultType != null);
			System.Diagnostics.Debug.Assert(node != null);

			SqlSupersetValidator validator = new SqlSupersetValidator();

			// These are the rules that apply to every SQL tree.
			if(_checkQueries)
			{
				validator.AddValidator(new ColumnTypeValidator()); /* Column CLR Type must agree with its Expressions CLR Type */
				validator.AddValidator(new LiteralValidator()); /* Constrain literal Types */
			}

			validator.Validate(node);

			SqlColumnizer columnizer = new SqlColumnizer();

			// resolve member references
			bool canUseOuterApply = (this.Mode == SqlServerProviderMode.Sql2005 || this.Mode == SqlServerProviderMode.Sql2008 || this.Mode == SqlServerProviderMode.SqlCE);
			SqlBinder binder = new SqlBinder(_translator, _sqlFactory, _services.Model, _services.Context.LoadOptions, columnizer, canUseOuterApply);
			binder.OptimizeLinkExpansions = (_optimizationFlags & OptimizationFlags.OptimizeLinkExpansions) != 0;
			binder.SimplifyCaseStatements = (_optimizationFlags & OptimizationFlags.SimplifyCaseStatements) != 0;
			binder.PreBinder = delegate(SqlNode n)
			{
				// convert methods into known reversable operators
				return PreBindDotNetConverter.Convert(n, _sqlFactory, _services.Model);
			};
			node = binder.Bind(node);
			if(_checkQueries)
			{
				validator.AddValidator(new ExpectNoAliasRefs());
				validator.AddValidator(new ExpectNoSharedExpressions());
			}
			validator.Validate(node);

			node = PostBindDotNetConverter.Convert(node, _sqlFactory, this.Mode);

			// identify true flow of sql data types 
			SqlRetyper retyper = new SqlRetyper(new SqlFactory(_typeProvider, _services.Model));
			node = retyper.Retype(node);
			validator.Validate(node);

			// change CONVERT to special conversions like UNICODE,CHAR,...
			SqlTypeConverter converter = new SqlTypeConverter(_sqlFactory);
			node = converter.Visit(node);
			validator.Validate(node);

			// transform type-sensitive methods such as LEN (to DATALENGTH), ...
			SqlMethodTransformer methodTransformer = new SqlMethodTransformer(_sqlFactory);
			node = methodTransformer.Visit(node);
			validator.Validate(node);

			// convert multisets into separate queries
			SqlMultiplexerOptionType options = (this.Mode == SqlServerProviderMode.Sql2008 ||
											  this.Mode == SqlServerProviderMode.Sql2005 ||
											  this.Mode == SqlServerProviderMode.SqlCE)
				? SqlMultiplexerOptionType.EnableBigJoin : SqlMultiplexerOptionType.None;
			SqlMultiplexer mux = new SqlMultiplexer(options, parentParameters, _sqlFactory);
			node = mux.Multiplex(node);
			validator.Validate(node);

			// convert object construction expressions into flat row projections
			SqlFlattener flattener = new SqlFlattener(_sqlFactory, columnizer);
			node = flattener.Flatten(node);
			validator.Validate(node);

			if(_mode == SqlServerProviderMode.SqlCE)
			{
				SqlRewriteScalarSubqueries rss = new SqlRewriteScalarSubqueries(_sqlFactory);
				node = rss.Rewrite(node);
			}

			// Simplify case statements where all alternatives map to the same thing.
			// Doing this before deflator because the simplified results may lead to
			// more deflation opportunities.
			// Doing this before booleanizer because it may convert CASE statements (non-predicates) into
			// predicate expressions.
			// Doing this before reorderer because it may reduce some orders to constant nodes which should not
			// be passed onto ROW_NUMBER.
			node = SqlCaseSimplifier.Simplify(node, _sqlFactory);

			// Rewrite order-by clauses so that they only occur at the top-most select 
			// or in selects with TOP
			SqlReorderer reorderer = new SqlReorderer(_typeProvider, _sqlFactory);
			node = reorderer.Reorder(node);
			validator.Validate(node);

			// Inject code to turn predicates into bits, and bits into predicates where necessary
			node = SqlBooleanizer.Rationalize(node, _typeProvider, _services.Model);
			if(_checkQueries)
			{
				validator.AddValidator(new ExpectRationalizedBooleans()); /* From now on all boolean expressions should remain rationalized. */
			}
			validator.Validate(node);

			if(_checkQueries)
			{
				validator.AddValidator(new ExpectNoFloatingColumns());
			}

			// turning predicates into bits/ints can change Sql types, propagate changes            
			node = retyper.Retype(node);
			validator.Validate(node);

			// assign aliases to columns
			// we need to do this now so that the sql2k lifters will work
			SqlAliaser aliaser = new SqlAliaser();
			node = aliaser.AssociateColumnsWithAliases(node);
			validator.Validate(node);

			// SQL2K enablers.
			node = SqlLiftWhereClauses.Lift(node, new SqlFactory(_typeProvider, _services.Model));
			node = SqlLiftIndependentRowExpressions.Lift(node);
			node = SqlOuterApplyReducer.Reduce(node, _sqlFactory, annotations);
			node = SqlTopReducer.Reduce(node, annotations, _sqlFactory);

			// resolve references to columns in other scopes by adding them
			// to the intermediate selects
			SqlResolver resolver = new SqlResolver();
			node = resolver.Resolve(node);
			validator.Validate(node);

			// re-assign aliases after resolving (new columns may have been added)
			node = aliaser.AssociateColumnsWithAliases(node);
			validator.Validate(node);

			// fixup union projections
			node = SqlUnionizer.Unionize(node);

			// remove order-by of literals
			node = SqlRemoveConstantOrderBy.Remove(node);

			// throw out unused columns and redundant sub-queries...
			SqlDeflator deflator = new SqlDeflator();
			node = deflator.Deflate(node);
			validator.Validate(node);

			// Positioning after deflator because it may remove unnecessary columns
			// from SELECT projection lists and allow more CROSS APPLYs to be reduced
			// to CROSS JOINs.
			node = SqlCrossApplyToCrossJoin.Reduce(node, annotations);

			// fixup names for aliases, columns, locals, etc..
			SqlNamer namer = new SqlNamer();
			node = namer.AssignNames(node);
			validator.Validate(node);

			// Convert [N]Text,Image to [N]VarChar(MAX),VarBinary(MAX) where necessary.
			// These new types do not exist on SQL2k, so add annotations.
			LongTypeConverter longTypeConverter = new LongTypeConverter(_sqlFactory);
			node = longTypeConverter.AddConversions(node, annotations);

			// final validation            
			validator.AddValidator(new ExpectNoMethodCalls());
			validator.AddValidator(new ValidateNoInvalidComparison());
			validator.Validate(node);

			SqlParameterizer parameterizer = new SqlParameterizer(_typeProvider, annotations);
			SqlFormatter formatter = new SqlFormatter();
			if(_mode == SqlServerProviderMode.SqlCE ||
				_mode == SqlServerProviderMode.Sql2005 ||
				_mode == SqlServerProviderMode.Sql2008)
			{
				formatter.ParenthesizeTop = true;
			}

			SqlBlock block = node as SqlBlock;
			if(block != null && _mode == SqlServerProviderMode.SqlCE)
			{
				// SQLCE cannot batch multiple statements.
				ReadOnlyCollection<ReadOnlyCollection<SqlParameterInfo>> parameters = parameterizer.ParameterizeBlock(block);
				string[] commands = formatter.FormatBlock(block, false);
				QueryInfo[] queries = new QueryInfo[commands.Length];
				for(int i = 0, n = commands.Length; i < n; i++)
				{
					queries[i] = new QueryInfo(
						block.Statements[i],
						commands[i],
						parameters[i],
						(i < n - 1) ? ResultShape.Return : resultShape,
						(i < n - 1) ? typeof(int) : resultType
						);
				}
				return queries;
			}
			else
			{
				// build only one result
				ReadOnlyCollection<SqlParameterInfo> parameters = parameterizer.Parameterize(node);
				string commandText = formatter.Format(node);
				return new QueryInfo[] {
                    new QueryInfo(node, commandText, parameters, resultShape, resultType)
                    };
			}
		}

		private SqlSelect GetFinalSelect(SqlNode node)
		{
			switch(node.NodeType)
			{
				case SqlNodeType.Select:
					return (SqlSelect)node;
				case SqlNodeType.Block:
					{
						SqlBlock b = (SqlBlock)node;
						return GetFinalSelect(b.Statements[b.Statements.Count - 1]);
					}
			}
			return null;
		}

		private IObjectReaderFactory GetReaderFactory(SqlNode node, Type elemType)
		{
			SqlSelect sel = node as SqlSelect;
			SqlExpression projection = null;
			if(sel == null && node.NodeType == SqlNodeType.Block)
			{
				sel = this.GetFinalSelect(node);
			}
			if(sel != null)
			{
				projection = sel.Selection;
			}
			else
			{
				SqlUserQuery suq = node as SqlUserQuery;
				if(suq != null && suq.Projection != null)
				{
					projection = suq.Projection;
				}
			}
			IObjectReaderFactory factory;
			if(projection != null)
			{
				factory = _readerCompiler.Compile(projection, elemType);
			}
			else
			{
				return this.GetDefaultFactory(_services.Model.GetMetaType(elemType));
			}
			return factory;
		}

		private IObjectReaderFactory GetDefaultFactory(MetaType rowType)
		{
			if(rowType == null)
			{
				throw Error.ArgumentNull("rowType");
			}
			SqlNodeAnnotations annotations = new SqlNodeAnnotations();
			Expression tmp = Expression.Constant(null);
			SqlUserQuery suq = new SqlUserQuery(string.Empty, null, null, tmp);
			if(TypeSystem.IsSimpleType(rowType.Type))
			{
				// if the element type is a simple type (int, bool, etc.) we create
				// a single column binding
				SqlUserColumn col = new SqlUserColumn(rowType.Type, _typeProvider.From(rowType.Type), suq, "", false, suq.SourceExpression);
				suq.Columns.Add(col);
				suq.Projection = col;
			}
			else
			{
				// ... otherwise we generate a default projection
				SqlUserRow rowExp = new SqlUserRow(rowType.InheritanceRoot, _typeProvider.GetApplicationType((int)ConverterSpecialTypes.Row), suq, tmp);
				suq.Projection = _translator.BuildProjection(rowExp, rowType, true, null, tmp);
			}
			Type resultType = TypeSystem.GetSequenceType(rowType.Type);
			QueryInfo[] qis = this.BuildQuery(ResultShape.Sequence, resultType, suq, null, annotations);
			return this.GetReaderFactory(qis[qis.Length - 1].Query, rowType.Type);
		}


		private void SetProviderMode(Enum value)
		{
			if(value == null)
			{
				return;
			}
			if(!(value is SqlServerProviderMode))
			{
				throw Error.ArgumentTypeMismatch("ProviderMode value");
			}
			if(!Enum.IsDefined(typeof(SqlServerProviderMode), value))
			{
				throw Error.ArgumentOutOfRange("ProviderMode");
			}
			_mode = (SqlServerProviderMode)value;
		}


		private void InitializeProviderMode()
		{
			if(_typeProvider == null)
			{
				switch(_mode)
				{
					case SqlServerProviderMode.Sql2000:
						_typeProvider = SqlTypeSystem.Create2000Provider();
						break;
					case SqlServerProviderMode.Sql2005:
						_typeProvider = SqlTypeSystem.Create2005Provider();
						break;
					case SqlServerProviderMode.Sql2008:
						_typeProvider = SqlTypeSystem.Create2008Provider();
						break;
					case SqlServerProviderMode.SqlCE:
						_typeProvider = SqlTypeSystem.CreateCEProvider();
						break;
					default:
						System.Diagnostics.Debug.Assert(false);
						break;
				}
			}
			if(_sqlFactory == null)
			{
				_sqlFactory = new SqlFactory(_typeProvider, _services.Model);
				_translator = new Translator(_services, _sqlFactory, _typeProvider);
			}
		}

		#region Class Property Declarations
		internal SqlServerProviderMode Mode
		{
			get
			{
				this.CheckDispose();
				this.CheckInitialized();
				this.InitializeProviderMode();
				return _mode;
			}
		}

		Enum IProvider.ProviderMode
		{
			get { return _mode; }
			set { SetProviderMode(value); }
		}

		DbConnection IProvider.Connection
		{
			get
			{
				this.CheckDispose();
				this.CheckInitialized();
				return _conManager.Connection;
			}
		}

		TextWriter IProvider.Log
		{
			get
			{
				this.CheckDispose();
				this.CheckInitialized();
				return _log;
			}
			set
			{
				this.CheckDispose();
				this.CheckInitialized();
				_log = value;
			}
		}

		DbTransaction IProvider.Transaction
		{
			get
			{
				this.CheckDispose();
				this.CheckInitialized();
				return _conManager.Transaction;
			}
			set
			{
				this.CheckDispose();
				this.CheckInitialized();
				_conManager.Transaction = value;
			}
		}

		int IProvider.CommandTimeout
		{
			get
			{
				this.CheckDispose();
				return _commandTimeout;
			}
			set
			{
				this.CheckDispose();
				_commandTimeout = value;
			}
		}

		/// <summary>
		/// Expose a test hook which controls which SQL optimizations are executed.
		/// </summary>
		internal OptimizationFlags OptimizationFlags
		{
			get
			{
				CheckDispose();
				return _optimizationFlags;
			}
			set
			{
				CheckDispose();
				_optimizationFlags = value;
			}
		}

		/// <summary>
		/// Validate queries as they are generated.
		/// </summary>
		internal bool CheckQueries
		{
			get
			{
				CheckDispose();
				return _checkQueries;
			}
			set
			{
				CheckDispose();
				_checkQueries = value;
			}
		}

		internal bool EnableCacheLookup
		{
			get
			{
				CheckDispose();
				return _enableCacheLookup;
			}
			set
			{
				CheckDispose();
				_enableCacheLookup = value;
			}
		}

		internal int QueryCount
		{
			get
			{
				CheckDispose();
				return _queryCount;
			}
		}

		internal int MaxUsers
		{
			get
			{
				CheckDispose();
				return _conManager.MaxUsers;
			}
		}

		IDataServices IReaderProvider.Services
		{
			get { return _services; }
		}

		IConnectionManager IReaderProvider.ConnectionManager
		{
			get { return _conManager; }
		}
		#endregion
	}
}
