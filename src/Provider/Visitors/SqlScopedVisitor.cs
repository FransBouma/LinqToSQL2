using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SqlScopedVisitor : SqlVisitor
	{
		internal SqlScopedVisitor()
		{
			this.CurrentScope = new SqlScope(null, null);
		}

		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			SqlScope save = this.CurrentScope;
			this.CurrentScope = new SqlScope(null, this.CurrentScope);
			base.VisitSubSelect(ss);
			this.CurrentScope = save;
			return ss;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			select.From = (SqlSource)this.Visit(select.From);

			SqlScope save = this.CurrentScope;
			this.CurrentScope = new SqlScope(select.From, this.CurrentScope.ContainingScope);

			select.Where = this.VisitExpression(select.Where);
			for(int i = 0, n = select.GroupBy.Count; i < n; i++)
			{
				select.GroupBy[i] = this.VisitExpression(select.GroupBy[i]);
			}
			select.Having = this.VisitExpression(select.Having);
			for(int i = 0, n = select.OrderBy.Count; i < n; i++)
			{
				select.OrderBy[i].Expression = this.VisitExpression(select.OrderBy[i].Expression);
			}
			select.Top = this.VisitExpression(select.Top);
			select.Row = (SqlRow)this.Visit(select.Row);

			// selection must be able to see its own projection
			this.CurrentScope = new SqlScope(select, this.CurrentScope.ContainingScope);
			select.Selection = this.VisitExpression(select.Selection);

			this.CurrentScope = save;
			return select;
		}

		internal override SqlStatement VisitInsert(SqlInsert sin)
		{
			SqlScope save = this.CurrentScope;
			this.CurrentScope = new SqlScope(sin, this.CurrentScope.ContainingScope);
			base.VisitInsert(sin);
			this.CurrentScope = save;
			return sin;
		}

		internal override SqlStatement VisitUpdate(SqlUpdate sup)
		{
			SqlScope save = this.CurrentScope;
			this.CurrentScope = new SqlScope(sup.Select, this.CurrentScope.ContainingScope);
			base.VisitUpdate(sup);
			this.CurrentScope = save;
			return sup;
		}

		internal override SqlStatement VisitDelete(SqlDelete sd)
		{
			SqlScope save = this.CurrentScope;
			this.CurrentScope = new SqlScope(sd, this.CurrentScope.ContainingScope);
			base.VisitDelete(sd);
			this.CurrentScope = save;
			return sd;
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			SqlScope save = this.CurrentScope;
			switch(join.JoinType)
			{
				case SqlJoinType.CrossApply:
				case SqlJoinType.OuterApply:
				{
					this.Visit(join.Left);
					SqlScope tmp = new SqlScope(join.Left, this.CurrentScope.ContainingScope);
					this.CurrentScope = new SqlScope(null, tmp);
					this.Visit(join.Right);
					SqlScope tmp2 = new SqlScope(join.Right, tmp);
					this.CurrentScope = new SqlScope(null, tmp2);
					this.Visit(join.Condition);
					break;
				}
				default:
				{
					this.Visit(join.Left);
					this.Visit(join.Right);
					this.CurrentScope = new SqlScope(null, new SqlScope(join.Right, new SqlScope(join.Left, this.CurrentScope.ContainingScope)));
					this.Visit(join.Condition);
					break;
				}
			}
			this.CurrentScope = save;
			return join;
		}


		internal SqlScope CurrentScope { get; set; }
	}
}