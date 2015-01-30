using System.Data.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Interfaces
{
	internal interface IObjectReaderCompiler {
		IObjectReaderFactory Compile(SqlExpression expression, Type elementType);
		IObjectReaderSession CreateSession(DbDataReader reader, IReaderProvider provider, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries);
	}
}