using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Removes duplicate columns from order by and group by lists
	/// </summary>
	internal class SqlDuplicateColumnDeflator : SqlVisitor
	{
		SqlColumnEqualizer equalizer = new SqlColumnEqualizer();

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			select.From = this.VisitSource(select.From);
			select.Where = this.VisitExpression(select.Where);
			for(int i = 0, n = select.GroupBy.Count; i < n; i++)
			{
				select.GroupBy[i] = this.VisitExpression(select.GroupBy[i]);
			}
			// remove duplicate group expressions
			for(int i = select.GroupBy.Count - 1; i >= 0; i--)
			{
				for(int j = i - 1; j >= 0; j--)
				{
					if(SqlComparer.AreEqual(select.GroupBy[i], select.GroupBy[j]))
					{
						select.GroupBy.RemoveAt(i);
						break;
					}
				}
			}
			select.Having = this.VisitExpression(select.Having);
			for(int i = 0, n = select.OrderBy.Count; i < n; i++)
			{
				select.OrderBy[i].Expression = this.VisitExpression(select.OrderBy[i].Expression);
			}
			// remove duplicate order expressions
			if(select.OrderBy.Count > 0)
			{
				this.equalizer.BuildEqivalenceMap(select.From);

				for(int i = select.OrderBy.Count - 1; i >= 0; i--)
				{
					for(int j = i - 1; j >= 0; j--)
					{
						if(this.equalizer.AreEquivalent(select.OrderBy[i].Expression, select.OrderBy[j].Expression))
						{
							select.OrderBy.RemoveAt(i);
							break;
						}
					}
				}
			}
			select.Top = this.VisitExpression(select.Top);
			select.Row = (SqlRow)this.Visit(select.Row);
			select.Selection = this.VisitExpression(select.Selection);
			return select;
		}
	}
}