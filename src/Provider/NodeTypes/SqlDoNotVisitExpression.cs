using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlDoNotVisitExpression : SqlExpression {
		private SqlExpression expression;

		internal SqlDoNotVisitExpression(SqlExpression expr)
			: base(SqlNodeType.DoNotVisit, expr.ClrType, expr.SourceExpression) {
			if (expr == null)
				throw Error.ArgumentNull("expr");
			this.expression = expr;
			}

		internal SqlExpression Expression {
			get { return this.expression; }
		}

		internal override ProviderType SqlType {
			get { return this.expression.SqlType; }
		}
	}
}