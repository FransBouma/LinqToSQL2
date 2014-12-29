using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using System.Data.Linq.Provider;

	internal struct MultiKey<T1, T2>
	{
		T1 value1;
		T2 value2;

		internal MultiKey(T1 value1, T2 value2)
		{
			this.value1 = value1;
			this.value2 = value2;
		}

		internal class Comparer : IEqualityComparer<MultiKey<T1, T2>>, IEqualityComparer
		{
			IEqualityComparer<T1> comparer1;
			IEqualityComparer<T2> comparer2;

			internal Comparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
			{
				this.comparer1 = comparer1;
				this.comparer2 = comparer2;
			}

			public bool Equals(MultiKey<T1, T2> x, MultiKey<T1, T2> y)
			{
				return this.comparer1.Equals(x.value1, y.value1) &&
					   this.comparer2.Equals(x.value2, y.value2);
			}

			public int GetHashCode(MultiKey<T1, T2> x)
			{
				return this.comparer1.GetHashCode(x.value1) ^ this.comparer2.GetHashCode(x.value2);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				return this.Equals((MultiKey<T1, T2>)x, (MultiKey<T1, T2>)y);
			}

			int IEqualityComparer.GetHashCode(object x)
			{
				return this.GetHashCode((MultiKey<T1, T2>)x);
			}
		}
	}
}

