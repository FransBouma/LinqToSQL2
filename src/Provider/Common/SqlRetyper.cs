using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlRetyper
	{
		TypeCorrector visitor;

		internal SqlRetyper(NodeFactory factory)
		{
			this.visitor = new TypeCorrector(factory);
		}

		internal SqlNode Retype(SqlNode node)
		{
			return this.visitor.Visit(node);
		}
	}
}
