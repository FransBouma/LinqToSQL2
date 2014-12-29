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
	public struct EntityRef<TEntity>
		where TEntity : class
	{
		IEnumerable<TEntity> source;
		TEntity entity;

		public EntityRef(TEntity entity)
		{
			this.entity = entity;
			this.source = SourceState<TEntity>.Assigned;
		}

		public EntityRef(IEnumerable<TEntity> source)
		{
			this.source = source;
			this.entity = default(TEntity);
		}

		public EntityRef(EntityRef<TEntity> entityRef)
		{
			this.source = entityRef.source;
			this.entity = entityRef.entity;
		}

		public TEntity Entity
		{
			get
			{
				if(this.HasSource)
				{

					IEnumerable<TEntity> src = this.source;
					this.entity = Enumerable.SingleOrDefault(src);
					this.source = SourceState<TEntity>.Loaded;
				}
				return this.entity;
			}
			set
			{
				this.entity = value;
				this.source = SourceState<TEntity>.Assigned;
			}
		}

		public bool HasLoadedOrAssignedValue
		{
			get { return this.HasLoadedValue || this.HasAssignedValue; }
		}

		internal bool HasValue
		{
			get { return this.source == null || this.HasLoadedValue || this.HasAssignedValue; }
		}

		internal bool HasLoadedValue
		{
			get { return this.source == SourceState<TEntity>.Loaded; }
		}

		internal bool HasAssignedValue
		{
			get { return this.source == SourceState<TEntity>.Assigned; }
		}

		internal bool HasSource
		{
			get { return this.source != null && !this.HasLoadedValue && !this.HasAssignedValue; }
		}

		internal IEnumerable<TEntity> Source
		{
			get { return this.source; }
		}

		internal TEntity UnderlyingValue
		{
			get { return this.entity; }
		}
	}
}

