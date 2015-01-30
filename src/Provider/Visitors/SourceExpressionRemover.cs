using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SourceExpressionRemover : DuplicatingVisitor {
		internal SourceExpressionRemover()
			: base(true) {
			}
		internal override SqlNode Visit(SqlNode node) {
			node = base.Visit(node);
			if (node != null) {
				node.ClearSourceExpression();
			}
			return node;
		}
		internal override SqlExpression VisitColumnRef(SqlColumnRef cref) {
			SqlExpression result = base.VisitColumnRef(cref);
			if (result != null && result == cref) {
				// reference to outer scope, don't propogate references to expressions or aliases
				SqlColumn col = cref.Column;
				SqlColumn newcol = new SqlColumn(col.ClrType, col.SqlType, col.Name, col.MetaMember, null, col.SourceExpression);
				newcol.Ordinal = col.Ordinal;
				result = new SqlColumnRef(newcol);
				newcol.ClearSourceExpression();
			}
			return result;
		}
		internal override SqlExpression VisitAliasRef(SqlAliasRef aref) {
			SqlExpression result = base.VisitAliasRef(aref);
			if (result != null && result == aref) {
				// reference to outer scope, don't propogate references to expressions or aliases
				SqlAlias alias = aref.Alias;
				SqlAlias newalias = new SqlAlias(new SqlNop(aref.ClrType, aref.SqlType, null));
				return new SqlAliasRef(newalias);
			}
			return result;
		}
	}
}