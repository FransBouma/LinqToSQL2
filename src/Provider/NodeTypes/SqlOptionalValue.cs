namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlOptionalValue : SqlSimpleTypeExpression {
		private SqlExpression hasValue;
		private SqlExpression expressionValue;

		internal SqlOptionalValue( SqlExpression hasValue, SqlExpression value)
			: base(SqlNodeType.OptionalValue, value.ClrType, value.SqlType, value.SourceExpression) {
			this.HasValue = hasValue;
			this.Value = value;
			}

		internal SqlExpression HasValue {
			get { return this.hasValue; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.hasValue = value;
			}
		}

		internal SqlExpression Value {
			get { return this.expressionValue; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (value.ClrType != this.ClrType)
					throw Error.ArgumentWrongType("value", this.ClrType, value.ClrType);
				this.expressionValue = value;
			}
		}
	}
}