using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{

	/// <summary>
	/// Find projection expressions on the right side of CROSS APPLY that do not 
	/// depend exclusively on right side productions and move them outside of the 
	/// CROSS APPLY by enclosing the CROSS APPLY with a new source.
	/// </summary>
	internal class SqlLiftIndependentRowExpressions
	{
		internal static SqlNode Lift(SqlNode node)
		{
			ColumnLifter cl = new ColumnLifter();
			node = cl.Visit(node);
			return node;
		}
	}
}
