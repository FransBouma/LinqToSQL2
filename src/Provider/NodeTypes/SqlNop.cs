using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlNop : SqlSimpleTypeExpression {
		internal SqlNop(Type clrType, ProviderType sqlType, Expression sourceExpression)
			: base(SqlNodeType.Nop, clrType, sqlType, sourceExpression) {
			}
	}
}