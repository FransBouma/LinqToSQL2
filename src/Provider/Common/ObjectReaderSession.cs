using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Provider.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.Common
{
	internal class ObjectReaderSession<TDataReader> : IObjectReaderSession, IDisposable, IConnectionUser
		where TDataReader : DbDataReader
	{
		#region Member Declarations
		private TDataReader _dataReader;
		private ObjectReaderBase<TDataReader> _currentReader;
		private IReaderProvider _provider;
		private List<DbDataReader> _buffer;
		private int _indexNextBufferedReader;
		private bool _isDisposed;
		private bool _isDataReaderDisposed;
		private bool _hasResults;
		private object[] _parentArgs;
		private object[] _userArgs;
		private ICompiledSubQuery[] _subQueries;
		#endregion

		internal ObjectReaderSession(TDataReader dataReader, IReaderProvider provider, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries)
		{
			_dataReader = dataReader;
			_provider = provider;
			_parentArgs = parentArgs;
			_userArgs = userArgs;
			_subQueries = subQueries;
			_hasResults = true;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes", Justification = "[....]: Used only as a buffer and never used for string comparison.")]
		public void Buffer()
		{
			if(_buffer == null)
			{
				if(_currentReader != null && !_currentReader.IsBuffered)
				{
					_currentReader.Buffer();
					this.CheckNextResults();
				}
				// buffer anything remaining in the session
				_buffer = new List<DbDataReader>();
				while(_hasResults)
				{
					DataSet ds = new DataSet();
					ds.EnforceConstraints = false;
					DataTable tb = new DataTable();
					ds.Tables.Add(tb);
					string[] names = this.GetActiveNames();
					tb.Load(new Rereader(_dataReader, false, null), LoadOption.OverwriteChanges);
					_buffer.Add(new Rereader(tb.CreateDataReader(), false, names));
					this.CheckNextResults();
				}
			}
		}

		internal string[] GetActiveNames()
		{
			string[] names = new string[this.DataReader.FieldCount];
			for(int i = 0, n = this.DataReader.FieldCount; i < n; i++)
			{
				names[i] = this.DataReader.GetName(i);
			}
			return names;
		}

		public void CompleteUse()
		{
			this.Buffer();
		}

		public void Dispose()
		{
			if(!_isDisposed)
			{
				// Technically, calling GC.SuppressFinalize is not required because the class does not
				// have a finalizer, but it does no harm, protects against the case where a finalizer is added
				// in the future, and prevents an FxCop warning.
				GC.SuppressFinalize(this);
				_isDisposed = true;
				if(!_isDataReaderDisposed)
				{
					_isDataReaderDisposed = true;
					_dataReader.Dispose();
				}
				_provider.ConnectionManager.ReleaseConnection(this);
			}
		}

		internal ObjectReader<TDataReader, TObject> CreateReader<TObject>(Func<ObjectMaterializer<TDataReader>, TObject> fnMaterialize, NamedColumn[] namedColumns, object[] globals,
																		  int nLocals, bool disposeDataReader)
		{
			ObjectReader<TDataReader, TObject> objectReader = new ObjectReader<TDataReader, TObject>(this, namedColumns, globals, _userArgs, nLocals, disposeDataReader, fnMaterialize);
			_currentReader = objectReader;
			return objectReader;
		}

		internal ObjectReader<TDataReader, TObject> GetNextResult<TObject>(Func<ObjectMaterializer<TDataReader>, TObject> fnMaterialize, NamedColumn[] namedColumns, object[] globals,
																		   int nLocals, bool disposeDataReader)
		{
			// skip forward to next results
			if(_buffer != null)
			{
				if(_indexNextBufferedReader >= _buffer.Count)
				{
					return null;
				}
			}
			else
			{
				if(_currentReader != null)
				{
					// buffer current reader
					_currentReader.Buffer();
					this.CheckNextResults();
				}
				if(!_hasResults)
				{
					return null;
				}
			}

			ObjectReader<TDataReader, TObject> objectReader = new ObjectReader<TDataReader, TObject>(this, namedColumns, globals, _userArgs, nLocals, disposeDataReader, fnMaterialize);

			_currentReader = objectReader;
			return objectReader;
		}

		internal void Finish(ObjectReaderBase<TDataReader> finishedReader)
		{
			if(_currentReader == finishedReader)
			{
				this.CheckNextResults();
			}
		}

		internal DbDataReader GetNextBufferedReader()
		{
			if(_indexNextBufferedReader < _buffer.Count)
			{
				return _buffer[_indexNextBufferedReader++];
			}
			Diagnostics.Debug.Assert(false);
			return null;
		}

		private void CheckNextResults()
		{
			_hasResults = !_dataReader.IsClosed && _dataReader.NextResult();
			_currentReader = null;
			if(!_hasResults)
			{
				this.Dispose();
			}
		}

		#region Property Declarations
		internal ObjectReaderBase<TDataReader> CurrentReader
		{
			get { return _currentReader; }
		}

		internal TDataReader DataReader
		{
			get { return _dataReader; }
		}

		internal IReaderProvider Provider
		{
			get { return _provider; }
		}

		internal object[] ParentArguments
		{
			get { return _parentArgs; }
		}

		internal object[] UserArguments
		{
			get { return _userArgs; }
		}

		internal ICompiledSubQuery[] SubQueries
		{
			get { return _subQueries; }
		}


		public bool IsBuffered
		{
			get { return _buffer != null; }
		}
		#endregion
	}
}