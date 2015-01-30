using System.Collections.Generic;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlClientQuery : SqlSimpleTypeExpression {
		private SqlSubSelect query;
		private List<SqlExpression> arguments;
		private List<SqlParameter> parameters;
		int ordinal;

		internal SqlClientQuery(SqlSubSelect subquery)
			: base(SqlNodeType.ClientQuery, subquery.ClrType, subquery.SqlType, subquery.SourceExpression) {
			this.query = subquery;
			this.arguments = new List<SqlExpression>();
			this.parameters = new List<SqlParameter>();
			}

		internal SqlSubSelect Query {
			get { return this.query; }
			set {
				if (value == null || (this.query != null && this.query.ClrType != value.ClrType))
					throw Error.ArgumentWrongType(value, this.query.ClrType, value.ClrType);
				this.query = value;
			}
		}

		internal List<SqlExpression> Arguments {
			get { return this.arguments; }
		}

		internal List<SqlParameter> Parameters {
			get { return this.parameters; }
		}

		internal int Ordinal {
			get { return this.ordinal; }
			set { this.ordinal = value; }
		}
	}
}