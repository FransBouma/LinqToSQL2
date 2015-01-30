using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlStoredProcedureCall : SqlUserQuery {
		private MetaFunction function;

		internal SqlStoredProcedureCall(MetaFunction function, SqlExpression projection, IEnumerable<SqlExpression> args, Expression source)
			: base(SqlNodeType.StoredProcedureCall, projection, args, source) {
			if (function == null)
				throw Error.ArgumentNull("function");
			this.function = function;
			}

		internal MetaFunction Function {
			get { return this.function; }
		}
	}
}