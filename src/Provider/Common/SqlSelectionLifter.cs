using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal static class SqlSelectionLifter
	{
		internal static bool CanLift(SqlSource source, HashSet<SqlAlias> aliasesForLifting, HashSet<SqlExpression> liftedExpressions)
		{
			SelectionLifter v = new SelectionLifter(false, aliasesForLifting, liftedExpressions);
			v.VisitSource(source);
			return v.CanLiftAll;
		}

		internal static List<List<SqlColumn>> Lift(SqlSource source, HashSet<SqlAlias> aliasesForLifting, HashSet<SqlExpression> liftedExpressions)
		{
			SelectionLifter v = new SelectionLifter(true, aliasesForLifting, liftedExpressions);
			v.VisitSource(source);
			return v.Lifted;
		}
	}
}