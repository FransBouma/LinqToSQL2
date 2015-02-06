using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.Common
{
	internal abstract class ObjectReaderBase<TDataReader> : ObjectMaterializer<TDataReader>
		where TDataReader : DbDataReader
	{
		bool hasRead;
		bool hasCurrentRow;
		bool isFinished;
		IDataServices services;

		internal ObjectReaderBase(ObjectReaderSession<TDataReader> session,
								  NamedColumn[] namedColumns,
								  object[] globals,
								  object[] arguments,
								  int nLocals
			)
			: base()
		{
			this.Session = session;
			this.services = session.Provider.Services;
			this.DataReader = session.DataReader;
			this.Globals = globals;
			this.Arguments = arguments;
			if(nLocals > 0)
			{
				this.Locals = new object[nLocals];
			}
			if(this.Session.IsBuffered)
			{
				this.Buffer();
			}
			this.Ordinals = this.GetColumnOrdinals(namedColumns);
		}

		// This method is called from within this class's constructor (through a call to Buffer()) so it is sealed to prevent
		// derived classes from overriding it. See FxCop rule CA2214 for more information on why this is necessary.
		public override sealed bool Read()
		{
			if(this.isFinished)
			{
				return false;
			}
			if(this.BufferReader != null)
			{
				this.hasCurrentRow = this.BufferReader.Read();
			}
			else
			{
				this.hasCurrentRow = this.DataReader.Read();
			}
			if(!this.hasCurrentRow)
			{
				this.isFinished = true;
				this.Session.Finish(this);
			}
			this.hasRead = true;
			return this.hasCurrentRow;
		}

		internal bool IsBuffered
		{
			get { return this.BufferReader != null; }
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes", Justification = "[....]: Used only as a buffer and never used for string comparison.")]
		internal void Buffer()
		{
			if(this.BufferReader == null && (this.hasCurrentRow || !this.hasRead))
			{
				if(this.Session.IsBuffered)
				{
					this.BufferReader = this.Session.GetNextBufferedReader();
				}
				else
				{
					DataSet ds = new DataSet();
					ds.EnforceConstraints = false;
					DataTable bufferTable = new DataTable();
					ds.Tables.Add(bufferTable);
					string[] names = this.Session.GetActiveNames();
					bufferTable.Load(new Rereader(this.DataReader, this.hasCurrentRow, null), LoadOption.OverwriteChanges);
					this.BufferReader = new Rereader(bufferTable.CreateDataReader(), false, names);
				}
				if(this.hasCurrentRow)
				{
					this.Read();
				}
			}
		}

		public override object InsertLookup(int iMetaType, object instance)
		{
			MetaType mType = (MetaType)this.Globals[iMetaType];
			return this.services.InsertLookupCachedObject(mType, instance);
		}

		public override void SendEntityMaterialized(int iMetaType, object instance)
		{
			MetaType mType = (MetaType)this.Globals[iMetaType];
			this.services.OnEntityMaterialized(mType, instance);
		}

		public override IEnumerable ExecuteSubQuery(int iSubQuery, object[] parentArgs)
		{
			if(this.Session.ParentArguments != null)
			{
				// Create array to accumulate args, and add both parent
				// args and the supplied args to the array
				int nParent = this.Session.ParentArguments.Length;
				object[] tmp = new object[nParent + parentArgs.Length];
				Array.Copy(this.Session.ParentArguments, tmp, nParent);
				Array.Copy(parentArgs, 0, tmp, nParent, parentArgs.Length);
				parentArgs = tmp;
			}
			ICompiledSubQuery subQuery = this.Session.SubQueries[iSubQuery];
			IEnumerable results = (IEnumerable)subQuery.Execute(this.Session.Provider, parentArgs, this.Session.UserArguments).ReturnValue;
			return results;
		}

		public override bool CanDeferLoad
		{
			get { return this.services.Context.DeferredLoadingEnabled; }
		}

		public override IEnumerable<T> GetLinkSource<T>(int iGlobalLink, int iLocalFactory, object[] keyValues)
		{
			IDeferredSourceFactory factory = (IDeferredSourceFactory)this.Locals[iLocalFactory];
			if(factory == null)
			{
				MetaDataMember member = (MetaDataMember)this.Globals[iGlobalLink];
				factory = this.services.GetDeferredSourceFactory(member);
				this.Locals[iLocalFactory] = factory;
			}
			return (IEnumerable<T>)factory.CreateDeferredSource(keyValues);
		}

		public override IEnumerable<T> GetNestedLinkSource<T>(int iGlobalLink, int iLocalFactory, object instance)
		{
			IDeferredSourceFactory factory = (IDeferredSourceFactory)this.Locals[iLocalFactory];
			if(factory == null)
			{
				MetaDataMember member = (MetaDataMember)this.Globals[iGlobalLink];
				factory = this.services.GetDeferredSourceFactory(member);
				this.Locals[iLocalFactory] = factory;
			}
			return (IEnumerable<T>)factory.CreateDeferredSource(instance);
		}

		private int[] GetColumnOrdinals(NamedColumn[] namedColumns)
		{
			DbDataReader reader = null;
			if(this.BufferReader != null)
			{
				reader = this.BufferReader;
			}
			else
			{
				reader = this.DataReader;
			}
			if(namedColumns == null || namedColumns.Length == 0)
			{
				return null;
			}
			int[] columnOrdinals = new int[namedColumns.Length];
			Dictionary<string, int> lookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			//we need to compare the quoted names on both sides
			//because the designer might quote the name unnecessarily
			for(int i = 0, n = reader.FieldCount; i < n; i++)
			{
				lookup[SqlIdentifier.QuoteCompoundIdentifier(reader.GetName(i))] = i;
			}
			for(int i = 0, n = namedColumns.Length; i < n; i++)
			{
				int ordinal;
				if(lookup.TryGetValue(SqlIdentifier.QuoteCompoundIdentifier(namedColumns[i].Name), out ordinal))
				{
					columnOrdinals[i] = ordinal;
				}
				else if(namedColumns[i].IsRequired)
				{
					throw Error.RequiredColumnDoesNotExist(namedColumns[i].Name);
				}
				else
				{
					columnOrdinals[i] = -1;
				}
			}
			return columnOrdinals;
		}

		#region Property Declarations
		protected ObjectReaderSession<TDataReader> Session { get; set; }
		#endregion
	}
}