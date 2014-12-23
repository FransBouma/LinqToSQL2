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

	internal class EntitySetDefValueAccessor<T, V> : MetaAccessor<T, IEnumerable<V>> where V : class
	{
		MetaAccessor<T, EntitySet<V>> acc;
		internal EntitySetDefValueAccessor(MetaAccessor<T, EntitySet<V>> acc)
		{
			this.acc = acc;
		}
		public override IEnumerable<V> GetValue(T instance)
		{
			EntitySet<V> eset = this.acc.GetValue(instance);
			return eset.GetUnderlyingValues();
		}
		public override void SetValue(ref T instance, IEnumerable<V> value)
		{
			EntitySet<V> eset = this.acc.GetValue(instance);
			if(eset == null)
			{
				eset = new EntitySet<V>();
				this.acc.SetValue(ref instance, eset);
			}
			eset.Assign(value);
		}
	}
}

