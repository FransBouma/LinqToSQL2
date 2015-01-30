using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlJoin : SqlSource {
		private SqlJoinType joinType;
		private SqlSource left;
		private SqlSource right;
		private SqlExpression condition;

		internal SqlJoin(SqlJoinType type, SqlSource left, SqlSource right, SqlExpression cond, Expression sourceExpression)
			: base(SqlNodeType.Join, sourceExpression) {
			this.JoinType = type;
			this.Left = left;
			this.Right = right;
			this.Condition = cond;
			}

		internal SqlJoinType JoinType {
			get { return this.joinType; }
			set { this.joinType = value; }
		}

		internal SqlSource Left {
			get { return this.left; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.left = value;
			}
		}

		internal SqlSource Right {
			get { return this.right; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				this.right = value;
			}
		}

		internal SqlExpression Condition {
			get { return this.condition; }
			set { this.condition = value; }
		}
	}
}