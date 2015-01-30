using System.Data.Linq.Provider.Common;
using System.Reflection;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlMember : SqlSimpleTypeExpression {
		private SqlExpression expression;
		private MemberInfo member;

		internal SqlMember(Type clrType, ProviderType sqlType, SqlExpression expr, MemberInfo member)
			: base(SqlNodeType.Member, clrType, sqlType, expr.SourceExpression) {
			this.member = member;
			this.Expression = expr;
			}

		internal MemberInfo Member {
			get { return this.member; }
		}

		internal SqlExpression Expression {
			get {
				return this.expression;
			}
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (!this.member.ReflectedType.IsAssignableFrom(value.ClrType) &&
					!value.ClrType.IsAssignableFrom(this.member.ReflectedType))
					throw Error.MemberAccessIllegal(this.member, this.member.ReflectedType, value.ClrType);
				this.expression = value;
			}
		}
	}
}