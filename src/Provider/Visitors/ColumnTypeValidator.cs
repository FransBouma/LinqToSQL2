using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Column ClrType must agree with the expression that it points to.
	/// </summary>
	internal class ColumnTypeValidator : SqlVisitor
	{

		internal override SqlRow VisitRow(SqlRow row)
		{
			for(int i = 0, n = row.Columns.Count; i < n; i++)
			{
				SqlColumn col = row.Columns[i];
				SqlExpression expr = this.VisitExpression(col.Expression);
				if(expr != null)
				{
					if(TypeSystem.GetNonNullableType(col.ClrType) != TypeSystem.GetNonNullableType(expr.ClrType))
					{
						throw Error.ColumnClrTypeDoesNotAgreeWithExpressionsClrType();
					}
				}
			}
			return row;
		}
	}
}