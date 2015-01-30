using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal abstract class SqlSource : SqlNode {
		internal SqlSource(SqlNodeType nt, Expression sourceExpression)
			: base(nt, sourceExpression) {
			}
	}
}