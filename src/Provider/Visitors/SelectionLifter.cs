using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using SqlAliasesReferenced = System.Data.Linq.Provider.Common.SqlAliasesReferenced;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SelectionLifter : SqlVisitor
	{
		private Common.SqlAliasesReferenced aliases;
		private HashSet<SqlColumn> referencedColumns;
		private HashSet<SqlExpression> liftedExpressions;
		private bool hasLifted;
		private bool doLifting;
		private SqlAggregateChecker aggregateChecker;

		internal SelectionLifter(bool doLifting, HashSet<SqlAlias> aliasesForLifting, HashSet<SqlExpression> liftedExpressions)
		{
			this.doLifting = doLifting;
			this.aliases = new Common.SqlAliasesReferenced(aliasesForLifting);
			this.referencedColumns = new HashSet<SqlColumn>();
			this.liftedExpressions = liftedExpressions;
			this.CanLiftAll = true;
			if(doLifting)
				this.Lifted = new List<List<SqlColumn>>();
			this.aggregateChecker = new SqlAggregateChecker();
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			this.ReferenceColumns(@join.Condition);
			return base.VisitJoin(@join);
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			// reference all columns
			this.ReferenceColumns(@select.Where);
			foreach(SqlOrderExpression oe in @select.OrderBy)
			{
				this.ReferenceColumns(oe.Expression);
			}
			foreach(SqlExpression e in @select.GroupBy)
			{
				this.ReferenceColumns(e);
			}
			this.ReferenceColumns(@select.Having);

			// determine what if anything should be lifted from this select
			List<SqlColumn> lift = null;
			List<SqlColumn> keep = null;
			foreach(SqlColumn sc in @select.Row.Columns)
			{
				bool referencesAliasesForLifting = this.aliases.ReferencesAny(sc.Expression);
				bool isLockedExpression = this.referencedColumns.Contains(sc);
				if(referencesAliasesForLifting)
				{
					// 
					if(isLockedExpression)
					{
						this.CanLiftAll = false;
						this.ReferenceColumns(sc);
					}
					else
					{
						this.hasLifted = true;
						if(this.doLifting)
						{
							if(lift == null)
								lift = new List<SqlColumn>();
							lift.Add(sc);
						}
					}
				}
				else
				{
					if(this.doLifting)
					{
						if(keep == null)
							keep = new List<SqlColumn>();
						keep.Add(sc);
					}
					this.ReferenceColumns(sc);
				}
			}

			// check subqueries too
			if(this.CanLiftAll)
			{
				this.VisitSource(@select.From);
			}

			// don't allow lifting through these operations
			if(@select.Top != null ||
			   @select.GroupBy.Count > 0 ||
			   this.aggregateChecker.HasAggregates(@select) ||
			   @select.IsDistinct)
			{
				if(this.hasLifted)
				{
					// 
					this.CanLiftAll = false;
				}
			}

			// do the actual lifting for this select
			if(this.doLifting && this.CanLiftAll)
			{
				@select.Row.Columns.Clear();
				if(keep != null)
					@select.Row.Columns.AddRange(keep);
				if(lift != null)
				{
					// 
					this.Lifted.Add(lift);
				}
			}

			return @select;
		}

		private void ReferenceColumns(SqlExpression expression)
		{
			if(expression != null)
			{
				if(this.liftedExpressions == null || !this.liftedExpressions.Contains(expression))
				{
					SqlGatherReferencedColumns.Gather(expression, this.referencedColumns);
				}
			}
		}

		internal List<List<SqlColumn>> Lifted { get; private set; }
		internal bool CanLiftAll { get; private set; }
	}
}