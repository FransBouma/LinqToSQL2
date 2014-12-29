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

	internal abstract class KeyManager
	{
		internal abstract Type KeyType { get; }
	}
}

