using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Skips over client portion of selection expression
	/// </summary>
	internal class SqlSelectionSkipper : SqlVisitor
	{
		SqlVisitor parent;
		internal SqlSelectionSkipper(SqlVisitor parent)
		{
			this.parent = parent;
		}
		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			// pass control back to parent
			return parent.VisitColumn(col);
		}
		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			// pass control back to parent
			return this.parent.VisitSubSelect(ss);
		}
		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			// pass control back to parent
			return this.parent.VisitClientQuery(cq);
		}
	}
}