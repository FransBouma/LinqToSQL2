using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlDiscriminatorOf : SqlSimpleTypeExpression {
		SqlExpression obj;
		internal SqlDiscriminatorOf(SqlExpression obj, Type clrType, ProviderType sqlType, Expression sourceExpression)
			: base(SqlNodeType.DiscriminatorOf, clrType, sqlType, sourceExpression) {
			this.obj = obj;
			}
		internal SqlExpression Object {
			get { return this.obj; }
			set { this.obj = value; }
		}
	}
}