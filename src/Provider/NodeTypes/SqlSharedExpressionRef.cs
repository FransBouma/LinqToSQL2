using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlSharedExpressionRef : SqlExpression {
		private SqlSharedExpression expr;

		internal SqlSharedExpressionRef(SqlSharedExpression expr)
			: base(SqlNodeType.SharedExpressionRef, expr.ClrType, expr.SourceExpression) {
			this.expr = expr;
			}

		internal SqlSharedExpression SharedExpression {
			get { return this.expr; }
		}

		internal override ProviderType SqlType {
			get { return this.expr.SqlType; }
		}
	}
}