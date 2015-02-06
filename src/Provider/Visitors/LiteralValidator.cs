using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// A validator that ensures literal values are reasonable.
	/// </summary>
	internal class LiteralValidator : SqlVisitor
	{
		internal override SqlExpression VisitValue(SqlValue value)
		{
			if(!value.IsClientSpecified
				&& value.ClrType.IsClass
				&& value.ClrType != typeof(string)
				&& value.ClrType != typeof(Type)
				&& value.Value != null)
			{
				throw Error.ClassLiteralsNotAllowed(value.ClrType);
			}
			return value;
		}

		internal override SqlExpression VisitBinaryOperator(SqlBinary bo)
		{
			bo.Left = this.VisitExpression(bo.Left);
			return bo;
		}
	}
}