using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Data.Linq.BindingLists;

namespace System.Data.Linq
{
	[SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "[....]: Types are never compared to each other.  When comparisons happen it is against the entities that are represented by these constructs.")]
	public struct Link<T>
	{
		T underlyingValue;
		IEnumerable<T> source;

		public Link(T value)
		{
			this.underlyingValue = value;
			this.source = null;
		}

		public Link(IEnumerable<T> source)
		{
			this.source = source;
			this.underlyingValue = default(T);
		}

		public Link(Link<T> link)
		{
			this.underlyingValue = link.underlyingValue;
			this.source = link.source;
		}

		public bool HasValue
		{
			get { return this.source == null || this.HasLoadedValue || this.HasAssignedValue; }
		}

		public bool HasLoadedOrAssignedValue
		{
			get { return this.HasLoadedValue || this.HasAssignedValue; }
		}

		internal bool HasLoadedValue
		{
			get { return this.source == SourceState<T>.Loaded; }
		}

		internal bool HasAssignedValue
		{
			get { return this.source == SourceState<T>.Assigned; }
		}

		internal T UnderlyingValue
		{
			get { return this.underlyingValue; }
		}

		internal IEnumerable<T> Source
		{
			get { return this.source; }
		}

		internal bool HasSource
		{
			get { return this.source != null && !this.HasAssignedValue && !this.HasLoadedValue; }
		}

		public T Value
		{
			get
			{
				if(this.HasSource)
				{
					this.underlyingValue = Enumerable.SingleOrDefault(this.source);
					this.source = SourceState<T>.Loaded;
				}
				return this.underlyingValue;
			}
			set
			{
				this.underlyingValue = value;
				this.source = SourceState<T>.Assigned;
			}
		}
	}
}

