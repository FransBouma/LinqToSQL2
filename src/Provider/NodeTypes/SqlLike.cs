using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlLike : SqlSimpleTypeExpression {
		private SqlExpression expression;
		private SqlExpression pattern;
		private SqlExpression escape;

		internal SqlLike(Type clrType, ProviderType sqlType, SqlExpression expr, SqlExpression pattern, SqlExpression escape, Expression source)
			: base(SqlNodeType.Like, clrType, sqlType, source) {
			if (expr == null)
				throw Error.ArgumentNull("expr");
			if (pattern == null)
				throw Error.ArgumentNull("pattern");
			this.Expression = expr;
			this.Pattern = pattern;
			this.Escape = escape;
			}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (value.ClrType != typeof(string))
					throw Error.ArgumentWrongType("value", "string", value.ClrType);
				this.expression = value;
			}
		}

		internal SqlExpression Pattern {
			get { return this.pattern; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (value.ClrType != typeof(string))
					throw Error.ArgumentWrongType("value", "string", value.ClrType);
				this.pattern = value;
			}
		}

		internal SqlExpression Escape {
			get { return this.escape; }
			set {
				if (value != null && value.ClrType != typeof(string))
					throw Error.ArgumentWrongType("value", "string", value.ClrType);
				this.escape = value;
			}
		}
	}
}