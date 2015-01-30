using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlRowNumber : SqlSimpleTypeExpression {
		private List<SqlOrderExpression> orderBy;

		internal List<SqlOrderExpression> OrderBy {
			get { return orderBy; }
		}

		internal SqlRowNumber(Type clrType, ProviderType sqlType, List<SqlOrderExpression> orderByList, Expression sourceExpression)
			: base(SqlNodeType.RowNumber, clrType, sqlType, sourceExpression) {
			if (orderByList == null) {
				throw Error.ArgumentNull("orderByList");
			}

			this.orderBy = orderByList;
			}
	}
}