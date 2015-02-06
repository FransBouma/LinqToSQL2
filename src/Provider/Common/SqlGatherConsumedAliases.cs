using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Walk a tree and return the set of unique aliases it consumes.
	/// </summary>
	internal class SqlGatherConsumedAliases
	{
		internal static HashSet<SqlAlias> Gather(SqlNode node)
		{
			ConsumedAliaseGatherer g = new ConsumedAliaseGatherer();
			g.Visit(node);
			return g.Consumed;
		}
	}
}
