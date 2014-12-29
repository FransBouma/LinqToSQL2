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

	internal class StandardIdentityManager : IdentityManager
	{
		Dictionary<MetaType, IdentityCache> caches;
		IdentityCache currentCache;
		MetaType currentType;

		internal StandardIdentityManager()
		{
			this.caches = new Dictionary<MetaType, IdentityCache>();
		}

		internal override object InsertLookup(MetaType type, object instance)
		{
			this.SetCurrent(type);
			return this.currentCache.InsertLookup(instance);
		}

		internal override bool RemoveLike(MetaType type, object instance)
		{
			this.SetCurrent(type);
			return this.currentCache.RemoveLike(instance);
		}

		internal override object Find(MetaType type, object[] keyValues)
		{
			this.SetCurrent(type);
			return this.currentCache.Find(keyValues);
		}

		internal override object FindLike(MetaType type, object instance)
		{
			this.SetCurrent(type);
			return this.currentCache.FindLike(instance);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private void SetCurrent(MetaType type)
		{
			type = type.InheritanceRoot;
			if(this.currentType != type)
			{
				if(!this.caches.TryGetValue(type, out this.currentCache))
				{
					KeyManager km = GetKeyManager(type);
					this.currentCache = (IdentityCache)Activator.CreateInstance(
						typeof(IdentityCache<,>).MakeGenericType(type.Type, km.KeyType),
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
						new object[] { km }, null
						);
					this.caches.Add(type, this.currentCache);
				}
				this.currentType = type;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		static KeyManager GetKeyManager(MetaType type)
		{
			int n = type.IdentityMembers.Count;
			MetaDataMember mm = type.IdentityMembers[0];

			KeyManager km = (KeyManager)Activator.CreateInstance(
						typeof(SingleKeyManager<,>).MakeGenericType(type.Type, mm.Type),
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
						new object[] { mm.StorageAccessor, 0 }, null
						);
			for(int i = 1; i < n; i++)
			{
				mm = type.IdentityMembers[i];
				km = (KeyManager)
					Activator.CreateInstance(
						typeof(MultiKeyManager<,,>).MakeGenericType(type.Type, mm.Type, km.KeyType),
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
						new object[] { mm.StorageAccessor, i, km }, null
						);
			}

			return km;
		}
	}
}

