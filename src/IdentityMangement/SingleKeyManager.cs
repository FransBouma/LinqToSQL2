using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using Linq;

	internal class SingleKeyManager<T, V> : KeyManager<T, V>
	{
		bool isKeyNullAssignable;
		MetaAccessor<T, V> accessor;
		int offset;
		IEqualityComparer<V> comparer;

		internal SingleKeyManager(MetaAccessor<T, V> accessor, int offset)
		{
			this.accessor = accessor;
			this.offset = offset;
			this.isKeyNullAssignable = System.Data.Linq.SqlClient.TypeSystem.IsNullAssignable(typeof(V));
		}

		internal override V CreateKeyFromInstance(T instance)
		{
			return this.accessor.GetValue(instance);
		}

		internal override bool TryCreateKeyFromValues(object[] values, out V v)
		{
			object o = values[this.offset];
			if(o == null && !this.isKeyNullAssignable)
			{
				v = default(V);
				return false;
			}
			v = (V)o;
			return true;
		}

		internal override Type KeyType
		{
			get { return typeof(V); }
		}

		internal override IEqualityComparer<V> Comparer
		{
			get
			{
				if(this.comparer == null)
				{
					this.comparer = EqualityComparer<V>.Default;
				}
				return this.comparer;
			}
		}
	}
}

