using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlSimpleExpression : SqlExpression {
		private SqlExpression expr;

		internal SqlSimpleExpression(SqlExpression expr)
			: base(SqlNodeType.SimpleExpression, expr.ClrType, expr.SourceExpression) {
			this.expr = expr;
			}

		internal SqlExpression Expression {
			get { return this.expr; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (!TypeSystem.GetNonNullableType(this.ClrType).IsAssignableFrom(TypeSystem.GetNonNullableType(value.ClrType)))
					throw Error.ArgumentWrongType("value", this.ClrType, value.ClrType);
				this.expr = value;
			}
		}

		internal override ProviderType SqlType {
			get { return this.expr.SqlType; }
		}
	}
}