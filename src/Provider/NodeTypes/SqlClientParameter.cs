using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlClientParameter : SqlSimpleTypeExpression {
		// Expression<Func<object[], T>>
		LambdaExpression accessor;
		internal SqlClientParameter(Type clrType, ProviderType sqlType, LambdaExpression accessor, Expression sourceExpression):
			base(SqlNodeType.ClientParameter, clrType, sqlType, sourceExpression) {
			this.accessor = accessor;
			}
		internal LambdaExpression Accessor {
			get { return this.accessor; }
		}
	}
}