using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using Linq;
	using System.Diagnostics.CodeAnalysis;
	using System.Data.Linq.BindingLists;


	/// <summary>
	/// The DataContext is the source of all entities mapped over a database connection.
	/// It tracks changes made to all retrieved entities and maintains an 'identity cache' 
	/// that guarantees that entities retrieved more than once are represented using the 
	/// same object instance.
	/// </summary>
	public class DataContext : IDisposable
	{
		#region Member Declarations
		private CommonDataServices _services;
		private IProvider _provider;
		private Dictionary<MetaTable, ITable> _tables;
		private bool _objectTrackingEnabled = true;
		private bool _deferredLoadingEnabled = true;
		private bool _disposed;
		private bool _isInSubmitChanges;
		private DataLoadOptions _loadOptions;
		private ChangeConflictCollection _conflicts;
		private Enum _perInstanceProviderMode;
		#endregion

		#region Statics
		/// <summary>
		/// General provider mode for all DataContext instances. Set this static member to have all instances use the same value. 
		/// </summary>
		public static Enum ProviderMode;
		#endregion

		/// <summary>
		/// Prevents a default instance of the <see cref="DataContext"/> class from being created.
		/// </summary>
		private DataContext()
		{
		}

		public DataContext(string fileOrServerOrConnection)
		{
			if(fileOrServerOrConnection == null)
			{
				throw Error.ArgumentNull("fileOrServerOrConnection");
			}
			this.InitWithDefaultMapping(fileOrServerOrConnection);
		}

		public DataContext(string fileOrServerOrConnection, MappingSource mapping)
		{
			if(fileOrServerOrConnection == null)
			{
				throw Error.ArgumentNull("fileOrServerOrConnection");
			}
			if(mapping == null)
			{
				throw Error.ArgumentNull("mapping");
			}
			this.Init(fileOrServerOrConnection, mapping);
		}

		public DataContext(IDbConnection connection)
		{
			if(connection == null)
			{
				throw Error.ArgumentNull("connection");
			}
			this.InitWithDefaultMapping(connection);
		}

		public DataContext(IDbConnection connection, MappingSource mapping)
		{
			if(connection == null)
			{
				throw Error.ArgumentNull("connection");
			}
			if(mapping == null)
			{
				throw Error.ArgumentNull("mapping");
			}
			this.Init(connection, mapping);
		}

		internal DataContext(DataContext context)
		{
			if(context == null)
			{
				throw Error.ArgumentNull("context");
			}
			this.Init(context.Connection, context.Mapping.MappingSource);
			this.LoadOptions = context.LoadOptions;
			this.Transaction = context.Transaction;
			this.Log = context.Log;
			this.CommandTimeout = context.CommandTimeout;
		}

		#region Dispose\Finalize
		public void Dispose()
		{
			this._disposed = true;
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
				if(this._provider != null)
				{
					this._provider.Dispose();
					this._provider = null;
				}
				this._services = null;
				this._tables = null;
				this._loadOptions = null;
			}
		}

		internal void CheckDispose()
		{
			if(this._disposed)
			{
				throw Error.DataContextCannotBeUsedAfterDispose();
			}
		}
		#endregion

		private void InitWithDefaultMapping(object connection)
		{
			this.Init(connection, new AttributeMappingSource());
		}

		internal object Clone()
		{
			CheckDispose();
			return Activator.CreateInstance(this.GetType(), new object[] { this.Connection, this.Mapping.MappingSource });
		}

		private void Init(object connection, MappingSource mapping)
		{
			_perInstanceProviderMode = DataContext.ProviderMode;
			MetaModel model = mapping.GetModel(this.GetType());
			this._services = new CommonDataServices(this, model);
			this._conflicts = new ChangeConflictCollection();

			// determine provider
			Type providerType;
			if(model.ProviderType != null)
			{
				providerType = model.ProviderType;
			}
			else
			{
				throw Error.ProviderTypeNull();
			}

			if(!typeof(IProvider).IsAssignableFrom(providerType))
			{
				throw Error.ProviderDoesNotImplementRequiredInterface(providerType, typeof(IProvider));
			}

			this._provider = (IProvider)Activator.CreateInstance(providerType);
			SetProviderMode();
			this._provider.Initialize(this._services, connection);

			this._tables = new Dictionary<MetaTable, ITable>();
			this.InitTables(this);
		}

		private void SetProviderMode()
		{
			if(_provider == null)
			{
				return;
			}
			_provider.ProviderMode = _perInstanceProviderMode;
		}

		internal void ClearCache()
		{
			CheckDispose();
			this._services.ResetServices();
		}

		internal CommonDataServices Services
		{
			get
			{
				CheckDispose();
				return this._services;
			}
		}

		/// <summary>
		/// The connection object used by this DataContext when executing queries and commands.
		/// </summary>
		public DbConnection Connection
		{
			get
			{
				CheckDispose();
				return this._provider.Connection;
			}
		}

		/// <summary>
		/// The transaction object used by this DataContext when executing queries and commands.
		/// </summary>
		public DbTransaction Transaction
		{
			get
			{
				CheckDispose();
				return this._provider.Transaction;
			}
			set
			{
				CheckDispose();
				this._provider.Transaction = value;
			}
		}

		/// <summary>
		/// The command timeout to use when executing commands.
		/// </summary>
		public int CommandTimeout
		{
			get
			{
				CheckDispose();
				return this._provider.CommandTimeout;
			}
			set
			{
				CheckDispose();
				this._provider.CommandTimeout = value;
			}
		}

		/// <summary>
		/// A text writer used by this DataContext to output information such as query and commands
		/// being executed.
		/// </summary>
		public TextWriter Log
		{
			get
			{
				CheckDispose();
				return this._provider.Log;
			}
			set
			{
				CheckDispose();
				this._provider.Log = value;
			}
		}

		/// <summary>
		/// True if object tracking is enabled, false otherwise.  Object tracking
		/// includes identity caching and change tracking.  If tracking is turned off, 
		/// SubmitChanges and related functionality is disabled.  DeferredLoading is
		/// also disabled when object tracking is disabled.
		/// </summary>
		public bool ObjectTrackingEnabled
		{
			get
			{
				CheckDispose();
				return _objectTrackingEnabled;
			}
			set
			{
				CheckDispose();
				if(Services.HasCachedObjects)
				{
					throw Error.OptionsCannotBeModifiedAfterQuery();
				}
				_objectTrackingEnabled = value;
				if(!_objectTrackingEnabled)
				{
					_deferredLoadingEnabled = false;
				}
				// force reinitialization of cache/tracking objects
				_services.ResetServices();
			}
		}

		/// <summary>
		/// True if deferred loading is enabled, false otherwise.  With deferred
		/// loading disabled, association members return default values and are 
		/// not defer loaded.
		/// </summary>
		public bool DeferredLoadingEnabled
		{
			get
			{
				CheckDispose();
				return _deferredLoadingEnabled;
			}
			set
			{
				CheckDispose();
				if(Services.HasCachedObjects)
				{
					throw Error.OptionsCannotBeModifiedAfterQuery();
				}
				// can't have tracking disabled and deferred loading enabled
				if(!ObjectTrackingEnabled && value)
				{
					throw Error.DeferredLoadingRequiresObjectTracking();
				}
				_deferredLoadingEnabled = value;
			}
		}

		/// <summary>
		/// The mapping model used to describe the entities
		/// </summary>
		public MetaModel Mapping
		{
			get
			{
				CheckDispose();
				return this._services.Model;
			}
		}

		/// <summary>
		/// Verify that change tracking is enabled, and throw an exception
		/// if it is not.
		/// </summary>
		internal void VerifyTrackingEnabled()
		{
			CheckDispose();
			if(!ObjectTrackingEnabled)
			{
				throw Error.ObjectTrackingRequired();
			}
		}

		/// <summary>
		/// Verify that submit changes is not occurring
		/// </summary>
		internal void CheckNotInSubmitChanges()
		{
			CheckDispose();
			if(this._isInSubmitChanges)
			{
				throw Error.CannotPerformOperationDuringSubmitChanges();
			}
		}

		/// <summary>
		/// Verify that submit changes is occurring
		/// </summary>
		internal void CheckInSubmitChanges()
		{
			CheckDispose();
			if(!this._isInSubmitChanges)
			{
				throw Error.CannotPerformOperationOutsideSubmitChanges();
			}
		}

		/// <summary>
		/// Returns the strongly-typed Table object representing a collection of persistent entities.  
		/// Use this collection as the starting point for queries.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity objects. In case of a persistent hierarchy
		/// the entity specified must be the base type of the hierarchy.</typeparam>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		public Table<TEntity> GetTable<TEntity>() where TEntity : class
		{
			CheckDispose();
			MetaTable metaTable = this._services.Model.GetTable(typeof(TEntity));
			if(metaTable == null)
			{
				throw Error.TypeIsNotMarkedAsTable(typeof(TEntity));
			}
			ITable table = this.GetTable(metaTable);
			if(table.ElementType != typeof(TEntity))
			{
				throw Error.CouldNotGetTableForSubtype(typeof(TEntity), metaTable.RowType.Type);
			}
			return (Table<TEntity>)table;
		}

		/// <summary>
		/// Returns the weakly-typed ITable object representing a collection of persistent entities. 
		/// Use this collection as the starting point for dynamic/runtime-computed queries.
		/// </summary>
		/// <param name="type">The type of the entity objects. In case of a persistent hierarchy
		/// the entity specified must be the base type of the hierarchy.</param>
		/// <returns></returns>
		public ITable GetTable(Type type)
		{
			CheckDispose();
			if(type == null)
			{
				throw Error.ArgumentNull("type");
			}
			MetaTable metaTable = this._services.Model.GetTable(type);
			if(metaTable == null)
			{
				throw Error.TypeIsNotMarkedAsTable(type);
			}
			if(metaTable.RowType.Type != type)
			{
				throw Error.CouldNotGetTableForSubtype(type, metaTable.RowType.Type);
			}
			return this.GetTable(metaTable);
		}

		private ITable GetTable(MetaTable metaTable)
		{
			System.Diagnostics.Debug.Assert(metaTable != null);
			ITable tb;
			if(!this._tables.TryGetValue(metaTable, out tb))
			{
				ValidateTable(metaTable);
				Type tbType = typeof(Table<>).MakeGenericType(metaTable.RowType.Type);
				tb = (ITable)Activator.CreateInstance(tbType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { this, metaTable }, null);
				this._tables.Add(metaTable, tb);
			}
			return tb;
		}

		private static void ValidateTable(MetaTable metaTable)
		{
			// Associations can only be between entities - verify both that both ends of all
			// associations are entities.
			foreach(MetaAssociation assoc in metaTable.RowType.Associations)
			{
				if(!assoc.ThisMember.DeclaringType.IsEntity)
				{
					throw Error.NonEntityAssociationMapping(assoc.ThisMember.DeclaringType.Type, assoc.ThisMember.Name, assoc.ThisMember.DeclaringType.Type);
				}
				if(!assoc.OtherType.IsEntity)
				{
					throw Error.NonEntityAssociationMapping(assoc.ThisMember.DeclaringType.Type, assoc.ThisMember.Name, assoc.OtherType.Type);
				}
			}
		}

		private void InitTables(object schema)
		{
			FieldInfo[] fields = schema.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
			foreach(FieldInfo fi in fields)
			{
				Type ft = fi.FieldType;
				if(ft.IsGenericType && ft.GetGenericTypeDefinition() == typeof(Table<>))
				{
					ITable tb = (ITable)fi.GetValue(schema);
					if(tb == null)
					{
						Type rowType = ft.GetGenericArguments()[0];
						tb = this.GetTable(rowType);
						fi.SetValue(schema, tb);
					}
				}
			}
		}

		/// <summary>
		/// Internal method that can be accessed by tests to retrieve the provider
		/// The IProvider result can then be cast to the actual provider to call debug methods like
		///   CheckQueries, QueryCount, EnableCacheLookup
		/// </summary>
		internal IProvider Provider
		{
			get
			{
				CheckDispose();
				return this._provider;
			}
		}

		/// <summary>
		/// Returns true if the database specified by the connection object exists.
		/// </summary>
		/// <returns></returns>
		public bool DatabaseExists()
		{
			CheckDispose();
			return this._provider.DatabaseExists();
		}

		/// <summary>
		/// Creates a new database instance (catalog or file) at the location specified by the connection
		/// using the metadata encoded within the entities or mapping file.
		/// </summary>
		public void CreateDatabase()
		{
			CheckDispose();
			this._provider.CreateDatabase();
		}

		/// <summary>
		/// Deletes the database instance at the location specified by the connection.
		/// </summary>
		public void DeleteDatabase()
		{
			CheckDispose();
			this._provider.DeleteDatabase();
		}

		/// <summary>
		/// Submits one or more commands to the database reflecting the changes made to the retreived entities.
		/// If a transaction is not already specified one will be created for the duration of this operation.
		/// If a change conflict is encountered a ChangeConflictException will be thrown.
		/// </summary>
		public void SubmitChanges()
		{
			CheckDispose();
			SubmitChanges(ConflictMode.FailOnFirstConflict);
		}

		/// <summary>
		/// Submits one or more commands to the database reflecting the changes made to the retreived entities.
		/// If a transaction is not already specified one will be created for the duration of this operation.
		/// If a change conflict is encountered a ChangeConflictException will be thrown.  
		/// You can override this method to implement common conflict resolution behaviors.
		/// </summary>
		/// <param name="failureMode">Determines how SubmitChanges handles conflicts.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[....]: In the middle of attempting to rollback a transaction, outer transaction is thrown.")]
		public virtual void SubmitChanges(ConflictMode failureMode)
		{
			CheckDispose();
			CheckNotInSubmitChanges();
			VerifyTrackingEnabled();
			this._conflicts.Clear();

			try
			{
				this._isInSubmitChanges = true;

				if(System.Transactions.Transaction.Current == null && this._provider.Transaction == null)
				{
					bool openedConnection = false;
					DbTransaction transaction = null;
					try
					{
						if(this._provider.Connection.State == ConnectionState.Open)
						{
							this._provider.ClearConnection();
						}
						if(this._provider.Connection.State == ConnectionState.Closed)
						{
							this._provider.Connection.Open();
							openedConnection = true;
						}
						transaction = this._provider.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
						this._provider.Transaction = transaction;
						new ChangeProcessor(this._services, this).SubmitChanges(failureMode);
						this.AcceptChanges();

						// to commit a transaction, there can be no open readers
						// on the connection.
						this._provider.ClearConnection();

						transaction.Commit();
					}
					catch
					{
						if(transaction != null)
						{
							transaction.Rollback();
						}
						throw;
					}
					finally
					{
						this._provider.Transaction = null;
						if(openedConnection)
						{
							this._provider.Connection.Close();
						}
					}
				}
				else
				{
					new ChangeProcessor(_services, this).SubmitChanges(failureMode);
					this.AcceptChanges();
				}
			}
			finally
			{
				this._isInSubmitChanges = false;
			}
		}

		/// <summary>
		/// Refresh the specified object using the mode specified.  If the refresh
		/// cannot be performed (for example if the object no longer exists in the
		/// database) an InvalidOperationException is thrown.
		/// </summary>
		/// <param name="mode">How the refresh should be performed.</param>
		/// <param name="entity">The object to refresh.  The object must be
		/// the result of a previous query.</param>
		public void Refresh(RefreshMode mode, object entity)
		{
			CheckDispose();
			CheckNotInSubmitChanges();
			VerifyTrackingEnabled();
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			Array items = Array.CreateInstance(entity.GetType(), 1);
			items.SetValue(entity, 0);
			this.Refresh(mode, items as IEnumerable);
		}

		/// <summary>
		/// Refresh a set of objects using the mode specified.  If the refresh
		/// cannot be performed (for example if the object no longer exists in the
		/// database) an InvalidOperationException is thrown.
		/// </summary>
		/// <param name="mode">How the refresh should be performed.</param>
		/// <param name="entities">The objects to refresh.</param>
		public void Refresh(RefreshMode mode, params object[] entities)
		{
			CheckDispose(); // code hygeine requirement

			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}

			Refresh(mode, (IEnumerable)entities);
		}

		/// <summary>
		/// Refresh a collection of objects using the mode specified.  If the refresh
		/// cannot be performed (for example if the object no longer exists in the
		/// database) an InvalidOperationException is thrown.
		/// </summary>
		/// <param name="mode">How the refresh should be performed.</param>
		/// <param name="entities">The collection of objects to refresh.</param>
		public void Refresh(RefreshMode mode, IEnumerable entities)
		{
			CheckDispose();
			CheckNotInSubmitChanges();
			VerifyTrackingEnabled();

			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}

			// if the collection is a query, we need to execute and buffer,
			// since below we will be issuing additional queries and can only
			// have a single reader open.
			var list = entities.Cast<object>().ToList();

			// create a fresh context to fetch new state from
			DataContext refreshContext = this.CreateRefreshContext();

			foreach(object o in list)
			{
				// verify that each object in the list is an entity
				MetaType inheritanceRoot = _services.Model.GetMetaType(o.GetType()).InheritanceRoot;
				GetTable(inheritanceRoot.Type);

				TrackedObject trackedObject = this._services.ChangeTracker.GetTrackedObject(o);
				if(trackedObject == null)
				{
					throw Error.UnrecognizedRefreshObject();
				}

				if(trackedObject.IsNew)
				{
					throw Error.RefreshOfNewObject();
				}

				// query to get the current database values
				object[] keyValues = CommonDataServices.GetKeyValues(trackedObject.Type, trackedObject.Original);
				object freshInstance = refreshContext.Services.GetObjectByKey(trackedObject.Type, keyValues);
				if(freshInstance == null)
				{
					throw Error.RefreshOfDeletedObject();
				}

				// refresh the tracked object using the new values and
				// the mode specified.
				trackedObject.Refresh(mode, freshInstance);
			}
		}

		internal DataContext CreateRefreshContext()
		{
			CheckDispose();
			return new DataContext(this);
		}

		private void AcceptChanges()
		{
			CheckDispose();
			VerifyTrackingEnabled();
			this._services.ChangeTracker.AcceptChanges();
		}

		/// <summary>
		/// Returns the query text in the database server's native query language
		/// that would need to be executed to perform the specified query.
		/// </summary>
		/// <param name="query">The query</param>
		/// <returns></returns>
		internal string GetQueryText(IQueryable query)
		{
			CheckDispose();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			return this._provider.GetQueryText(query.Expression);
		}

		/// <summary>
		/// Returns an IDbCommand object representing the query in the database server's
		/// native query language.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DbCommand GetCommand(IQueryable query)
		{
			CheckDispose();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			return this._provider.GetCommand(query.Expression);
		}

		/// <summary>
		/// Returns the command text in the database server's native query langauge
		/// that would need to be executed in order to persist the changes made to the
		/// objects back into the database.
		/// </summary>
		/// <returns></returns>
		internal string GetChangeText()
		{
			CheckDispose();
			VerifyTrackingEnabled();
			return new ChangeProcessor(_services, this).GetChangeText();
		}

		/// <summary>
		/// Computes the un-ordered set of objects that have changed
		/// </summary>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ChangeSet", Justification = "The capitalization was deliberately chosen.")]
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Non-trivial operations are not suitable for properties.")]
		public ChangeSet GetChangeSet()
		{
			CheckDispose();
			return new ChangeProcessor(this._services, this).GetChangeSet();
		}

		/// <summary>
		/// Execute a command against the database server that does not return a sequence of objects.
		/// The command is specified using the server's native query language, such as SQL.
		/// </summary>
		/// <param name="command">The command specified in the server's native query language.</param>
		/// <param name="parameters">The parameter values to use for the query.</param>
		/// <returns>A single integer return value</returns>
		public int ExecuteCommand(string command, params object[] parameters)
		{
			CheckDispose();
			if(command == null)
			{
				throw Error.ArgumentNull("command");
			}
			if(parameters == null)
			{
				throw Error.ArgumentNull("parameters");
			}
			return (int)this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), command, parameters).ReturnValue;
		}

		/// <summary>
		/// Execute the sequence returning query against the database server. 
		/// The query is specified using the server's native query language, such as SQL.
		/// </summary>
		/// <typeparam name="TResult">The element type of the result sequence.</typeparam>
		/// <param name="query">The query specified in the server's native query language.</param>
		/// <param name="parameters">The parameter values to use for the query.</param>
		/// <returns>An IEnumerable sequence of objects.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		public IEnumerable<TResult> ExecuteQuery<TResult>(string query, params object[] parameters)
		{
			CheckDispose();
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			if(parameters == null)
			{
				throw Error.ArgumentNull("parameters");
			}
			return (IEnumerable<TResult>)this.ExecuteMethodCall(this, ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), query, parameters).ReturnValue;
		}

		/// <summary>
		/// Execute the sequence returning query against the database server. 
		/// The query is specified using the server's native query language, such as SQL.
		/// </summary>
		/// <param name="elementType">The element type of the result sequence.</param>
		/// <param name="query">The query specified in the server's native query language.</param>
		/// <param name="parameters">The parameter values to use for the query.</param>
		/// <returns></returns>
		public IEnumerable ExecuteQuery(Type elementType, string query, params object[] parameters)
		{
			CheckDispose();
			if(elementType == null)
			{
				throw Error.ArgumentNull("elementType");
			}
			if(query == null)
			{
				throw Error.ArgumentNull("query");
			}
			if(parameters == null)
			{
				throw Error.ArgumentNull("parameters");
			}
			if(_miExecuteQuery == null)
			{
				_miExecuteQuery = typeof(DataContext).GetMethods().Single(m => m.Name == "ExecuteQuery" && m.GetParameters().Length == 2);
			}
			return (IEnumerable)this.ExecuteMethodCall(this, _miExecuteQuery.MakeGenericMethod(elementType), query, parameters).ReturnValue;
		}
		private static MethodInfo _miExecuteQuery;


		/// <summary>
		/// Executes the equivalent of the specified method call on the database server.
		/// </summary>
		/// <param name="instance">The instance the method is being called on.</param>
		/// <param name="methodInfo">The reflection MethodInfo for the method to invoke.</param>
		/// <param name="parameters">The parameters for the method call.</param>
		/// <returns>The result of the method call. Use this type's ReturnValue property to access the actual return value.</returns>
		internal protected IExecuteResult ExecuteMethodCall(object instance, MethodInfo methodInfo, params object[] parameters)
		{
			CheckDispose();
			if(instance == null)
			{
				throw Error.ArgumentNull("instance");
			}
			if(methodInfo == null)
			{
				throw Error.ArgumentNull("methodInfo");
			}
			if(parameters == null)
			{
				throw Error.ArgumentNull("parameters");
			}
			return this._provider.Execute(this.GetMethodCall(instance, methodInfo, parameters));
		}

		/// <summary>
		/// Create a query object for the specified method call.
		/// </summary>
		/// <typeparam name="TResult">The element type of the query.</typeparam>
		/// <param name="instance">The instance the method is being called on.</param>
		/// <param name="methodInfo">The reflection MethodInfo for the method to invoke.</param>
		/// <param name="parameters">The parameters for the method call.</param>
		/// <returns>The returned query object</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		internal protected IQueryable<TResult> CreateMethodCallQuery<TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
		{
			CheckDispose();
			if(instance == null)
			{
				throw Error.ArgumentNull("instance");
			}
			if(methodInfo == null)
			{
				throw Error.ArgumentNull("methodInfo");
			}
			if(parameters == null)
			{
				throw Error.ArgumentNull("parameters");
			}
			if(!typeof(IQueryable<TResult>).IsAssignableFrom(methodInfo.ReturnType))
			{
				throw Error.ExpectedQueryableArgument("methodInfo", typeof(IQueryable<TResult>));
			}
			return new DataQuery<TResult>(this, this.GetMethodCall(instance, methodInfo, parameters));
		}

		private Expression GetMethodCall(object instance, MethodInfo methodInfo, params object[] parameters)
		{
			CheckDispose();
			if(parameters.Length > 0)
			{
				ParameterInfo[] pis = methodInfo.GetParameters();
				List<Expression> args = new List<Expression>(parameters.Length);
				for(int i = 0, n = parameters.Length; i < n; i++)
				{
					Type pType = pis[i].ParameterType;
					if(pType.IsByRef)
					{
						pType = pType.GetElementType();
					}
					args.Add(Expression.Constant(parameters[i], pType));
				}
				return Expression.Call(Expression.Constant(instance), methodInfo, args);
			}
			return Expression.Call(Expression.Constant(instance), methodInfo);
		}

		/// <summary>
		/// Execute a dynamic insert
		/// </summary>
		/// <param name="entity"></param>
		internal protected void ExecuteDynamicInsert(object entity)
		{
			CheckDispose();
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			this.CheckInSubmitChanges();
			TrackedObject tracked = this._services.ChangeTracker.GetTrackedObject(entity);
			if(tracked == null)
			{
				throw Error.CannotPerformOperationForUntrackedObject();
			}
			this._services.ChangeDirector.DynamicInsert(tracked);
		}

		/// <summary>
		/// Execute a dynamic update
		/// </summary>
		/// <param name="entity"></param>
		internal protected void ExecuteDynamicUpdate(object entity)
		{
			CheckDispose();
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			this.CheckInSubmitChanges();
			TrackedObject tracked = this._services.ChangeTracker.GetTrackedObject(entity);
			if(tracked == null)
			{
				throw Error.CannotPerformOperationForUntrackedObject();
			}
			int result = this._services.ChangeDirector.DynamicUpdate(tracked);
			if(result == 0)
			{
				throw new ChangeConflictException();
			}
		}

		/// <summary>
		/// Execute a dynamic delete
		/// </summary>
		/// <param name="entity"></param>
		internal protected void ExecuteDynamicDelete(object entity)
		{
			CheckDispose();
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			this.CheckInSubmitChanges();
			TrackedObject tracked = this._services.ChangeTracker.GetTrackedObject(entity);
			if(tracked == null)
			{
				throw Error.CannotPerformOperationForUntrackedObject();
			}
			int result = this._services.ChangeDirector.DynamicDelete(tracked);
			if(result == 0)
			{
				throw new ChangeConflictException();
			}
		}

		/// <summary>
		/// Translates the data from a DbDataReader into sequence of objects.
		/// </summary>
		/// <typeparam name="TResult">The element type of the resulting sequence</typeparam>
		/// <param name="reader">The DbDataReader to translate</param>
		/// <returns>The translated sequence of objects</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		public IEnumerable<TResult> Translate<TResult>(DbDataReader reader)
		{
			CheckDispose();
			return (IEnumerable<TResult>)this.Translate(typeof(TResult), reader);
		}

		/// <summary>
		/// Translates the data from a DbDataReader into sequence of objects.
		/// </summary>
		/// <param name="elementType">The element type of the resulting sequence</param>
		/// <param name="reader">The DbDataReader to translate</param>
		/// <returns>The translated sequence of objects</returns>
		public IEnumerable Translate(Type elementType, DbDataReader reader)
		{
			CheckDispose();
			if(elementType == null)
			{
				throw Error.ArgumentNull("elementType");
			}
			if(reader == null)
			{
				throw Error.ArgumentNull("reader");
			}
			return this._provider.Translate(elementType, reader);
		}

		/// <summary>
		/// Translates the data from a DbDataReader into IMultipleResults.
		/// </summary>
		/// <param name="reader">The DbDataReader to translate</param>
		/// <returns>The translated sequence of objects</returns>
		public IMultipleResults Translate(DbDataReader reader)
		{
			CheckDispose();
			if(reader == null)
			{
				throw Error.ArgumentNull("reader");
			}
			return this._provider.Translate(reader);
		}

		/// <summary>
		/// Remove all Include\Subquery LoadOptions settings.
		/// </summary>
		internal void ResetLoadOptions()
		{
			CheckDispose();
			this._loadOptions = null;
		}


		#region Property Declarations
		/// <summary>
		/// The DataLoadOptions used to define prefetch behavior for defer loaded members
		/// and membership of related collections.
		/// </summary>
		public DataLoadOptions LoadOptions
		{
			get
			{
				CheckDispose();
				return this._loadOptions;
			}
			set
			{
				CheckDispose();
				if(this._services.HasCachedObjects && value != this._loadOptions)
				{
					throw Error.LoadOptionsChangeNotAllowedAfterQuery();
				}
				if(value != null)
				{
					value.Freeze();
				}
				this._loadOptions = value;
			}
		}

		/// <summary>
		/// This list of change conflicts produced by the last call to SubmitChanges.  Use this collection
		/// to resolve conflicts after catching a ChangeConflictException and before calling SubmitChanges again.
		/// </summary>
		public ChangeConflictCollection ChangeConflicts
		{
			get
			{
				CheckDispose();
				return this._conflicts;
			}
		}


		/// <summary>
		/// Gets or sets the per instance provider mode. The per-instance provider mode is initialized with the static DataContext.ProviderMode value at class instantiation and
		/// can be overriden per instance with this property. The enum has to match the provider used, otherwise the default of the provider will be used.
		/// </summary>
		public Enum PerInstanceProviderMode
		{
			get {  return _perInstanceProviderMode; }
			set
			{
				if(value == null)
				{
					throw Error.ArgumentNull("value");
				}
				_perInstanceProviderMode = value;
				SetProviderMode();
			}
		}
		#endregion
	}
}
