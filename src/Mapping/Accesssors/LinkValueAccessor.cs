using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Security;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	using Linq;
	using System.Diagnostics.CodeAnalysis;

	// deferred type accessors 
	internal class LinkValueAccessor<T, V> : MetaAccessor<T, V>
	{
		MetaAccessor<T, Link<V>> acc;
		internal LinkValueAccessor(MetaAccessor<T, Link<V>> acc)
		{
			this.acc = acc;
		}
		public override bool HasValue(object instance)
		{
			Link<V> link = this.acc.GetValue((T)instance);
			return link.HasValue;
		}
		public override bool HasAssignedValue(object instance)
		{
			Link<V> link = this.acc.GetValue((T)instance);
			return link.HasAssignedValue;
		}
		public override bool HasLoadedValue(object instance)
		{
			Link<V> link = this.acc.GetValue((T)instance);
			return link.HasLoadedValue;
		}
		public override V GetValue(T instance)
		{
			Link<V> link = this.acc.GetValue(instance);
			return link.Value;
		}
		public override void SetValue(ref T instance, V value)
		{
			this.acc.SetValue(ref instance, new Link<V>(value));
		}
	}
}

