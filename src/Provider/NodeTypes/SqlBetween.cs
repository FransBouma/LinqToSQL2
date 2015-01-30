using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlBetween : SqlSimpleTypeExpression {
		SqlExpression expression;
		SqlExpression start;
		SqlExpression end;

		internal SqlBetween(Type clrType, ProviderType sqlType, SqlExpression expr, SqlExpression start, SqlExpression end, Expression source)
			: base(SqlNodeType.Between, clrType, sqlType, source) {
			this.expression = expr;
			this.start = start;
			this.end = end;
			}

		internal SqlExpression Expression {
			get { return this.expression; }
			set { this.expression = value; }
		}

		internal SqlExpression Start {
			get { return this.start; }
			set { this.start = value; }
		}

		internal SqlExpression End {
			get { return this.end; }
			set { this.end = value; }
		}
	}
}