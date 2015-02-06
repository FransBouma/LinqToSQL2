using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SqlColumnEqualizer : SqlVisitor
	{
		#region Member Declarations
		private Dictionary<SqlColumn, SqlColumn> _map;
		#endregion

		internal void BuildEqivalenceMap(SqlSource scope)
		{
			this._map = new Dictionary<SqlColumn, SqlColumn>();
			this.Visit(scope);
		}

		internal bool AreEquivalent(SqlExpression e1, SqlExpression e2)
		{
			if(SqlComparer.AreEqual(e1, e2))
				return true;

			SqlColumnRef cr1 = e1 as SqlColumnRef;
			SqlColumnRef cr2 = e2 as SqlColumnRef;

			if(cr1 != null && cr2 != null)
			{
				SqlColumn c1 = cr1.GetRootColumn();
				SqlColumn c2 = cr2.GetRootColumn();
				SqlColumn r;
				return this._map.TryGetValue(c1, out r) && r == c2;
			}

			return false;
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			base.VisitJoin(@join);
			if(@join.Condition != null)
			{
				this.CheckJoinCondition(@join.Condition);
			}
			return @join;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			base.VisitSelect(@select);
			if(@select.Where != null)
			{
				this.CheckJoinCondition(@select.Where);
			}
			return @select;
		}

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "[....]: Cast is dependent on node type and casts do not happen unecessarily in a single code path.")]
		private void CheckJoinCondition(SqlExpression expr)
		{
			switch(expr.NodeType)
			{
				case SqlNodeType.And:
				{
					SqlBinary b = (SqlBinary)expr;
					CheckJoinCondition(b.Left);
					CheckJoinCondition(b.Right);
					break;
				}
				case SqlNodeType.EQ:
				case SqlNodeType.EQ2V:
				{
					SqlBinary b = (SqlBinary)expr;
					SqlColumnRef crLeft = b.Left as SqlColumnRef;
					SqlColumnRef crRight = b.Right as SqlColumnRef;
					if(crLeft != null && crRight != null)
					{
						SqlColumn cLeft = crLeft.GetRootColumn();
						SqlColumn cRight = crRight.GetRootColumn();
						this._map[cLeft] = cRight;
						this._map[cRight] = cLeft;
					}
					break;
				}
			}
		}

		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			return ss;
		}
	}
}