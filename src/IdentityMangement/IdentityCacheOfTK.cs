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

	internal class IdentityCache<T, K> : IdentityCache
	{
		int[] buckets;
		Slot[] slots;
		int count;
		int freeList;
		KeyManager<T, K> keyManager;
		IEqualityComparer<K> comparer;

		#region Private classes
		internal struct Slot
		{
			internal int hashCode;
			internal K key;
			internal T value;
			internal int next;
		}
		#endregion


		public IdentityCache(KeyManager<T, K> keyManager)
		{
			this.keyManager = keyManager;
			this.comparer = keyManager.Comparer;
			buckets = new int[7];
			slots = new Slot[7];
			freeList = -1;
		}

		internal override object InsertLookup(object instance)
		{
			T value = (T)instance;
			K key = this.keyManager.CreateKeyFromInstance(value);
			Find(key, ref value, true);
			return value;
		}

		internal override bool RemoveLike(object instance)
		{
			T value = (T)instance;
			K key = this.keyManager.CreateKeyFromInstance(value);

			int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
			int bucket = hashCode % buckets.Length;
			int last = -1;
			for(int i = buckets[bucket] - 1; i >= 0; last = i, i = slots[i].next)
			{
				if(slots[i].hashCode == hashCode && comparer.Equals(slots[i].key, key))
				{
					if(last < 0)
					{
						buckets[bucket] = slots[i].next + 1;
					}
					else
					{
						slots[last].next = slots[i].next;
					}
					slots[i].hashCode = -1;
					slots[i].value = default(T);
					slots[i].next = freeList;
					freeList = i;
					return true;
				}
			}
			return false;
		}

		internal override object Find(object[] keyValues)
		{
			K key;
			if(this.keyManager.TryCreateKeyFromValues(keyValues, out key))
			{
				T value = default(T);
				if(Find(key, ref value, false))
					return value;
			}
			return null;
		}

		internal override object FindLike(object instance)
		{
			T value = (T)instance;
			K key = this.keyManager.CreateKeyFromInstance(value);
			if(Find(key, ref value, false))
				return value;
			return null;
		}

		bool Find(K key, ref T value, bool add)
		{
			int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
			for(int i = buckets[hashCode % buckets.Length] - 1; i >= 0; i = slots[i].next)
			{
				if(slots[i].hashCode == hashCode && comparer.Equals(slots[i].key, key))
				{
					value = slots[i].value;
					return true;
				}
			}
			if(add)
			{
				int index;
				if(freeList >= 0)
				{
					index = freeList;
					freeList = slots[index].next;
				}
				else
				{
					if(count == slots.Length) Resize();
					index = count;
					count++;
				}
				int bucket = hashCode % buckets.Length;
				slots[index].hashCode = hashCode;
				slots[index].key = key;
				slots[index].value = value;
				slots[index].next = buckets[bucket] - 1;
				buckets[bucket] = index + 1;
			}
			return false;
		}

		void Resize()
		{
			int newSize = checked(count * 2 + 1);
			int[] newBuckets = new int[newSize];
			Slot[] newSlots = new Slot[newSize];
			Array.Copy(slots, 0, newSlots, 0, count);
			for(int i = 0; i < count; i++)
			{
				int bucket = newSlots[i].hashCode % newSize;
				newSlots[i].next = newBuckets[bucket] - 1;
				newBuckets[bucket] = i + 1;
			}
			buckets = newBuckets;
			slots = newSlots;
		}
	}
}

