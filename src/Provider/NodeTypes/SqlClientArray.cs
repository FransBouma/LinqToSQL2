using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlClientArray : SqlSimpleTypeExpression {
		private List<SqlExpression> expressions;

		internal SqlClientArray(Type clrType, ProviderType sqlType, SqlExpression[ ] exprs, Expression sourceExpression)
			: base(SqlNodeType.ClientArray, clrType, sqlType, sourceExpression) {
			this.expressions = new List<SqlExpression>();
			if (exprs != null)
				this.Expressions.AddRange(exprs);
			}

		internal List<SqlExpression> Expressions {
			get { return this.expressions; }
		}
	}
}