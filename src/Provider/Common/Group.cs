using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Linq.Provider.Common
{
	internal class Group<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
	{
		#region Member Declarations
		private K key;
		private IEnumerable<T> items;
		#endregion

		internal Group(K key, IEnumerable<T> items)
		{
			this.key = key;
			this.items = items;
		}

		K IGrouping<K, T>.Key
		{
			get { return this.key; }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)this.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
	}
}