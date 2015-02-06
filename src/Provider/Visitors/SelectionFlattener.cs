using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SelectionFlattener : SqlVisitor
	{
		#region Member Declarations
		private SqlRow row;
		private Dictionary<SqlColumn, SqlColumn> map;
		private bool isInput;
		private bool isNew;
		#endregion

		internal SelectionFlattener(SqlRow row, Dictionary<SqlColumn, SqlColumn> map, bool isInput)
		{
			this.row = row;
			this.map = map;
			this.isInput = isInput;
		}

		internal override SqlExpression VisitNew(SqlNew sox)
		{
			this.isNew = true;
			return base.VisitNew(sox);
		}

		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			SqlColumn c = this.FindColumn(this.row.Columns, col);
			if(c == null && col.Expression != null && !this.isInput && (!this.isNew || (this.isNew && !col.Expression.IsConstantColumn)))
			{
				c = this.FindColumnWithExpression(this.row.Columns, col.Expression);
			}
			if(c == null)
			{
				this.row.Columns.Add(col);
				c = col;
			}
			else if(c != col)
			{
				// preserve expr-sets when folding expressions together
				if(col.Expression.NodeType == SqlNodeType.ExprSet && c.Expression.NodeType != SqlNodeType.ExprSet)
				{
					c.Expression = col.Expression;
				}
				this.map[col] = c;
			}
			return new SqlColumnRef(c);
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			SqlColumn c = this.FindColumn(this.row.Columns, cref.Column);
			if(c == null)
			{
				return MakeFlattenedColumn(cref, null);
			}
			else
			{
				return new SqlColumnRef(c);
			}
		}

		// ignore subquery in selection
		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			return ss;
		}

		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			return cq;
		}

		private SqlColumnRef MakeFlattenedColumn(SqlExpression expr, string name)
		{
			SqlColumn c = (!this.isInput) ? this.FindColumnWithExpression(this.row.Columns, expr) : null;
			if(c == null)
			{
				c = new SqlColumn(expr.ClrType, expr.SqlType, name, null, expr, expr.SourceExpression);
				this.row.Columns.Add(c);
			}
			return new SqlColumnRef(c);
		}


		private SqlColumn FindColumn(IEnumerable<SqlColumn> columns, SqlColumn col)
		{
			foreach(SqlColumn c in columns)
			{
				if(this.RefersToColumn(c, col))
				{
					return c;
				}
			}
			return null;
		}

		private SqlColumn FindColumnWithExpression(IEnumerable<SqlColumn> columns, SqlExpression expr)
		{
			foreach(SqlColumn c in columns)
			{
				if(c == expr)
				{
					return c;
				}
				if(SqlComparer.AreEqual(c.Expression, expr))
				{
					return c;
				}
			}
			return null;
		}
	}
}