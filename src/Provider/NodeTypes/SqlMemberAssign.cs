using System.Reflection;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlMemberAssign : SqlNode {
		private MemberInfo member;
		private SqlExpression expression;

		internal SqlMemberAssign(MemberInfo member, SqlExpression expr)
			: base(SqlNodeType.MemberAssign, expr.SourceExpression) {
			if (member == null)
				throw Error.ArgumentNull("member");
			this.member = member;
			this.Expression = expr;
			}

		internal MemberInfo Member {
			get { return this.member; }
		}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.expression = value;
			}
		}
	}
}