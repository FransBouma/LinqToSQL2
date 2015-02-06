using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Common
{

	/// <summary>
	/// Hoist WHERE clauses as close to the root as possible.
	/// </summary>
	internal class SqlLiftWhereClauses
	{
		internal static SqlNode Lift(SqlNode node, NodeFactory factory)
		{
			return new WhereClauseLifter(factory).Visit(node);
		}
	}
}
