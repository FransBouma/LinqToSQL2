using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ProducedColumnsGatherer : SqlVisitor
	{
		private List<SqlColumn> columns;

		internal ProducedColumnsGatherer(List<SqlColumn> columns)
		{
			this.columns = columns;
		}
		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			foreach(SqlColumn c in @select.Row.Columns)
			{
				this.columns.Add(c);
			}
			return @select;
		}
		internal override SqlNode VisitUnion(SqlUnion su)
		{
			return su;
		}
	}
}