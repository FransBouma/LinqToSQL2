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

	internal class EntityRefDefSourceAccessor<T, V> : MetaAccessor<T, IEnumerable<V>> where V : class
	{
		MetaAccessor<T, EntityRef<V>> acc;
		internal EntityRefDefSourceAccessor(MetaAccessor<T, EntityRef<V>> acc)
		{
			this.acc = acc;
		}
		public override IEnumerable<V> GetValue(T instance)
		{
			EntityRef<V> er = this.acc.GetValue(instance);
			return (IEnumerable<V>)er.Source;
		}
		public override void SetValue(ref T instance, IEnumerable<V> value)
		{
			EntityRef<V> er = this.acc.GetValue(instance);
			if(er.HasAssignedValue || er.HasLoadedValue)
			{
				throw Error.EntityRefAlreadyLoaded();
			}
			this.acc.SetValue(ref instance, new EntityRef<V>(value));
		}
	}
}

