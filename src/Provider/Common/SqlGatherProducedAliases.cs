using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Walk a tree and return the set of unique aliases it produces.
	/// </summary>
	internal class SqlGatherProducedAliases
	{
		internal static HashSet<SqlAlias> Gather(SqlNode node)
		{
			ProducedAliasGatherer g = new ProducedAliasGatherer();
			g.Visit(node);
			return g.Produced;
		}
	}
}
