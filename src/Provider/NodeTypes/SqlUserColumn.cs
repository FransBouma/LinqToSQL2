using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlUserColumn : SqlSimpleTypeExpression {
		private SqlUserQuery query;
		private string name;
		private bool isRequired;

		internal SqlUserColumn(Type clrType, ProviderType sqlType, SqlUserQuery query, string name, bool isRequired, Expression source)
			: base(SqlNodeType.UserColumn, clrType, sqlType, source) {
			this.Query = query;
			this.name = name;
			this.isRequired = isRequired;
			}

		internal SqlUserQuery Query {
			get { return this.query; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.query != null && this.query != value)
					throw Error.ArgumentWrongValue("value");
				this.query = value;
			}
		}

		internal string Name {
			get { return this.name; }
		}

		internal bool IsRequired {
			get { return this.isRequired; }
		}
	}
}