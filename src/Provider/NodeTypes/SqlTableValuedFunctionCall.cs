using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/// <summary>
	/// This class is used to represent a table value function.  It inherits normal function
	/// call functionality, and adds TVF specific members.
	/// </summary>
	internal class SqlTableValuedFunctionCall : SqlFunctionCall {
		private MetaType rowType;
		private List<SqlColumn> columns;

		internal SqlTableValuedFunctionCall(MetaType rowType, Type clrType, ProviderType sqlType, string name, IEnumerable <SqlExpression > args , Expression source)
			: base(SqlNodeType.TableValuedFunctionCall, clrType , sqlType, name, args, source) {
			this.rowType = rowType;
			this.columns = new List<SqlColumn>();
			}

		internal MetaType RowType {
			get { return this.rowType; }
		}

		internal List<SqlColumn> Columns {
			get { return this.columns; }
		}

		internal SqlColumn Find(string name) {
			foreach (SqlColumn c in this.Columns) {
				if (c.Name == name)
					return c;
			}
			return null;
		}

	}
}