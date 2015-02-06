using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// convert multiset & element expressions into separate queries 
	/// </summary>
	internal class SqlMultiplexer
	{
		MultiSetMultiPlexer _multiSetMultiPlexor;
		
		internal SqlMultiplexer(SqlMultiplexerOptionType options, IEnumerable<SqlParameter> parentParameters, NodeFactory sqlFactory)
		{
			this._multiSetMultiPlexor = new MultiSetMultiPlexer(options, parentParameters, sqlFactory);
		}

		internal SqlNode Multiplex(SqlNode node)
		{
			return this._multiSetMultiPlexor.Visit(node);
		}
	}
}
