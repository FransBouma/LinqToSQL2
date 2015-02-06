using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Linq.Provider.Common
{
	internal class OrderedResults<T> : IOrderedEnumerable<T>, IEnumerable<T>
	{
		#region Member Declarations
		private List<T> values;
		#endregion

		internal OrderedResults(IEnumerable<T> results)
		{
			this.values = results as List<T>;
			if(this.values == null)
				this.values = new List<T>(results);
		}
		IOrderedEnumerable<T> IOrderedEnumerable<T>.CreateOrderedEnumerable<K>(Func<T, K> keySelector, IComparer<K> comparer, bool descending)
		{
			throw Error.NotSupported();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.values).GetEnumerator();
		}
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.values.GetEnumerator();
		}
	}
}