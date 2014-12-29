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

	internal abstract class IdentityCache
	{
		internal abstract object Find(object[] keyValues);
		internal abstract object FindLike(object instance);
		internal abstract object InsertLookup(object instance);
		internal abstract bool RemoveLike(object instance);
	}
}

