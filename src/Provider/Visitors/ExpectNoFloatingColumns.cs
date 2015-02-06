using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ExpectNoFloatingColumns : SqlVisitor
	{
		internal override SqlRow VisitRow(SqlRow row)
		{
			foreach(SqlColumn c in row.Columns)
			{
				this.Visit(c.Expression);
			}
			return row;
		}
		internal override SqlTable VisitTable(SqlTable tab)
		{
			foreach(SqlColumn c in tab.Columns)
			{
				this.Visit(c.Expression);
			}
			return tab;
		}
		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			throw Error.UnexpectedFloatingColumn();
		}
	}
}