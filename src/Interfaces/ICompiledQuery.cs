using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Common;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	using Linq;

	/// <summary>
	/// A compiled query.
	/// </summary>
	internal interface ICompiledQuery
	{
		/// <summary>
		/// Executes the compiled query using the specified provider and a set of arguments.
		/// </summary>
		/// <param name="provider">The provider that will execute the compiled query.</param>
		/// <param name="arguments">Argument values to supply to the parameters of the compiled query, 
		/// when the query is specified as a LambdaExpression.</param>
		/// <returns></returns>
		IExecuteResult Execute(IProvider provider, object[] arguments);
	}
}

