using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Security;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	using System.Data.Linq.Provider;
	using System.Diagnostics.CodeAnalysis;

	internal delegate V DGet<T, V>(T t);
	internal delegate void DSet<T, V>(T t, V v);
	internal delegate void DRSet<T, V>(ref T t, V v);
}
