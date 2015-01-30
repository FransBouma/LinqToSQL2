using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlSelect : SqlStatement {
		private SqlExpression top;
		private bool isPercent;
		private bool isDistinct;
		private SqlExpression selection;
		private SqlRow row;
		private SqlSource from;
		private SqlExpression where;
		private List<SqlExpression> groupBy;
		private SqlExpression having;
		private List<SqlOrderExpression> orderBy;
		private SqlOrderingType orderingType;
		private bool squelch;

		internal SqlSelect(SqlExpression selection, SqlSource from, Expression sourceExpression)
			: base(SqlNodeType.Select, sourceExpression) {
			this.Row = new SqlRow(sourceExpression);
			this.Selection = selection;
			this.From = from;
			this.groupBy = new List<SqlExpression>();
			this.orderBy = new List<SqlOrderExpression>();
			this.orderingType = SqlOrderingType.Default;
			}

		internal SqlExpression Top {
			get { return this.top; }
			set { this.top = value; }
		}

		internal bool IsPercent {
			get { return this.isPercent; }
			set { this.isPercent = value; }
		}

		internal bool IsDistinct {
			get { return this.isDistinct; }
			set { this.isDistinct = value; }
		}

		internal SqlExpression Selection {
			get { return this.selection; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.selection = value;
			}
		}

		internal SqlRow Row {
			get { return this.row; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.row = value;
			}
		}

		internal SqlSource From {
			get { return this.from; }
			set { this.from = value; }
		}

		internal SqlExpression Where {
			get { return this.where; }
			set {
				if (value != null && TypeSystem.GetNonNullableType(value.ClrType) != typeof(bool)) {
					throw Error.ArgumentWrongType("value", "bool", value.ClrType);
				}
				this.where = value;
			}
		}

		internal List<SqlExpression> GroupBy {
			get { return this.groupBy; }
		}

		internal SqlExpression Having {
			get { return this.having; }
			set {
				if (value != null && TypeSystem.GetNonNullableType(value.ClrType) != typeof(bool)) {
					throw Error.ArgumentWrongType("value", "bool", value.ClrType);
				}
				this.having = value;
			}
		}

		internal List<SqlOrderExpression> OrderBy {
			get { return this.orderBy; }
		}

		internal SqlOrderingType OrderingType {
			get { return this.orderingType; }
			set { this.orderingType = value; }
		}

		internal bool DoNotOutput {
			get { return this.squelch; }
			set { this.squelch = value; }
		}
	}
}