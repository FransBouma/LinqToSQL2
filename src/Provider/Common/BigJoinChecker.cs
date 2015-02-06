using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class BigJoinChecker
	{
		#region Private Classes
		private class Visitor : SqlVisitor
		{
			internal bool canBigJoin = true;


			internal override SqlExpression VisitMultiset(SqlSubSelect sms)
			{
				return sms;
			}


			internal override SqlExpression VisitElement(SqlSubSelect elem)
			{
				return elem;
			}


			internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
			{
				return cq;
			}


			internal override SqlExpression VisitExists(SqlSubSelect ss)
			{
				return ss;
			}


			internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss)
			{
				return ss;
			}


			internal override SqlSelect VisitSelect(SqlSelect select)
			{
				// big-joins may need to lift PK's out for default ordering, so don't allow big-join if we see these
				this.canBigJoin &= select.GroupBy.Count == 0 && select.Top == null && !select.IsDistinct;
				if(!this.canBigJoin)
				{
					return select;
				}
				return base.VisitSelect(select);
			}
		}

		#endregion

		internal static bool CanBigJoin(SqlSelect select)
		{
			Visitor v = new Visitor();
			v.Visit(select);
			return v.canBigJoin;
		}

	}
}