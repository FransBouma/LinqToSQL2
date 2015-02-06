using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Class which flatten object expressions into rows
	/// </summary>
	internal class SqlFlattener
	{
		ObjectExpressionFlattener visitor;

		internal SqlFlattener(NodeFactory sql, SqlColumnizer columnizer)
		{
			this.visitor = new ObjectExpressionFlattener(sql, columnizer);
		}

		internal SqlNode Flatten(SqlNode node)
		{
			node = this.visitor.Visit(node);
			return node;
		}
	}
}
