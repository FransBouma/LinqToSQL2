using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlDeflator
	{
		#region Member Declarations
		private SqlValueDeflator _valueDeflator;
		private SqlColumnDeflator _columnDeflator;
		private SqlAliasDeflator _aliasDeflator;
		private SqlTopSelectDeflator _topselectDeflator;
		private SqlDuplicateColumnDeflator _duplicateColumnDeflator;
		#endregion

		internal SqlDeflator()
		{
			_valueDeflator = new SqlValueDeflator();
			_columnDeflator = new SqlColumnDeflator();
			_aliasDeflator = new SqlAliasDeflator();
			_topselectDeflator = new SqlTopSelectDeflator();
			_duplicateColumnDeflator = new SqlDuplicateColumnDeflator();
		}

		internal SqlNode Deflate(SqlNode node)
		{
			node = _valueDeflator.Visit(node);
			node = _columnDeflator.Visit(node);
			node = _aliasDeflator.Visit(node);
			node = _topselectDeflator.Visit(node);
			node = _duplicateColumnDeflator.Visit(node);
			return node;
		}
	}
}
