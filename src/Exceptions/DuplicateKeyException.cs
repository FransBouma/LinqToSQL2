using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	/// <summary>
	/// An attempt was made to add an object to the identity cache with a key that is already in use
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "Unknown reason.")]
	[SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Unknown reason.")]
	public class DuplicateKeyException : InvalidOperationException
	{
		private object duplicate;
		public DuplicateKeyException(object duplicate)
		{
			this.duplicate = duplicate;
		}
		public DuplicateKeyException(object duplicate, string message)
			: base(message)
		{
			this.duplicate = duplicate;
		}
		public DuplicateKeyException(object duplicate, string message, Exception innerException)
			: base(message, innerException)
		{
			this.duplicate = duplicate;
		}

		/// <summary>
		/// The object whose duplicate key caused the exception.
		/// </summary>
		public object Object
		{
			get
			{
				return duplicate;
			}
		}
	}
}

