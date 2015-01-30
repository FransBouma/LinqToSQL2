using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Threading;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal static class MethodFinder
	{
		internal static MethodInfo FindMethod(Type type, string name, BindingFlags flags, Type[] argTypes)
		{
			return FindMethod(type, name, flags, argTypes, true);
		}

		internal static MethodInfo FindMethod(Type type, string name, BindingFlags flags, Type[] argTypes, bool allowInherit)
		{
			for(; type != typeof(object); type = type.BaseType)
			{
				MethodInfo mi = type.GetMethod(name, flags | BindingFlags.DeclaredOnly, null, argTypes, null);
				if(mi != null || !allowInherit)
				{
					return mi;
				}
			}
			return null;
		}
	}
}

