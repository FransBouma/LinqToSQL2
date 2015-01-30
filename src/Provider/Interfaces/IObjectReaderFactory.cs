using System.Data.Common;

namespace System.Data.Linq.Provider.Interfaces
{
	internal interface IObjectReaderFactory {
		IObjectReader Create(DbDataReader reader, bool disposeReader, IReaderProvider provider, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries);
		IObjectReader GetNextResult(IObjectReaderSession session, bool disposeReader);
	}
}