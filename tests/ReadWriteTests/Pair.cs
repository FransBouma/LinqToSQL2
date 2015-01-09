using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadWriteTests.SqlServer
{
	public class Pair<T1, T2>
	{
		public Pair()
		{
		}

		public Pair(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		public T1 Value1 { get; set; }
		public T2 Value2 { get; set; }
	}
}
