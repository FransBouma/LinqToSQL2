using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// if the top level select is simply a reprojection of the subquery, then remove it,
	/// pushing any distinct names down
	/// </summary>
	internal class SqlTopSelectDeflator : SqlVisitor
	{
		#region Private Classes
		private class ColumnMapper : SqlVisitor
		{
			Dictionary<SqlColumn, SqlColumnRef> map;
			internal ColumnMapper(Dictionary<SqlColumn, SqlColumnRef> map)
			{
				this.map = map;
			}
			internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
			{
				SqlColumnRef mapped;
				return this.map.TryGetValue(cref.Column, out mapped) ? mapped : cref;
			}
		}
		#endregion


		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			if(IsTrivialSelect(@select))
			{
				SqlSelect aselect = (SqlSelect)((SqlAlias)@select.From).Node;
				// build up a column map, so we can rewrite the top-level selection expression
				Dictionary<SqlColumn, SqlColumnRef> map = new Dictionary<SqlColumn, SqlColumnRef>();
				foreach(SqlColumn c in @select.Row.Columns)
				{
					SqlColumnRef cref = (SqlColumnRef)c.Expression;
					map.Add(c, cref);
					// push the interesting column names down (non null)
					if(!String.IsNullOrEmpty(c.Name))
					{
						cref.Column.Name = c.Name;
					}
				}
				aselect.Selection = new ColumnMapper(map).VisitExpression(@select.Selection);
				return aselect;
			}
			return @select;
		}

		private bool IsTrivialSelect(SqlSelect select)
		{
			if(@select.OrderBy.Count != 0 ||
			   @select.GroupBy.Count != 0 ||
			   @select.Having != null ||
			   @select.Top != null ||
			   @select.IsDistinct ||
			   @select.Where != null)
				return false;
			return this.HasTrivialSource(@select.From) && this.HasTrivialProjection(@select);
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private bool HasTrivialSource(SqlSource node)
		{
			SqlAlias alias = node as SqlAlias;
			if(alias == null) return false;
			return alias.Node is SqlSelect;
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private bool HasTrivialProjection(SqlSelect select)
		{
			return select.Row.Columns.All(c=>c.Expression == null || c.Expression.NodeType == SqlNodeType.ColumnRef);
		}
	}
}