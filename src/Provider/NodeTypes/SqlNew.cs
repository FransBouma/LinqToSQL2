using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlNew : SqlSimpleTypeExpression {
		private MetaType metaType;
		private ConstructorInfo constructor;
		private List<SqlExpression> args;
		private List<MemberInfo> argMembers;
		private List<SqlMemberAssign> members;

		internal SqlNew(MetaType metaType, ProviderType sqlType, ConstructorInfo cons, IEnumerable<SqlExpression> args, IEnumerable<MemberInfo> argMembers, IEnumerable<SqlMemberAssign> members, Expression sourceExpression)
			: base(SqlNodeType.New, metaType.Type, sqlType, sourceExpression) {
			this.metaType = metaType;
            
			if (cons == null && metaType.Type.IsClass) { // structs do not need to have a constructor
				throw Error.ArgumentNull("cons");
			}
			this.constructor = cons;
			this.args = new List<SqlExpression>();
			this.argMembers = new List<MemberInfo>();
			this.members = new List<SqlMemberAssign>();
			if (args != null) {
				this.args.AddRange(args);
			}
			if (argMembers != null) {
				this.argMembers.AddRange(argMembers);
			}
			if (members != null) {
				this.members.AddRange(members);
			}
			}

		internal MetaType MetaType {
			get { return this.metaType; }
		}

		internal ConstructorInfo Constructor {
			get { return this.constructor; }
		}

		internal List<SqlExpression> Args {
			get { return this.args; }
		}

		internal List<MemberInfo> ArgMembers {
			get { return this.argMembers; }
		}

		internal List<SqlMemberAssign> Members {
			get { return this.members; }
		}

		internal SqlExpression Find(MemberInfo mi) {
			for (int i = 0, n = this.argMembers.Count; i < n; i++) {
				MemberInfo argmi = this.argMembers[i];
				if (argmi.Name == mi.Name) {
					return this.args[i];
				}
			}

			foreach (SqlMemberAssign ma in this.Members) {
				if (ma.Member.Name == mi.Name) {
					return ma.Expression;
				}
			}

			return null;
		}
	}
}