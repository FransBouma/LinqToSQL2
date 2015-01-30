using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlAssign : SqlStatement {
		private SqlExpression leftValue;
		private SqlExpression rightValue;

		internal SqlAssign(SqlExpression lValue, SqlExpression rValue, Expression sourceExpression)
			: base(SqlNodeType.Assign, sourceExpression) {
			this.LValue = lValue;
			this.RValue = rValue;
			}

		internal SqlExpression LValue {
			get { return this.leftValue; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.rightValue != null && !value.ClrType.IsAssignableFrom(this.rightValue.ClrType))
					throw Error.ArgumentWrongType("value", this.rightValue.ClrType, value.ClrType);
				this.leftValue = value;
			}
		}

		internal SqlExpression RValue {
			get { return this.rightValue; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.leftValue != null && !this.leftValue.ClrType.IsAssignableFrom(value.ClrType))
					throw Error.ArgumentWrongType("value", this.leftValue.ClrType, value.ClrType);
				this.rightValue = value;
			}
		}
	}
}