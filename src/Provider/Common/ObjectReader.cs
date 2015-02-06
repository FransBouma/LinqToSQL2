using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Provider.Interfaces;

namespace System.Data.Linq.Provider.Common
{
	internal class ObjectReader<TDataReader, TObject> : ObjectReaderBase<TDataReader>, IEnumerator<TObject>, IObjectReader, IDisposable
		where TDataReader : DbDataReader
	{
		#region Member Declarations
		private Func<ObjectMaterializer<TDataReader>, TObject> _materializeFunc;
		private TObject _current;
		private bool _disposeSession;
		#endregion

		internal ObjectReader(ObjectReaderSession<TDataReader> session, NamedColumn[] namedColumns, object[] globals, object[] arguments, int numberOfLocals, bool disposeSession,
							  Func<ObjectMaterializer<TDataReader>, TObject> materializeFunc)
			: base(session, namedColumns, globals, arguments, numberOfLocals)
		{
			_disposeSession = disposeSession;
			_materializeFunc = materializeFunc;
		}

		public void Dispose()
		{
			// Technically, calling GC.SuppressFinalize is not required because the class does not
			// have a finalizer, but it does no harm, protects against the case where a finalizer is added
			// in the future, and prevents an FxCop warning.
			GC.SuppressFinalize(this);
			if(_disposeSession)
			{
				base.Session.Dispose();
			}
		}

		public void Reset()
		{
		}

		public bool MoveNext()
		{
			if(this.Read())
			{
				_current = _materializeFunc(this);
				return true;
			}
			_current = default(TObject);
			this.Dispose();
			return false;
		}

		#region Explicit IEnumerator implementation
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}
		#endregion

		#region Property Declarations
		public IObjectReaderSession Session
		{
			get { return base.Session; }
		}

		public TObject Current
		{
			get { return _current; }
		}
		#endregion
	}
}