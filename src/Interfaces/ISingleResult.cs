using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	/// <summary>
	/// An interface for representing the result of a mapped function with a single return sequence.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "[....]: Meant to represent a database table which is delayed loaded and doesn't provide collection semantics.")]
	public interface ISingleResult<T> : IEnumerable<T>, IFunctionResult, IDisposable { }
}

