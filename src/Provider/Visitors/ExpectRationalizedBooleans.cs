using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// A validator which enforces rationalized boolean expressions.
	/// </summary>
	internal class ExpectRationalizedBooleans : SqlBooleanMismatchVisitor
	{
		internal override SqlExpression ConvertValueToPredicate(SqlExpression bitExpression)
		{
			throw Error.ExpectedPredicateFoundBit();
		}

		internal override SqlExpression ConvertPredicateToValue(SqlExpression predicateExpression)
		{
			throw Error.ExpectedBitFoundPredicate();
		}
	}
}