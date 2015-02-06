using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Linq;

namespace System.Data.Linq.DbEngines.SqlServer
{
	using System.Data.Linq.Mapping;
	using Linq;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics;
	using System.Data.Linq.Provider.NodeTypes;
	using System.Data.Linq.Provider.Common;


	/// <summary>
	/// Factory class produces SqlNodes. Smarts about type system mappings should go
	/// here and not in the individual SqlNodes.
	/// </summary>
	internal class SqlFactory : NodeFactory
	{
		internal SqlFactory(TypeSystemProvider typeProvider, MetaModel model)
			: base(typeProvider, model)
		{
		}


		/// <summary>
		/// Creates a function call to obtain the length in characters of the element in expr. Non-internal string length.  This should only be used when translating an explicit call by the
		/// user to String.Length.
		/// </summary>
		/// <param name="expr">The expr.</param>
		/// <returns></returns>
		internal override SqlExpression FunctionCallStringLength(SqlExpression expr)
		{
			return FunctionCall(typeof(int), "LEN", new SqlExpression[] { expr }, expr.SourceExpression);
		}


		/// <summary>
		/// Creates a function call to obtain the length in bytes of the element in expr. This represents the SQL DATALENGTH function, which is the raw number of bytes in the argument.  In the
		/// case of string types it will count trailing spaces, but doesn't understand unicode.
		/// </summary>
		/// <param name="expr">The expr.</param>
		/// <returns></returns>
		internal override SqlExpression FunctionCallDataLength(SqlExpression expr)
		{
			return FunctionCall(typeof(int), "DATALENGTH", new SqlExpression[] { expr }, expr.SourceExpression);
		}


		/// <summary>
		/// A unary function that uses DATALENGTH, dividing by two if the string is unicode.  This is the internal
		/// form of String.Length that should always be used.
		/// </summary>
		/// <param name="expr">The expr.</param>
		/// <returns></returns>
		internal override SqlExpression FunctionCallChrLength(SqlExpression expr)
		{
			return Unary(SqlNodeType.ClrLength, expr);
		}


		internal override SqlExpression AddTimeSpan(SqlExpression dateTime, SqlExpression timeSpan, bool asNullable)
		{
			Debug.Assert(this.IsHighPrecisionDateTimeType(timeSpan));

			SqlExpression ns = FunctionCallDatePart("NANOSECOND", timeSpan);
			SqlExpression ms = FunctionCallDatePart("MILLISECOND", timeSpan);
			SqlExpression ss = FunctionCallDatePart("SECOND", timeSpan);
			SqlExpression mi = FunctionCallDatePart("MINUTE", timeSpan);
			SqlExpression hh = FunctionCallDatePart("HOUR", timeSpan);

			SqlExpression result = dateTime;
			if(this.IsHighPrecisionDateTimeType(dateTime))
			{
				result = FunctionCallDateAdd("NANOSECOND", ns, result, dateTime.SourceExpression, asNullable);
			}
			else
			{
				result = FunctionCallDateAdd("MILLISECOND", ms, result, dateTime.SourceExpression, asNullable);
			}
			result = FunctionCallDateAdd("SECOND", ss, result, dateTime.SourceExpression, asNullable);
			result = FunctionCallDateAdd("MINUTE", mi, result, dateTime.SourceExpression, asNullable);
			result = FunctionCallDateAdd("HOUR", hh, result, dateTime.SourceExpression, asNullable);

			if(this.IsDateTimeOffsetType(dateTime))
				return ConvertTo(typeof(DateTimeOffset), result);

			return result;
		}


		/// <summary>
		/// Creates a function call using DATEPART, with the part name specified. 
		/// </summary>
		/// <param name="partName">Name of the part.</param>
		/// <param name="expr">The expr.</param>
		/// <returns></returns>
		internal override SqlExpression FunctionCallDatePart(string partName, SqlExpression expr)
		{
			return FunctionCall(
				typeof(int),
				"DATEPART",
				new SqlExpression[] { 
                    new SqlVariable(typeof(void), null, partName, expr.SourceExpression), 
                    expr 
                },
				expr.SourceExpression
				);
		}


		/// <summary>
		/// Creates a function call using DATEADD, with the part specified and adds the element in value
		/// </summary>
		/// <param name="partName">Name of the part.</param>
		/// <param name="value">The value.</param>
		/// <param name="expr">The expr.</param>
		/// <param name="sourceExpression">The source expression.</param>
		/// <param name="asNullable">if set to <c>true</c> [as nullable].</param>
		/// <returns></returns>
		internal override SqlExpression FunctionCallDateAdd(string partName, SqlExpression value, SqlExpression expr, Expression sourceExpression, bool asNullable)
		{
			Type returnType = asNullable ? typeof(DateTime?) : typeof(DateTime);

			return FunctionCall(
				returnType,
				"DATEADD",
				new SqlExpression[] {
                    new SqlVariable(typeof(void), null, partName, sourceExpression),
                    value,
                    expr },
				sourceExpression
				);
		}


		/// <summary>
		/// Creates a function call using DATETIMEOFFSETADD, with the part specified, and adds the element in value.
		/// </summary>
		/// <param name="partName">Name of the part.</param>
		/// <param name="value">The value.</param>
		/// <param name="expr">The expr.</param>
		/// <param name="sourceExpression">The source expression.</param>
		/// <param name="asNullable">if set to <c>true</c> [as nullable].</param>
		/// <returns></returns>
		internal override SqlExpression FunctionCallDateTimeOffsetAdd(string partName, SqlExpression value, SqlExpression expr, Expression sourceExpression, bool asNullable)
		{
			Type returnType = asNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);

			return FunctionCall(
				returnType,
				"DATEADD",
				new SqlExpression[] {
                    new SqlVariable(typeof(void), null, partName, sourceExpression),
                    value,
                    expr },
				sourceExpression
				);
		}


		/// <summary>
		/// Determines whether the type represented by the element in exp is a DateTime type
		/// </summary>
		/// <param name="exp">The exp.</param>
		/// <returns></returns>
		internal override bool IsDateTimeType(SqlExpression exp)
		{
			SqlDbType sqlDbType = ((SqlType)(exp.SqlType)).SqlDbType;
			return (sqlDbType == SqlDbType.DateTime || sqlDbType == SqlDbType.SmallDateTime);
		}


		/// <summary>
		/// Determines whether the type represented by the element in exp is a Date type
		/// </summary>
		/// <param name="exp">The exp.</param>
		/// <returns></returns>
		internal override bool IsDateType(SqlExpression exp)
		{
			return (((SqlType)(exp.SqlType)).SqlDbType == SqlDbType.Date);
		}


		/// <summary>
		/// Determines whether the type represented by the element in exp is a Time type
		/// </summary>
		/// <param name="exp">The exp.</param>
		/// <returns></returns>
		internal override bool IsTimeType(SqlExpression exp)
		{
			return (((SqlType)(exp.SqlType)).SqlDbType == SqlDbType.Time);
		}


		/// <summary>
		/// Determines whether the type represented by the element in exp is a DateTimeOffset type
		/// </summary>
		/// <param name="exp">The exp.</param>
		/// <returns></returns>
		internal override bool IsDateTimeOffsetType(SqlExpression exp)
		{
			return (((SqlType)(exp.SqlType)).SqlDbType == SqlDbType.DateTimeOffset);
		}


		/// <summary>
		/// Determines whether the type represented by the element in exp is a high precision DateTime type
		/// </summary>
		/// <param name="exp">The exp.</param>
		/// <returns></returns>
		internal override bool IsHighPrecisionDateTimeType(SqlExpression exp)
		{
			SqlDbType sqlDbType = ((SqlType)(exp.SqlType)).SqlDbType;
			return (sqlDbType == SqlDbType.Time || sqlDbType == SqlDbType.DateTime2 || sqlDbType == SqlDbType.DateTimeOffset);
		}
	}
}
