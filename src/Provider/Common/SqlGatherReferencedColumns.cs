using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal static class SqlGatherReferencedColumns
	{
		#region Private Classes
		private class Visitor : SqlVisitor
		{
			HashSet<SqlColumn> columns;
			internal Visitor(HashSet<SqlColumn> columns)
			{
				this.columns = columns;
			}
			internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
			{
				if(!this.columns.Contains(cref.Column))
				{
					this.columns.Add(cref.Column);
					if(cref.Column.Expression != null)
					{
						this.Visit(cref.Column.Expression);
					}
				}
				return cref;
			}
		}
		#endregion

		internal static HashSet<SqlColumn> Gather(SqlNode node, HashSet<SqlColumn> columns)
		{
			Visitor v = new Visitor(columns);
			v.Visit(node);
			return columns;
		}
	}
}