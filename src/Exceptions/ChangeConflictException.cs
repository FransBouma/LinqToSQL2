using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq {
    /// <summary>
    /// DLinq-specific custom exception factory.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "Unknown reason.")]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Unknown reason.")]
    public class ChangeConflictException : Exception {
        public ChangeConflictException() { }
        public ChangeConflictException(string message) : base(message) { }
        public ChangeConflictException(string message, Exception innerException) : base(message, innerException) { }
    }
}
