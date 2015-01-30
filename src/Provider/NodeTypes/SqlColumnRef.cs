using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlColumnRef : SqlExpression {
		private SqlColumn column;
		internal SqlColumnRef(SqlColumn col)
			: base(SqlNodeType.ColumnRef, col.ClrType, col.SourceExpression) {
			this.column = col;
			}

		internal SqlColumn Column {
			get { return this.column; }
		}

		internal override ProviderType SqlType {
			get { return this.column.SqlType; }
		}

		public override bool Equals(object obj) {
			SqlColumnRef cref = obj as SqlColumnRef;
			return cref != null && cref.Column == this.column;
		}

		public override int GetHashCode() {
			return this.column.GetHashCode();
		}

		internal SqlColumn GetRootColumn() {
			SqlColumn c = this.column;
			while (c.Expression != null && c.Expression.NodeType == SqlNodeType.ColumnRef) {
				c = ((SqlColumnRef)c.Expression).Column;
			}
			return c;
		}
	}
}