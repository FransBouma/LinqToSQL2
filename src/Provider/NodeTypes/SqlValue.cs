using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlValue : SqlSimpleTypeExpression {
		private object value;
		private bool isClient;

		internal SqlValue(Type clrType, ProviderType sqlType, object value, bool isClientSpecified, Expression sourceExpression)
			: base(SqlNodeType.Value, clrType, sqlType, sourceExpression) {
			this.value = value;
			this.isClient = isClientSpecified;
			}

		internal object Value {
			get { return this.value; }
		}

		internal bool IsClientSpecified {
			get { return this.isClient; }
		}
	}
}