using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlUserQuery : SqlNode {
		private string queryText;
		private SqlExpression projection;
		private List<SqlExpression> args;
		private List<SqlUserColumn> columns;

		internal SqlUserQuery(SqlNodeType nt, SqlExpression projection, IEnumerable<SqlExpression> args, Expression source)
			: base(nt, source) {
			this.Projection = projection;
			this.args = (args != null) ? new List<SqlExpression>(args) : new List<SqlExpression>();
			this.columns = new List<SqlUserColumn>();
			}

		internal SqlUserQuery(string queryText, SqlExpression projection, IEnumerable<SqlExpression> args, Expression source)
			: base(SqlNodeType.UserQuery, source) {
			this.queryText = queryText;
			this.Projection = projection;
			this.args = (args != null) ? new List<SqlExpression>(args) : new List<SqlExpression>();
			this.columns = new List<SqlUserColumn>();
			}

		internal string QueryText {
			get { return this.queryText; }
		}

		internal SqlExpression Projection {
			get { return this.projection; }
			set {
				if (this.projection != null && this.projection.ClrType != value.ClrType)
					throw Error.ArgumentWrongType("value", this.projection.ClrType, value.ClrType);
				this.projection = value;
			}
		}

		internal List<SqlExpression> Arguments {
			get { return this.args; }
		}

		internal List<SqlUserColumn> Columns {
			get { return this.columns; }
		}

		internal SqlUserColumn Find(string name) {
			foreach (SqlUserColumn c in this.Columns) {
				if (c.Name == name)
					return c;
			}
			return null;
		}
	}
}