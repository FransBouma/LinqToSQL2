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

	internal class EntityRefDefValueAccessor<T, V> : MetaAccessor<T, V> where V : class
	{
		MetaAccessor<T, EntityRef<V>> acc;
		internal EntityRefDefValueAccessor(MetaAccessor<T, EntityRef<V>> acc)
		{
			this.acc = acc;
		}
		public override V GetValue(T instance)
		{
			EntityRef<V> er = this.acc.GetValue(instance);
			return er.UnderlyingValue;
		}
		public override void SetValue(ref T instance, V value)
		{
			this.acc.SetValue(ref instance, new EntityRef<V>(value));
		}
	}
}

