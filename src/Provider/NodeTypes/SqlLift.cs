using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlLift : SqlExpression {
		internal SqlExpression liftedExpression;

		internal SqlLift(Type type, SqlExpression liftedExpression, Expression sourceExpression)
			: base(SqlNodeType.Lift, type, sourceExpression) {
			if (liftedExpression == null)
				throw Error.ArgumentNull("liftedExpression");
			this.liftedExpression = liftedExpression;
			}

		internal SqlExpression Expression {
			get { return this.liftedExpression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.liftedExpression = value;
			}
		}

		internal override ProviderType SqlType {
			get { return this.liftedExpression.SqlType; }
		}
	}
}