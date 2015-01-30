using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlIn : SqlSimpleTypeExpression {
		private SqlExpression expression;
		private List<SqlExpression> values;

		internal SqlIn(Type clrType, ProviderType sqlType, SqlExpression expression, IEnumerable<SqlExpression> values, Expression sourceExpression)
			:base(SqlNodeType.In, clrType, sqlType, sourceExpression) {
			this.expression = expression;
			this.values = values != null ? new List<SqlExpression>(values) : new List<SqlExpression>(0);
			}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null) {
					throw Error.ArgumentNull("value");
				}
				this.expression = value;
			}
		}
		internal List<SqlExpression> Values {
			get { return this.values; }
		}
	}
}