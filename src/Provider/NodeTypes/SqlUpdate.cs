using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlUpdate : SqlStatement {
		private SqlSelect select;
		private List<SqlAssign> assignments;

		internal SqlUpdate(SqlSelect select, IEnumerable<SqlAssign> assignments, Expression sourceExpression)
			: base(SqlNodeType.Update, sourceExpression) {
			this.Select = select;
			this.assignments = new List<SqlAssign>(assignments);
			}

		internal SqlSelect Select {
			get { return this.select; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.select = value;
			}
		}

		internal List<SqlAssign> Assignments {
			get { return this.assignments; }
		}
	}
}