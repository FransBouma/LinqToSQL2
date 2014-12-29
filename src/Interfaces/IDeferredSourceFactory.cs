using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;

	internal interface IDeferredSourceFactory
	{
		IEnumerable CreateDeferredSource(object instance);
		IEnumerable CreateDeferredSource(object[] keyValues);
	}
}

