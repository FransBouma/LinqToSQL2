using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlDelete : SqlStatement {
		private SqlSelect select;

		internal SqlDelete(SqlSelect select, Expression sourceExpression)
			: base(SqlNodeType.Delete, sourceExpression) {
			this.Select = select;
			}

		internal SqlSelect Select {
			get { return this.select; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.select = value;
			}
		}
	}
}