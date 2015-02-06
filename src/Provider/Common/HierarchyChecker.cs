using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class HierarchyChecker
	{
		internal static bool HasHierarchy(SqlExpression expr)
		{
			QueryHierarchyFinder v = new QueryHierarchyFinder();
			v.Visit(expr);
			return v.FoundHierarchy;
		}
	}
}