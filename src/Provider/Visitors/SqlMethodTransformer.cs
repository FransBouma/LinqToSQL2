using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{

	/// <summary>
	/// After retyping and conversions take place, some functions need to be changed into more suitable calls.
	/// Example: LEN -> DATALENGTH for long text types.
	/// </summary>
	internal class SqlMethodTransformer : SqlVisitor
	{
		protected NodeFactory sql;

		internal SqlMethodTransformer(NodeFactory sql)
		{
			this.sql = sql;
		}

		internal override SqlExpression VisitFunctionCall(SqlFunctionCall fc)
		{
			// process the arguments
			SqlExpression result = base.VisitFunctionCall(fc);
			if(result is SqlFunctionCall)
			{
				SqlFunctionCall resultFunctionCall = (SqlFunctionCall)result;

				if(resultFunctionCall.Name == sql.LengthFunctionName)
				{
					SqlExpression expr = resultFunctionCall.Arguments[0];

					if(expr.SqlType.IsLargeType && !expr.SqlType.SupportsLength)
					{
						result = sql.FunctionCallDataLength(expr);

						if(expr.SqlType.IsUnicodeType)
						{
							result = sql.ConvertToInt(sql.Divide(result, sql.ValueFromObject(2, expr.SourceExpression)));
						}
					}
				}

				// If the return type of the sql function is not compatible with
				// the expected CLR type of the function, inject a conversion. This
				// step must be performed AFTER SqlRetyper has run.
				Type clrType = resultFunctionCall.SqlType.GetClosestRuntimeType();
				bool skipConversion = SkipConversionForDateAdd(resultFunctionCall.Name,
																					resultFunctionCall.ClrType,
																					clrType);
				if((resultFunctionCall.ClrType != clrType) && !skipConversion)
				{
					result = sql.ConvertTo(resultFunctionCall.ClrType, resultFunctionCall);
				}
			}

			return result;
		}

		internal override SqlExpression VisitUnaryOperator(SqlUnary fc)
		{
			// process the arguments
			SqlExpression result = base.VisitUnaryOperator(fc);
			if(result is SqlUnary)
			{
				SqlUnary unary = (SqlUnary)result;

				switch(unary.NodeType)
				{
					case SqlNodeType.ClrLength:
						SqlExpression expr = unary.Operand;

						result = sql.FunctionCallDataLength(expr);

						if(expr.SqlType.IsUnicodeType)
						{
							result = sql.Divide(result, sql.ValueFromObject(2, expr.SourceExpression));
						}

						result = sql.ConvertToInt(result);
						break;
					default:
						break;
				}
			}

			return result;
		}

		// We don't inject a conversion for DATEADD if doing so will downgrade the result to
		// a less precise type.
		//
		private bool SkipConversionForDateAdd(string functionName, Type expected, Type actual)
		{
			if(string.Compare(functionName, this.sql.DataAddFunctionName, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return false;
			}
			return (expected == typeof(DateTime) && actual == typeof(DateTimeOffset));
		}
	}
}
