using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq {

    /// <summary>
    /// An interface for representing results of mapped functions or queries with variable return sequences.
    /// </summary>
    public interface IMultipleResults : IFunctionResult, IDisposable {
        /// <summary>
        /// Retrieves the next result as a sequence of Type 'TElement'.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
        IEnumerable<TElement> GetResult<TElement>();
    }
}
