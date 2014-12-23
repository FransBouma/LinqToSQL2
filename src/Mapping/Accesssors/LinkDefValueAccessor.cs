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
	using System.Data.Linq.Provider;
	using System.Diagnostics.CodeAnalysis;

	internal class LinkDefValueAccessor<T, V> : MetaAccessor<T, V>
	{
		MetaAccessor<T, Link<V>> acc;
		internal LinkDefValueAccessor(MetaAccessor<T, Link<V>> acc)
		{
			this.acc = acc;
		}
		public override V GetValue(T instance)
		{
			Link<V> link = this.acc.GetValue(instance);
			return link.UnderlyingValue;
		}
		public override void SetValue(ref T instance, V value)
		{
			this.acc.SetValue(ref instance, new Link<V>(value));
		}
	}
}

