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

	internal static class PropertyAccessor
	{

		internal static MetaAccessor Create(Type objectType, PropertyInfo pi, MetaAccessor storageAccessor)
		{
			Delegate dset = null;
			Delegate drset = null;
			Type dgetType = typeof(DGet<,>).MakeGenericType(objectType, pi.PropertyType);
			MethodInfo getMethod = pi.GetGetMethod(true);

			Delegate dget = Delegate.CreateDelegate(dgetType, getMethod, true);
			if(dget == null)
			{
				throw Error.CouldNotCreateAccessorToProperty(objectType, pi.PropertyType, pi);
			}

			if(pi.CanWrite)
			{
				if(!objectType.IsValueType)
				{
					dset = Delegate.CreateDelegate(typeof(DSet<,>).MakeGenericType(objectType, pi.PropertyType), pi.GetSetMethod(true), true);
				}
				else
				{
					DynamicMethod mset = new DynamicMethod(
						"xset_" + pi.Name,
						typeof(void),
						new Type[] { objectType.MakeByRefType(), pi.PropertyType },
						true
						);
					ILGenerator gen = mset.GetILGenerator();
					gen.Emit(OpCodes.Ldarg_0);
					if(!objectType.IsValueType)
					{
						gen.Emit(OpCodes.Ldind_Ref);
					}
					gen.Emit(OpCodes.Ldarg_1);
					gen.Emit(OpCodes.Call, pi.GetSetMethod(true));
					gen.Emit(OpCodes.Ret);
					drset = mset.CreateDelegate(typeof(DRSet<,>).MakeGenericType(objectType, pi.PropertyType));
				}
			}

			Type saType = (storageAccessor != null) ? storageAccessor.Type : pi.PropertyType;
			return (MetaAccessor)Activator.CreateInstance(
				typeof(Accessor<,,>).MakeGenericType(objectType, pi.PropertyType, saType),
				BindingFlags.Instance | BindingFlags.NonPublic, null,
				new object[] { pi, dget, dset, drset, storageAccessor }, null
				);
		}


		class Accessor<T, V, V2> : MetaAccessor<T, V> where V2 : V
		{
			PropertyInfo pi;
			DGet<T, V> dget;
			DSet<T, V> dset;
			DRSet<T, V> drset;
			MetaAccessor<T, V2> storage;

			internal Accessor(PropertyInfo pi, DGet<T, V> dget, DSet<T, V> dset, DRSet<T, V> drset, MetaAccessor<T, V2> storage)
			{
				this.pi = pi;
				this.dget = dget;
				this.dset = dset;
				this.drset = drset;
				this.storage = storage;
			}
			public override V GetValue(T instance)
			{
				return this.dget(instance);
			}
			public override void SetValue(ref T instance, V value)
			{
				if(this.dset != null)
				{
					this.dset(instance, value);
				}
				else if(this.drset != null)
				{
					this.drset(ref instance, value);
				}
				else if(this.storage != null)
				{
					this.storage.SetValue(ref instance, (V2)value);
				}
				else
				{
					throw Error.UnableToAssignValueToReadonlyProperty(this.pi);
				}
			}
		}
	}
}

