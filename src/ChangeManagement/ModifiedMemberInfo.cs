using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using System.Data.Linq.Provider;
	using System.Diagnostics.CodeAnalysis;
	using System.Data.Linq.BindingLists;

	[SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "[....]: Types are never compared to each other.  When comparisons happen it is against the entities that are represented by these constructs.")]
	public struct ModifiedMemberInfo
	{
		MemberInfo member;
		object current;
		object original;

		internal ModifiedMemberInfo(MemberInfo member, object current, object original)
		{
			this.member = member;
			this.current = current;
			this.original = original;
		}

		public MemberInfo Member
		{
			get { return this.member; }
		}

		public object CurrentValue
		{
			get { return this.current; }
		}

		public object OriginalValue
		{
			get { return this.original; }
		}
	}
}

