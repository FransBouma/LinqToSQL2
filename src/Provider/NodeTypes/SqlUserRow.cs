using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlUserRow : SqlSimpleTypeExpression {
		private SqlUserQuery query;
		private MetaType rowType;

		internal SqlUserRow(MetaType rowType, ProviderType sqlType, SqlUserQuery query, Expression source)
			: base(SqlNodeType.UserRow, rowType.Type, sqlType, source) {
			this.Query = query;
			this.rowType = rowType;
			}

		internal MetaType RowType {
			get { return this.rowType; }
		}

		internal SqlUserQuery Query {
			get { return this.query; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (value.Projection != null && value.Projection.ClrType != this.ClrType)
					throw Error.ArgumentWrongType("value", this.ClrType, value.Projection.ClrType);
				this.query = value;
			}
		}
	}
}