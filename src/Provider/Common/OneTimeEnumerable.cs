using System.Collections;
using System.Collections.Generic;

namespace System.Data.Linq.Provider.Common
{
	internal class OneTimeEnumerable<T> : IEnumerable<T>, IEnumerable
	{
		IEnumerator<T> enumerator;

		internal OneTimeEnumerable(IEnumerator<T> enumerator)
		{
			Diagnostics.Debug.Assert(enumerator != null);
			this.enumerator = enumerator;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if(this.enumerator == null)
			{
				throw Error.CannotEnumerateResultsMoreThanOnce();
			}
			IEnumerator<T> e = this.enumerator;
			this.enumerator = null;
			return e;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}