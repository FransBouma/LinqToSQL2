using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlRow : SqlNode {
		private List<SqlColumn> columns;

		internal SqlRow(Expression sourceExpression)
			: base(SqlNodeType.Row, sourceExpression) {
			this.columns = new List<SqlColumn>();
			}

		internal List<SqlColumn> Columns {
			get { return this.columns; }
		}

		internal SqlColumn Find(string name) {
			foreach (SqlColumn c in this.columns) {
				if (name == c.Name)
					return c;
			}
			return null;
		}
	}
}