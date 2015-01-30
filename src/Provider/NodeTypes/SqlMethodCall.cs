using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlMethodCall : SqlSimpleTypeExpression {
		private MethodInfo method;
		private SqlExpression obj;
		private List<SqlExpression> arguments;

		internal SqlMethodCall(Type clrType, ProviderType sqlType, MethodInfo method, SqlExpression obj, IEnumerable<SqlExpression> args, Expression sourceExpression)
			: base(SqlNodeType.MethodCall, clrType, sqlType, sourceExpression) {
			if (method == null)
				throw Error.ArgumentNull("method");
			this.method = method;
			this.Object = obj;
			this.arguments = new List<SqlExpression>();
			if (args != null)
				this.arguments.AddRange(args);
			}

		internal MethodInfo Method {
			get { return this.method; }
		}

		internal SqlExpression Object {
			get { return this.obj; }
			set {
				if (value == null && !this.method.IsStatic)
					throw Error.ArgumentNull("value");
				if (value != null && !this.method.DeclaringType.IsAssignableFrom(value.ClrType))
					throw Error.ArgumentWrongType("value", this.method.DeclaringType, value.ClrType);
				this.obj = value;
			}
		}

		internal List<SqlExpression> Arguments {
			get { return this.arguments; }
		}
	}
}