using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	/// <summary>
	/// An attempt was made to change an FK but the Entity is Loaded
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "Unknown reason.")]
	[SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Unknown reason.")]
	public class ForeignKeyReferenceAlreadyHasValueException : InvalidOperationException
	{
		public ForeignKeyReferenceAlreadyHasValueException() { }
		public ForeignKeyReferenceAlreadyHasValueException(string message) : base(message) { }
		public ForeignKeyReferenceAlreadyHasValueException(string message, Exception innerException) : base(message, innerException) { }
	}
}

