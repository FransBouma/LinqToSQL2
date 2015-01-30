using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlExprSet : SqlExpression {
		private List<SqlExpression> expressions;

		internal SqlExprSet(Type clrType, IEnumerable <SqlExpression> exprs, Expression sourceExpression)
			: base(SqlNodeType.ExprSet, clrType, sourceExpression) {
			this.expressions = new List<SqlExpression>(exprs);
			}

		internal List<SqlExpression> Expressions {
			get { return this.expressions; }
		}

		/// <summary>
		/// Get the first non-set expression of the set by drilling
		/// down the left expressions.
		/// </summary>
		internal SqlExpression GetFirstExpression() {
			SqlExpression expr = expressions[0];
			while (expr is SqlExprSet) {
				expr = ((SqlExprSet)expr).Expressions[0];
			}
			return expr;
		}

		internal override ProviderType SqlType {
			get { return this.expressions[0].SqlType; }
		}
	}
}