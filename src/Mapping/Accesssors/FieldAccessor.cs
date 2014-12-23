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

	internal static class FieldAccessor
	{
		#region Private classes
		/// <summary>
		/// Simple helper class which is used in the Create method to produce generically typed meta accessors. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="V"></typeparam>
		private class Accessor<T, V> : MetaAccessor<T, V>
		{
			DGet<T, V> dget;
			DRSet<T, V> drset;
			FieldInfo fi;

			internal Accessor(FieldInfo fi, DGet<T, V> dget, DRSet<T, V> drset)
			{
				this.fi = fi;
				this.dget = dget;
				this.drset = drset;
			}
			public override V GetValue(T instance)
			{
				if(this.dget != null)
					return this.dget(instance);
				return (V)fi.GetValue(instance);
			}
			public override void SetValue(ref T instance, V value)
			{
				if(this.drset != null)
					this.drset(ref instance, value);
				else
					this.fi.SetValue(instance, value);
			}
		}
		#endregion

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal static MetaAccessor Create(Type objectType, FieldInfo fi)
		{

			if(!fi.ReflectedType.IsAssignableFrom(objectType))
			{
				throw Error.InvalidFieldInfo(objectType, fi.FieldType, fi);
			}
			Delegate dget = null;
			Delegate drset = null;

			if(!objectType.IsGenericType)
			{
				DynamicMethod mget = new DynamicMethod(
					"xget_" + fi.Name,
					fi.FieldType,
					new Type[] { objectType },
					true
					);
				ILGenerator gen = mget.GetILGenerator();
				gen.Emit(OpCodes.Ldarg_0);
				gen.Emit(OpCodes.Ldfld, fi);
				gen.Emit(OpCodes.Ret);
				dget = mget.CreateDelegate(typeof(DGet<,>).MakeGenericType(objectType, fi.FieldType));

				DynamicMethod mset = new DynamicMethod(
					"xset_" + fi.Name,
					typeof(void),
					new Type[] { objectType.MakeByRefType(), fi.FieldType },
					true
					);
				gen = mset.GetILGenerator();
				gen.Emit(OpCodes.Ldarg_0);
				if(!objectType.IsValueType)
				{
					gen.Emit(OpCodes.Ldind_Ref);
				}
				gen.Emit(OpCodes.Ldarg_1);
				gen.Emit(OpCodes.Stfld, fi);
				gen.Emit(OpCodes.Ret);
				drset = mset.CreateDelegate(typeof(DRSet<,>).MakeGenericType(objectType, fi.FieldType));
			}

			return (MetaAccessor)Activator.CreateInstance(
				typeof(Accessor<,>).MakeGenericType(objectType, fi.FieldType),
				BindingFlags.Instance | BindingFlags.NonPublic, null,
				new object[] { fi, dget, drset }, null
				);
		}

	}
}

