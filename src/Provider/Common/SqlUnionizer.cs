using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlUnionizer
	{
		internal static SqlNode Unionize(SqlNode node)
		{
			return new QueryUnionizer().Visit(node);
		}
	}
}
