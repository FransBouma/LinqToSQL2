using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class WhereClauseLifter : SqlVisitor
	{
		#region Member Declarations
		private Scope current;
		private NodeFactory sql;
		private SqlAggregateChecker aggregateChecker;
		private SqlRowNumberChecker rowNumberChecker;
		#endregion

		#region Private Classes
		private class Scope
		{
			internal Scope Parent;
			internal SqlExpression Where;
			internal Scope(SqlExpression where, Scope parent)
			{
				this.Where = @where;
				this.Parent = parent;
			}
		};
		#endregion

		internal WhereClauseLifter(NodeFactory sql)
		{
			this.sql = sql;
			this.aggregateChecker = new SqlAggregateChecker();
			this.rowNumberChecker = new SqlRowNumberChecker();
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			Scope save = this.current;
			this.current = new Scope(select.Where, this.current);

			SqlSelect result = base.VisitSelect(select);

			bool stopHoisting =
				select.IsDistinct ||
				select.GroupBy.Count > 0 ||
				this.aggregateChecker.HasAggregates(select) ||
				select.Top != null ||
				this.rowNumberChecker.HasRowNumber(select);

			// Shift as much of the current WHERE to the parent as possible.
			if(this.current != null)
			{
				if(this.current.Parent != null && !stopHoisting)
				{
					this.current.Parent.Where = sql.AndAccumulate(this.current.Parent.Where, this.current.Where);
					this.current.Where = null;
				}
				select.Where = this.current.Where;
			}

			this.current = save;
			return result;
		}

		internal override SqlNode VisitUnion(SqlUnion su)
		{
			Scope save = this.current;
			this.current = null;
			SqlNode result = base.VisitUnion(su);
			this.current = save;
			return result;
		}
		internal override SqlSource VisitJoin(SqlJoin join)
		{
			// block where clauses from being lifted out of the cardinality-dependent 
			// side of an outer join.
			Scope save = this.current;
			try
			{
				switch(@join.JoinType)
				{
					case SqlJoinType.Cross:
					case SqlJoinType.CrossApply:
					case SqlJoinType.Inner:
						return base.VisitJoin(@join);
					case SqlJoinType.LeftOuter:
					case SqlJoinType.OuterApply:
					{
						@join.Left = this.VisitSource(@join.Left);
						this.current = null;
						@join.Right = this.VisitSource(@join.Right);
						@join.Condition = this.VisitExpression(@join.Condition);
						return @join;
					}
					default:
						this.current = null;
						return base.VisitJoin(@join);
				}
			}
			finally
			{
				this.current = save;
			}
		}
		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			// block where clauses from being lifted out of a sub-query
			Scope save = this.current;
			this.current = null;
			SqlExpression result = base.VisitSubSelect(ss);
			this.current = save;
			return result;
		}
		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			// block where clauses from being lifted out of a client-materialized sub-query
			Scope save = this.current;
			this.current = null;
			SqlExpression result = base.VisitClientQuery(cq);
			this.current = save;
			return result;
		}
	}
}