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

	internal class LinkDefSourceAccessor<T, V> : MetaAccessor<T, IEnumerable<V>>
	{
		MetaAccessor<T, Link<V>> acc;
		internal LinkDefSourceAccessor(MetaAccessor<T, Link<V>> acc)
		{
			this.acc = acc;
		}
		public override IEnumerable<V> GetValue(T instance)
		{
			Link<V> link = this.acc.GetValue(instance);
			return (IEnumerable<V>)link.Source;
		}
		public override void SetValue(ref T instance, IEnumerable<V> value)
		{
			Link<V> link = this.acc.GetValue(instance);
			if(link.HasAssignedValue || link.HasLoadedValue)
			{
				throw Error.LinkAlreadyLoaded();
			}
			this.acc.SetValue(ref instance, new Link<V>(value));
		}
	}
}

