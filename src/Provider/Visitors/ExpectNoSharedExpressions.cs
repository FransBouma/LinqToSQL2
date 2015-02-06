using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ExpectNoSharedExpressions : SqlVisitor
	{
		internal override SqlExpression VisitSharedExpression(SqlSharedExpression shared)
		{
			throw Error.UnexpectedSharedExpression();
		}
		internal override SqlExpression VisitSharedExpressionRef(SqlSharedExpressionRef sref)
		{
			throw Error.UnexpectedSharedExpressionReference();
		}
	}
}