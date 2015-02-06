using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ColumnNameGatherer : SqlVisitor
	{
		public ColumnNameGatherer()
			: base()
		{
			this.Names = new HashSet<string>();
		}

		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			if(!String.IsNullOrEmpty(col.Name))
			{
				this.Names.Add(col.Name);
			}

			return base.VisitColumn(col);
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			Visit(cref.Column);

			return base.VisitColumnRef(cref);
		}

		public HashSet<string> Names { get; set; }
	}
}