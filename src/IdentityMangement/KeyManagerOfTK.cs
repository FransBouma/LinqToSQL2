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
	using Linq;

	internal abstract class KeyManager<T, K> : KeyManager
	{
		internal abstract K CreateKeyFromInstance(T instance);
		internal abstract bool TryCreateKeyFromValues(object[] values, out K k);
		internal abstract IEqualityComparer<K> Comparer { get; }
	}
}

