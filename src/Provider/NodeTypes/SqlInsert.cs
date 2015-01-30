using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlInsert : SqlStatement {
		private SqlTable table;
		private SqlRow row;
		private SqlExpression expression;
		private SqlColumn outputKey;
		private bool outputToLocal;

		internal SqlInsert(SqlTable table, SqlExpression expr, Expression sourceExpression)
			: base(SqlNodeType.Insert, sourceExpression) {
			this.Table = table;
			this.Expression = expr;
			this.Row = new SqlRow(sourceExpression);
			}

		internal SqlTable Table {
			get { return this.table; }
			set {
				if (value == null)
					throw Error.ArgumentNull("null");
				this.table = value;
			}
		}

		internal SqlRow Row {
			get { return this.row; }
			set { this.row = value; }
		}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("null");
				if (!this.table.RowType.Type.IsAssignableFrom(value.ClrType))
					throw Error.ArgumentWrongType("value", this.table.RowType, value.ClrType);
				this.expression = value;
			}
		}

		internal SqlColumn OutputKey {
			get { return this.outputKey; }
			set { this.outputKey = value; }
		}

		internal bool OutputToLocal {
			get { return this.outputToLocal; }
			set { this.outputToLocal = value; }
		}
	}
}