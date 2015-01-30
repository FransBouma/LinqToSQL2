using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlJoinedCollection : SqlSimpleTypeExpression {
		private SqlExpression expression;
		private SqlExpression count;

		internal SqlJoinedCollection(Type clrType, ProviderType sqlType, SqlExpression expression, SqlExpression count, Expression sourceExpression)
			: base(SqlNodeType.JoinedCollection, clrType, sqlType, sourceExpression) {
			this.expression = expression;
			this.count = count;
			}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null || this.expression != null && this.expression.ClrType != value.ClrType)
					throw Error.ArgumentWrongType(value, this.expression.ClrType, value.ClrType);
				this.expression = value;
			}
		}

		internal SqlExpression Count {
			get { return this.count; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (value.ClrType != typeof(int))
					throw Error.ArgumentWrongType(value, typeof(int), value.ClrType);
				this.count = value;
			}
		}
	}
}