using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// converts correlated scalar subqueries into outer-applies
	/// </summary>
	/// <remarks>must be run after flattener.</remarks>
	internal class ScalarSubQueryRewriter : SqlVisitor
	{
		NodeFactory sql;
		SqlSelect currentSelect;
		SqlAggregateChecker aggregateChecker;

		internal ScalarSubQueryRewriter(NodeFactory sqlFactory)
		{
			this.sql = sqlFactory;
			this.aggregateChecker = new SqlAggregateChecker();
		}

		internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss)
		{
			SqlSelect innerSelect = this.VisitSelect(ss.Select);
			if(!this.aggregateChecker.HasAggregates(innerSelect))
			{
				innerSelect.Top = this.sql.ValueFromObject(1, ss.SourceExpression);
			}
			innerSelect.OrderingType = SqlOrderingType.Blocked;
			SqlAlias alias = new SqlAlias(innerSelect);
			this.currentSelect.From = new SqlJoin(SqlJoinType.OuterApply, this.currentSelect.From, alias, null, ss.SourceExpression);
			return new SqlColumnRef(innerSelect.Row.Columns[0]);
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			SqlSelect save = this.currentSelect;
			try
			{
				this.currentSelect = @select;
				return base.VisitSelect(@select);
			}
			finally
			{
				this.currentSelect = save;
			}
		}
	}
}