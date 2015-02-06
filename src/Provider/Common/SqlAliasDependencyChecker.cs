using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal static class SqlAliasDependencyChecker
	{
		internal static bool IsDependent(SqlNode node, HashSet<SqlAlias> aliasesToCheck, HashSet<SqlExpression> ignoreExpressions)
		{
			AliasDependencyChecker v = new AliasDependencyChecker(aliasesToCheck, ignoreExpressions);
			v.Visit(node);
			return v.hasDependency;
		}
	}
}