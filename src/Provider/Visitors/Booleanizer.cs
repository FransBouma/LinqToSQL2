using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class Booleanizer : SqlBooleanMismatchVisitor
	{
		private NodeFactory sql;

		internal Booleanizer(NodeFactory factory)
		{
			this.sql = factory;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			// DevDiv 179191
			if(@select.Where != null && @select.Where.NodeType == SqlNodeType.Coalesce)
			{
				SqlBinary bin = (SqlBinary)@select.Where;
				if(bin.Right.NodeType == SqlNodeType.Value)
				{
					SqlValue value = (SqlValue)bin.Right;
					if(value.Value != null && value.Value.GetType() == typeof(bool) && (bool)value.Value == false)
					{
						@select.Where = bin.Left;
					}
				}
			}

			return base.VisitSelect(@select);
		}

		internal override SqlExpression ConvertValueToPredicate(SqlExpression valueExpression)
		{
			// Transform the 'Bit' expression into a 'Predicate' by forming the 
			// following operation:
			//    OriginalExpr = 1
			// Yukon and later could also handle:
			//    OriginalExpr = 'true'
			// but Sql2000 does not support this.
			return new SqlBinary(SqlNodeType.EQ,
				valueExpression.ClrType, sql.TypeProvider.From(typeof(bool)),
				valueExpression,
				sql.Value(typeof(bool), valueExpression.SqlType, true, false, valueExpression.SourceExpression)
				);
		}

		internal override SqlExpression ConvertPredicateToValue(SqlExpression predicateExpression)
		{
			// Transform the 'Predicate' expression into a 'Bit' by forming the 
			// following operation:
			//   CASE
			//    WHEN predicateExpression THEN 1
			//    ELSE NOT(predicateExpression) THEN 0
			//    ELSE NULL
			//   END

			// Possible simplification to the generated SQL would be to detect when 'predicateExpression'
			// is SqlUnary(NOT) and use its operand with the literal 1 and 0 below swapped.
			SqlExpression valueTrue = sql.ValueFromObject(true, false, predicateExpression.SourceExpression);
			SqlExpression valueFalse = sql.ValueFromObject(false, false, predicateExpression.SourceExpression);
			if(SqlExpressionNullability.CanBeNull(predicateExpression) != false)
			{
				SqlExpression valueNull = sql.Value(valueTrue.ClrType, valueTrue.SqlType, null, false, predicateExpression.SourceExpression);
				return new SqlSearchedCase(
					predicateExpression.ClrType,
					new SqlWhen[] { 
									  new SqlWhen(predicateExpression, valueTrue),
									  new SqlWhen(new SqlUnary(SqlNodeType.Not, predicateExpression.ClrType, predicateExpression.SqlType, predicateExpression, predicateExpression.SourceExpression), valueFalse)
								  },
					valueNull,
					predicateExpression.SourceExpression
					);
			}
			else
			{
				return new SqlSearchedCase(
					predicateExpression.ClrType,
					new SqlWhen[] { new SqlWhen(predicateExpression, valueTrue) },
					valueFalse,
					predicateExpression.SourceExpression
					);
			}
		}
	}
}