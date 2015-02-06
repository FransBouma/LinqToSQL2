using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class PredicateLifter : SqlVisitor
	{
		private Common.SqlAliasesReferenced aliases;
		private HashSet<SqlExpression> liftedExpressions;
		private bool doLifting;
		private SqlAggregateChecker aggregateChecker;
		
		internal PredicateLifter(bool doLifting, HashSet<SqlAlias> aliasesForLifting, HashSet<SqlExpression> liftedExpressions)
		{
			this.doLifting = doLifting;
			this.aliases = new Common.SqlAliasesReferenced(aliasesForLifting);
			this.liftedExpressions = liftedExpressions;
			this.CanLiftAll = true;
			this.aggregateChecker = new SqlAggregateChecker();
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			// check subqueries first
			this.VisitSource(@select.From);

			// don't allow lifting through these operations
			if(@select.Top != null ||
			   @select.GroupBy.Count > 0 ||
			   this.aggregateChecker.HasAggregates(@select) ||
			   @select.IsDistinct)
			{
				this.CanLiftAll = false;
			}

			// only lift predicates that actually reference the aliases
			if(this.CanLiftAll && @select.Where != null)
			{
				bool referencesAliases = this.aliases.ReferencesAny(@select.Where);
				if(referencesAliases)
				{
					if(this.liftedExpressions != null)
					{
						this.liftedExpressions.Add(@select.Where);
					}
					if(this.doLifting)
					{
						if(this.Lifted != null)
							this.Lifted = new SqlBinary(SqlNodeType.And, this.Lifted.ClrType, this.Lifted.SqlType, this.Lifted, @select.Where);
						else
							this.Lifted = @select.Where;
						@select.Where = null;
					}
				}
			}

			return @select;
		}

		internal bool CanLiftAll { get; private set; }
		internal SqlExpression Lifted { get; private set; }
	}
}