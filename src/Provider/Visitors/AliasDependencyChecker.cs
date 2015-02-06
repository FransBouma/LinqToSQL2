using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class AliasDependencyChecker : SqlVisitor
	{
		HashSet<SqlAlias> aliasesToCheck;
		HashSet<SqlExpression> ignoreExpressions;
		internal bool hasDependency;

		internal AliasDependencyChecker(HashSet<SqlAlias> aliasesToCheck, HashSet<SqlExpression> ignoreExpressions)
		{
			this.aliasesToCheck = aliasesToCheck;
			this.ignoreExpressions = ignoreExpressions;
		}
		internal override SqlNode Visit(SqlNode node)
		{
			SqlExpression e = node as SqlExpression;
			if(this.hasDependency)
				return node;
			if(e != null && this.ignoreExpressions.Contains(e))
			{
				return node;
			}
			return base.Visit(node);
		}
		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			if(this.aliasesToCheck.Contains(cref.Column.Alias))
			{
				this.hasDependency = true;
			}
			else if(cref.Column.Expression != null)
			{
				this.Visit(cref.Column.Expression);
			}
			return cref;
		}
		internal override SqlExpression VisitColumn(SqlColumn col)
		{
			if(col.Expression != null)
			{
				this.Visit(col.Expression);
			}
			return col;
		}
	}
}