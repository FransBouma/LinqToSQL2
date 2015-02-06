using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// converts correlated scalar subqueries into outer-applies
	/// </summary>
	/// <remarks>must be run after flattener.</remarks>
	internal class SqlRewriteScalarSubqueries
	{
		ScalarSubQueryRewriter visitor;

		internal SqlRewriteScalarSubqueries(NodeFactory sqlFactory)
		{
			this.visitor = new ScalarSubQueryRewriter(sqlFactory);
		}

		internal SqlNode Rewrite(SqlNode node)
		{
			return this.visitor.Visit(node);
		}
	}
}
