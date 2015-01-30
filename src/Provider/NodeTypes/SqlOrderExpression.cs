namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlOrderExpression : IEquatable<SqlOrderExpression> {
		private SqlOrderType orderType;
		private SqlExpression expression;

		internal SqlOrderExpression(SqlOrderType type, SqlExpression expr) {
			this.OrderType = type;
			this.Expression = expr;
		}

		internal SqlOrderType OrderType {
			get { return this.orderType; }
			set { this.orderType = value; }
		}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.expression != null && !this.expression.ClrType.IsAssignableFrom(value.ClrType))
					throw Error.ArgumentWrongType("value", this.expression.ClrType, value.ClrType);
				this.expression = value;
			}
		}

		public override bool Equals(object obj) {
			if (this.EqualsTo(obj as SqlOrderExpression))
				return true;

			return base.Equals(obj);
		}

		public bool Equals(SqlOrderExpression other) {
			if (this.EqualsTo(other))
				return true;

			return base.Equals(other);
		}

		private bool EqualsTo(SqlOrderExpression other) {
			if (other == null)
				return false;
			if (object.ReferenceEquals(this, other))
				return true;
			if (this.OrderType != other.OrderType)
				return false;
			if (!this.Expression.SqlType.Equals(other.Expression.SqlType))
				return false;

			SqlColumn col1 = SqlOrderExpression.UnwrapColumn(this.Expression);
			SqlColumn col2 = SqlOrderExpression.UnwrapColumn(other.Expression);

			if (col1 == null || col2 == null)
				return false;

			return col1 == col2;
		}

		public override int GetHashCode() {
			SqlColumn col = SqlOrderExpression.UnwrapColumn(this.Expression);
			if (col != null)
				return col.GetHashCode();

			return base.GetHashCode();
		}

		private static SqlColumn UnwrapColumn(SqlExpression expr) {
			System.Diagnostics.Debug.Assert(expr != null);

			SqlUnary exprAsUnary = expr as SqlUnary;
			if (exprAsUnary != null) {
				expr = exprAsUnary.Operand;
			}

			SqlColumn exprAsColumn = expr as SqlColumn;
			if (exprAsColumn != null) {
				return exprAsColumn;
			}

			SqlColumnRef exprAsColumnRef = expr as SqlColumnRef;
			if (exprAsColumnRef != null) {
				return exprAsColumnRef.GetRootColumn();
			}
			//
			// For all other types return null to revert to default behavior for Equals()
			// and GetHashCode()
			//
			return null;
		}
	}
}