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

	internal class EntityRefValueAccessor<T, V> : MetaAccessor<T, V> where V : class
	{
		MetaAccessor<T, EntityRef<V>> acc;
		internal EntityRefValueAccessor(MetaAccessor<T, EntityRef<V>> acc)
		{
			this.acc = acc;
		}
		public override V GetValue(T instance)
		{
			EntityRef<V> er = this.acc.GetValue(instance);
			return er.Entity;
		}
		public override void SetValue(ref T instance, V value)
		{
			this.acc.SetValue(ref instance, new EntityRef<V>(value));
		}
		public override bool HasValue(object instance)
		{
			EntityRef<V> er = this.acc.GetValue((T)instance);
			return er.HasValue;
		}
		public override bool HasAssignedValue(object instance)
		{
			EntityRef<V> er = this.acc.GetValue((T)instance);
			return er.HasAssignedValue;
		}
		public override bool HasLoadedValue(object instance)
		{
			EntityRef<V> er = this.acc.GetValue((T)instance);
			return er.HasLoadedValue;
		}
	}
}

