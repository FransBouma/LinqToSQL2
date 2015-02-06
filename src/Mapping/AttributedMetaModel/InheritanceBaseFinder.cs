using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal static class InheritanceBaseFinder
	{
		internal static MetaType FindBase(MetaType derivedType)
		{
			if(derivedType.Type == typeof(object))
			{
				return null;
			}

			var clrType = derivedType.Type; // start
			var rootClrType = derivedType.InheritanceRoot.Type; // end
			var metaTable = derivedType.Table;
			MetaType metaType = null;

			while(true)
			{
				if(clrType == typeof(object) || clrType == rootClrType)
				{
					return null;
				}

				clrType = clrType.BaseType;
				metaType = derivedType.InheritanceRoot.GetInheritanceType(clrType);

				if(metaType != null)
				{
					return metaType;
				}
			}
		}
	}
}

