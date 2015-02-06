using System.Data.Common;
using System.Data.Linq.Provider.Interfaces;

namespace System.Data.Linq.Provider.Common
{
	internal class ObjectReaderFactory<TDataReader, TObject> : IObjectReaderFactory
		where TDataReader : DbDataReader
	{
		#region Member Declarations
		private Func<ObjectMaterializer<TDataReader>, TObject> _materializeFunc;
		private NamedColumn[] _namedColumns;
		private object[] _globals;
		private int _numberOfLocals;
		#endregion

		internal ObjectReaderFactory(Func<ObjectMaterializer<TDataReader>, TObject> materializeFunc, NamedColumn[] namedColumns, object[] globals, int numberOfLocals)
		{
			_materializeFunc = materializeFunc;
			_namedColumns = namedColumns;
			_globals = globals;
			_numberOfLocals = numberOfLocals;
		}

		public IObjectReader Create(DbDataReader dataReader, bool disposeDataReader, IReaderProvider provider, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries)
		{
			ObjectReaderSession<TDataReader> session = new ObjectReaderSession<TDataReader>((TDataReader)dataReader, provider, parentArgs, userArgs, subQueries);
			return session.CreateReader(_materializeFunc, _namedColumns, _globals, _numberOfLocals, disposeDataReader);
		}

		public IObjectReader GetNextResult(IObjectReaderSession session, bool disposeDataReader)
		{
			ObjectReaderSession<TDataReader> ors = (ObjectReaderSession<TDataReader>)session;
			IObjectReader reader = ors.GetNextResult(_materializeFunc, _namedColumns, _globals, _numberOfLocals, disposeDataReader);
			if(reader == null && disposeDataReader)
			{
				ors.Dispose();
			}
			return reader;
		}
	}
}