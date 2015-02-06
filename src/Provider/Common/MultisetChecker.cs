using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class MultisetChecker
	{
		internal static bool HasMultiset(SqlExpression expr)
		{
			MultiSetDetector v = new MultiSetDetector();
			v.Visit(expr);
			return v.FoundMultiset;
		}
	}
}