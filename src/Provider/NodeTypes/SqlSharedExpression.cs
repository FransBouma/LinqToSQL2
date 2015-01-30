using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlSharedExpression : SqlExpression {
		private SqlExpression expr;

		internal SqlSharedExpression(SqlExpression expr)
			: base(SqlNodeType.SharedExpression, expr.ClrType, expr.SourceExpression) {
			this.expr = expr;
			}

		internal SqlExpression Expression {
			get { return this.expr; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (!this.ClrType.IsAssignableFrom(value.ClrType)
					&& !value.ClrType.IsAssignableFrom(this.ClrType))
					throw Error.ArgumentWrongType("value", this.ClrType, value.ClrType);
				this.expr = value;
			}
		}

		internal override ProviderType SqlType {
			get { return this.expr.SqlType; }
		}
	}
}