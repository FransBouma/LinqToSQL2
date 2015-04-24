using System.Collections;
using System.Collections.Generic;

namespace System.Data.Linq.Provider.Common
{
	internal class SequenceOfOne<T> : IEnumerable<T>, IEnumerable
	{
		T[] sequence;
		internal SequenceOfOne(T value)
		{
			this.sequence = new T[] { value };
		}
		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)this.sequence).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}