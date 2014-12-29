using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	/// <summary>
	/// The result of executing a query.
	/// </summary>
	public interface IExecuteResult : IDisposable
	{
		/// <summary>
		/// The return value or result of the executed query. This object has the same type as the
		/// query expression's Type property.
		/// </summary>
		object ReturnValue { get; }

		/// <summary>
		/// Retrieves the nth output parameter.  This method is normally used when the query is a mapped
		/// function with output parameters.
		/// </summary>
		/// <param name="parameterIndex"></param>
		/// <returns></returns>
		object GetParameterValue(int parameterIndex);
	}
}

