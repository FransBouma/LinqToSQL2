using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.BindingLists
{
	internal static class BindingList
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal static IBindingList Create<T>(DataContext context, IEnumerable<T> sequence)
		{
			List<T> list = sequence.ToList();
			MetaTable metaTable = context.Services.Model.GetTable(typeof(T));
			if(metaTable != null)
			{
				ITable table = context.GetTable(metaTable.RowType.Type);
				Type bindingType = typeof(DataBindingList<>).MakeGenericType(metaTable.RowType.Type);
				return (IBindingList)Activator.CreateInstance(bindingType,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
					new object[] { list, table }, null
					);
			}
			else
			{
				return new SortableBindingList<T>(list);
			}
		}
	}
}

