using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Class which determines whether there's a query hierarchy in the visited node tree
	/// </summary>
	internal class QueryHierarchyFinder : SqlVisitor
	{
		internal bool FoundHierarchy { get; set; }

		internal override SqlExpression VisitMultiset(SqlSubSelect sms)
		{
			this.FoundHierarchy = true;
			return sms;
		}

		internal override SqlExpression VisitElement(SqlSubSelect elem)
		{
			this.FoundHierarchy = true;
			return elem;
		}

		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			this.FoundHierarchy = true;
			return cq;
		}

		internal override SqlExpression VisitExists(SqlSubSelect ss)
		{
			return ss;
		}

		internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss)
		{
			return ss;
		}
	}
}