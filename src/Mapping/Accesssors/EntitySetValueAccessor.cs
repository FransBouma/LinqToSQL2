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

	internal class EntitySetValueAccessor<T, V> : MetaAccessor<T, EntitySet<V>> where V : class
	{
		MetaAccessor<T, EntitySet<V>> acc;
		internal EntitySetValueAccessor(MetaAccessor<T, EntitySet<V>> acc)
		{
			this.acc = acc;
		}
		public override EntitySet<V> GetValue(T instance)
		{
			return this.acc.GetValue(instance);
		}
		public override void SetValue(ref T instance, EntitySet<V> value)
		{
			EntitySet<V> eset = this.acc.GetValue(instance);
			if(eset == null)
			{
				eset = new EntitySet<V>();
				this.acc.SetValue(ref instance, eset);
			}
			eset.Assign(value);
		}
		public override bool HasValue(object instance)
		{
			EntitySet<V> es = this.acc.GetValue((T)instance);
			return es != null && es.HasValues;
		}
		public override bool HasAssignedValue(object instance)
		{
			EntitySet<V> es = this.acc.GetValue((T)instance);
			return es != null && es.HasAssignedValues;
		}
		public override bool HasLoadedValue(object instance)
		{
			EntitySet<V> es = this.acc.GetValue((T)instance);
			return es != null && es.HasLoadedValues;
		}
	}
}

