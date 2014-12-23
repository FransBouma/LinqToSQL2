using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping
{
	/// <summary>
	/// A MetaAccessor
	/// </summary>
	public abstract class MetaAccessor
	{
		/// <summary>
		/// The type of the member accessed by this accessor.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "The contexts in which this is available are fairly specific.")]
		public abstract Type Type { get; }
		/// <summary>
		/// Gets the value as an object.
		/// </summary>
		/// <param name="instance">The instance to get the value from.</param>
		/// <returns>Value.</returns>
		public abstract object GetBoxedValue(object instance);
		/// <summary>
		/// Sets the value as an object.
		/// </summary>
		/// <param name="instance">The instance to set the value into.</param>
		/// <param name="value">The value to set.</param>
		[SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "[....]: Needs to handle classes and structs.")]
		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Unknown reason.")]
		public abstract void SetBoxedValue(ref object instance, object value);
		/// <summary>
		/// True if the instance has a loaded or assigned value.
		/// </summary>
		public virtual bool HasValue(object instance)
		{
			return true;
		}
		/// <summary>
		/// True if the instance has an assigned value.
		/// </summary>
		public virtual bool HasAssignedValue(object instance)
		{
			return true;
		}
		/// <summary>
		/// True if the instance has a value loaded from a deferred source.
		/// </summary>
		public virtual bool HasLoadedValue(object instance)
		{
			return false;
		}
	}
}

