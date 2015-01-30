using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlTable : SqlNode {
		private MetaTable table;
		private MetaType rowType;
		private ProviderType sqlRowType;
		private List<SqlColumn> columns;

		internal SqlTable(MetaTable table, MetaType rowType, ProviderType sqlRowType, Expression sourceExpression)
			: base(SqlNodeType.Table, sourceExpression) {
			this.table = table;
			this.rowType = rowType;
			this.sqlRowType = sqlRowType;
			this.columns = new List<SqlColumn>();
			}

		internal MetaTable MetaTable {
			get { return this.table; }
		}

		internal string Name {
			get { return this.table.TableName; }
		}

		internal List<SqlColumn> Columns {
			get { return this.columns; }
		}

		internal MetaType RowType {
			get { return this.rowType; }
		}

		internal ProviderType SqlRowType {
			get { return this.sqlRowType; }
		}

		internal SqlColumn Find(string columnName) {
			foreach (SqlColumn c in this.Columns) {
				if (c.Name == columnName)
					return c;
			}
			return null;
		}

	}
}