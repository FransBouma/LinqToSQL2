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
	/// A strongly-typed MetaAccessor. Used for reading from and writing to
	/// CLR objects.
	/// </summary>
	/// <typeparam name="T">The type of the object</typeparam>
	/// <typeparam name="V">The type of the accessed member</typeparam>
	public abstract class MetaAccessor<TEntity, TMember> : MetaAccessor
	{
		/// <summary>
		/// The underlying CLR type.
		/// </summary>
		public override Type Type
		{
			get { return typeof(TMember); }
		}
		/// <summary>
		/// Set the boxed value on an instance.
		/// </summary>
		public override void SetBoxedValue(ref object instance, object value)
		{
			TEntity tInst = (TEntity)instance;
			this.SetValue(ref tInst, (TMember)value);
			instance = tInst;
		}
		/// <summary>
		/// Retrieve the boxed value.
		/// </summary>
		public override object GetBoxedValue(object instance)
		{
			return this.GetValue((TEntity)instance);
		}
		/// <summary>
		/// Gets the strongly-typed value.
		/// </summary>
		public abstract TMember GetValue(TEntity instance);
		/// <summary>
		/// Sets the strongly-typed value
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Unknown reason.")]
		public abstract void SetValue(ref TEntity instance, TMember value);
	}
}

