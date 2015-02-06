using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ConsumedAliaseGatherer : SqlVisitor
	{
		private HashSet<SqlAlias> _consumed = new HashSet<SqlAlias>();

		internal void VisitAliasConsumed(SqlAlias a)
		{
			_consumed.Add(a);
		}

		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			VisitAliasConsumed(col.Alias);
			VisitExpression(col.Expression);
			return col;
		}
		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			VisitAliasConsumed(cref.Column.Alias);
			return cref;
		}

		internal HashSet<SqlAlias> Consumed
		{
			get { return _consumed; }
		}
	}
}