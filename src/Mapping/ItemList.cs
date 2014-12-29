using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Data.Linq.BindingLists;

namespace System.Data.Linq
{
	internal struct ItemList<T> where T : class
	{
		T[] items;
		int count;

		public int Count
		{
			get { return count; }
		}

		public T[] Items
		{
			get { return items; }
		}

		public T this[int index]
		{
			get { return items[index]; }
			set { items[index] = value; }
		}

		public void Add(T item)
		{
			if(items == null || items.Length == count) GrowItems();
			items[count] = item;
			count++;
		}

		public bool Contains(T item)
		{
			return IndexOf(item) >= 0;
		}

		public Enumerator GetEnumerator()
		{
			Enumerator e;
			e.items = items;
			e.index = -1;
			e.endIndex = count - 1;
			return e;
		}

		public bool Include(T item)
		{
			if(LastIndexOf(item) >= 0) return false;
			Add(item);
			return true;
		}

		public int IndexOf(T item)
		{
			for(int i = 0; i < count; i++)
			{
				if(items[i] == item) return i;
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			if(items == null || items.Length == count) GrowItems();
			if(index < count) Array.Copy(items, index, items, index + 1, count - index);
			items[index] = item;
			count++;
		}

		public int LastIndexOf(T item)
		{
			int i = count;
			while(i > 0)
			{
				--i;
				if(items[i] == item) return i;
			}
			return -1;
		}

		public bool Remove(T item)
		{
			int i = IndexOf(item);
			if(i < 0) return false;
			RemoveAt(i);
			return true;
		}

		public void RemoveAt(int index)
		{
			count--;
			if(index < count) Array.Copy(items, index + 1, items, index, count - index);
			items[count] = default(T);
		}

		void GrowItems()
		{
			Array.Resize(ref items, count == 0 ? 4 : count * 2);
		}

		public struct Enumerator
		{
			internal T[] items;
			internal int index;
			internal int endIndex;

			public bool MoveNext()
			{
				if(index == endIndex) return false;
				index++;
				return true;
			}

			public T Current
			{
				get { return items[index]; }
			}
		}
	}
}

