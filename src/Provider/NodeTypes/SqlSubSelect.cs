using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlSubSelect : SqlSimpleTypeExpression {
		private SqlSelect select;

		internal SqlSubSelect(SqlNodeType nt , Type clrType, ProviderType sqlType , SqlSelect select)
			: base(nt, clrType, sqlType, select.SourceExpression) {
			switch (nt) {
				case SqlNodeType.Multiset:
				case SqlNodeType.ScalarSubSelect:
				case SqlNodeType.Element:
				case SqlNodeType.Exists:
					break;
				default:
					throw Error.UnexpectedNode(nt);
			}
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