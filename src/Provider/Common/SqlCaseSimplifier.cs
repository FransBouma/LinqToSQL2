using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{

	/// <summary>
	/// SQL with CASE statements is harder to read. This visitor attempts to reduce CASE
	/// statements to equivalent (but easier to read) logic.
	/// </summary>
	internal class SqlCaseSimplifier
	{
		internal static SqlNode Simplify(SqlNode node, NodeFactory sql)
		{
			return new CaseSimplifier(sql).Visit(node);
		}
	}
}
