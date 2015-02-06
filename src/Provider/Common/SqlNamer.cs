using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{

	internal class SqlNamer
	{
		private NameAssigner visitor;

		internal SqlNamer()
		{
			this.visitor = new NameAssigner();
		}

		internal SqlNode AssignNames(SqlNode node)
		{
			return this.visitor.Visit(node);
		}

		internal static string DiscoverName(SqlExpression e)
		{
			if(e != null)
			{
				switch(e.NodeType)
				{
					case SqlNodeType.Column:
						return DiscoverName(((SqlColumn)e).Expression);
					case SqlNodeType.ColumnRef:
						SqlColumnRef cref = (SqlColumnRef)e;
						if(cref.Column.Name != null) return cref.Column.Name;
						return DiscoverName(cref.Column);
					case SqlNodeType.ExprSet:
						SqlExprSet eset = (SqlExprSet)e;
						return DiscoverName(eset.Expressions[0]);
				}
			}
			return "value";
		}
	}
}
