using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/// <summary>
	/// A SqlExpression with a simple implementation of ClrType and SqlType.
	/// </summary>
	internal abstract class SqlSimpleTypeExpression : SqlExpression {
		private ProviderType sqlType;

		internal SqlSimpleTypeExpression(SqlNodeType nodeType, Type clrType, ProviderType sqlType, Expression sourceExpression)
			: base(nodeType, clrType, sourceExpression) {
			this.sqlType = sqlType;
			}

		internal override ProviderType SqlType {
			get { return this.sqlType; }
		}

		internal void SetSqlType(ProviderType type) {
			this.sqlType = type;
		}
	}
}