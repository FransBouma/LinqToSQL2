using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlWhen {
		private SqlExpression matchExpression;
		private SqlExpression valueExpression;

		internal SqlWhen(SqlExpression match, SqlExpression value) {
			// 'match' may be null when this when represents the ELSE condition.
			if (value == null)
				throw Error.ArgumentNull("value");
			this.Match = match;
			this.Value = value;
		}

		internal SqlExpression Match {
			get { return this.matchExpression; }
			set {
				if (this.matchExpression != null && value != null && this.matchExpression.ClrType != value.ClrType
					// Exception: bool types, because predicates can have type bool or bool?
					&& !TypeSystem.GetNonNullableType(this.matchExpression.ClrType).Equals(typeof(bool))
					&& !TypeSystem.GetNonNullableType(value.ClrType).Equals(typeof(bool)))
					throw Error.ArgumentWrongType("value", this.matchExpression.ClrType, value.ClrType);
				this.matchExpression = value;
			}
		}

		internal SqlExpression Value {
			get { return this.valueExpression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.valueExpression != null && !this.valueExpression.ClrType.IsAssignableFrom(value.ClrType))
					throw Error.ArgumentWrongType("value", this.valueExpression.ClrType, value.ClrType);
				this.valueExpression = value;
			}
		}
	}
}