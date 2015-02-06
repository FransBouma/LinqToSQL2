using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Visitor which checks whether there's a side effect to be expected when the query is compiled. 
	/// </summary>
	internal class SideEffectChecker : SqlVisitor
	{
		bool hasSideEffect;

		internal bool HasSideEffect(SqlNode node)
		{
			this.hasSideEffect = false;
			this.Visit(node);
			return this.hasSideEffect;
		}

		internal override SqlExpression VisitJoinedCollection(SqlJoinedCollection jc)
		{
			this.hasSideEffect = true;
			return jc;
		}

		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			return cq;
		}
	}
}