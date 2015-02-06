using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class AliasMapper : SqlVisitor
	{
		#region Member Declarations
		private Dictionary<SqlColumn, SqlAlias> _aliasMap;
		private SqlAlias _currentAlias;
		#endregion

		internal AliasMapper(Dictionary<SqlColumn, SqlAlias> aliasMap)
		{
			_aliasMap = aliasMap;
		}

		internal override SqlAlias VisitAlias(SqlAlias a)
		{
			SqlAlias save = _currentAlias;
			_currentAlias = a;
			base.VisitAlias(a);
			_currentAlias = save;
			return a;
		}

		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			_aliasMap[col] = _currentAlias;
			this.Visit(col.Expression);
			return col;
		}

		internal override SqlRow VisitRow(SqlRow row)
		{
			foreach(SqlColumn col in row.Columns)
			{
				this.VisitColumn(col);
			}
			return row;
		}

		internal override SqlTable VisitTable(SqlTable tab)
		{
			foreach(SqlColumn col in tab.Columns)
			{
				this.VisitColumn(col);
			}
			return tab;
		}

		internal override SqlExpression VisitTableValuedFunctionCall(SqlTableValuedFunctionCall fc)
		{
			foreach(SqlColumn col in fc.Columns)
			{
				this.VisitColumn(col);
			}
			return fc;
		}
	}
}