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
	using System.Data.Linq.Provider;

	internal class MultiKeyManager<T, V1, V2> : KeyManager<T, MultiKey<V1, V2>>
	{
		MetaAccessor<T, V1> accessor;
		int offset;
		KeyManager<T, V2> next;
		IEqualityComparer<MultiKey<V1, V2>> comparer;

		internal MultiKeyManager(MetaAccessor<T, V1> accessor, int offset, KeyManager<T, V2> next)
		{
			this.accessor = accessor;
			this.next = next;
			this.offset = offset;
		}

		internal override MultiKey<V1, V2> CreateKeyFromInstance(T instance)
		{
			return new MultiKey<V1, V2>(
				this.accessor.GetValue(instance),
				this.next.CreateKeyFromInstance(instance)
				);
		}

		internal override bool TryCreateKeyFromValues(object[] values, out MultiKey<V1, V2> k)
		{
			System.Diagnostics.Debug.Assert(this.offset < values.Length, "offset is outside the bounds of the values array");

			object o = values[this.offset];
			if(o == null && typeof(V1).IsValueType)
			{
				k = default(MultiKey<V1, V2>);
				return false;
			}
			V2 v2;
			if(!this.next.TryCreateKeyFromValues(values, out v2))
			{
				k = default(MultiKey<V1, V2>);
				return false;
			}
			k = new MultiKey<V1, V2>((V1)o, v2);
			return true;
		}

		internal override Type KeyType
		{
			get { return typeof(MultiKey<V1, V2>); }
		}

		internal override IEqualityComparer<MultiKey<V1, V2>> Comparer
		{
			get
			{
				if(this.comparer == null)
				{
					this.comparer = new MultiKey<V1, V2>.Comparer(EqualityComparer<V1>.Default, next.Comparer);
				}
				return this.comparer;
			}
		}
	}
}

