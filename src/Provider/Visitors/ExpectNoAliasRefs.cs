using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ExpectNoAliasRefs : SqlVisitor
	{
		internal override SqlExpression VisitAliasRef(SqlAliasRef aref)
		{
			throw Error.UnexpectedNode(aref.NodeType);
		}
	}
}