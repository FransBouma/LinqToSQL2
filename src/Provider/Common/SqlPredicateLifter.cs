using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal static class SqlPredicateLifter
	{
		internal static bool CanLift(SqlSource source, HashSet<SqlAlias> aliasesForLifting, HashSet<SqlExpression> liftedExpressions)
		{
			Diagnostics.Debug.Assert(source != null);
			Diagnostics.Debug.Assert(aliasesForLifting != null);
			PredicateLifter v = new PredicateLifter(false, aliasesForLifting, liftedExpressions);
			v.VisitSource(source);
			return v.CanLiftAll;
		}

		internal static SqlExpression Lift(SqlSource source, HashSet<SqlAlias> aliasesForLifting)
		{
			Diagnostics.Debug.Assert(source != null);
			Diagnostics.Debug.Assert(aliasesForLifting != null);
			PredicateLifter v = new PredicateLifter(true, aliasesForLifting, null);
			v.VisitSource(source);
			return v.Lifted;
		}
	}
}