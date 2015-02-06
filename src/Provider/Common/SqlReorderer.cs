using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// moves order-by clauses from sub-queries to outer-most or top selects
	/// removes ordering in correlated sub-queries
	/// </summary>
	internal class SqlReorderer
	{
		#region Member Declarations
		private TypeSystemProvider typeProvider;
		private NodeFactory sql;
		#endregion

		internal SqlReorderer(TypeSystemProvider typeProvider, NodeFactory sqlFactory)
		{
			this.typeProvider = typeProvider;
			this.sql = sqlFactory;
		}

		internal SqlNode Reorder(SqlNode node)
		{
			return new OrderByLifter(this.typeProvider, this.sql).Visit(node);
		}
	}
}
