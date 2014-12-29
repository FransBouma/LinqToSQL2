using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	/// <summary>
	/// Interface providing access to a function return value.
	/// </summary>
	public interface IFunctionResult
	{
		/// <summary>
		/// The value.
		/// </summary>
		object ReturnValue { get; }
	}
}

