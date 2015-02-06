using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{

	/// <summary>
	/// duplicates an expression up until a column or column ref is encountered
	/// goes 'deep' through alias ref's
	/// assumes that columnizing has been done already
	/// </summary>
	internal class SqlExpander
	{
		NodeFactory factory;

		internal SqlExpander(NodeFactory factory)
		{
			this.factory = factory;
		}

		internal SqlExpression Expand(SqlExpression exp)
		{
			return (new ExpressionDuplicator(this.factory)).VisitExpression(exp);
		}
	}
}
