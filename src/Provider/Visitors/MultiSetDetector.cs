using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class MultiSetDetector : SqlVisitor
	{
		internal bool FoundMultiset { get; set; }


		internal override SqlExpression VisitMultiset(SqlSubSelect sms)
		{
			this.FoundMultiset = true;
			return sms;
		}

		internal override SqlExpression VisitElement(SqlSubSelect elem)
		{
			return elem;
		}

		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
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