using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ColumnDeclarer : SqlVisitor {
		HashSet<SqlExpression> candidates;

		internal ColumnDeclarer() {
		}

		internal SqlExpression Declare(SqlExpression expression, HashSet<SqlExpression> candidates) {
			this.candidates = candidates;
			return (SqlExpression)this.Visit(expression);
		}

		internal override SqlNode Visit(SqlNode node) {
			SqlExpression expr = node as SqlExpression;
			if (expr != null) {
				if (this.candidates.Contains(expr)) {
					if (expr.NodeType == SqlNodeType.Column ||
						expr.NodeType == SqlNodeType.ColumnRef) {
							return expr;
						}
					else {
						return new SqlColumn(expr.ClrType, expr.SqlType, null, null, expr, expr.SourceExpression);
					}
				}
			}
			return base.Visit(node);
		}
	}
}