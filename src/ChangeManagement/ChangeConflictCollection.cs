using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using Linq;
	using System.Diagnostics.CodeAnalysis;

	public sealed class ChangeConflictCollection : ICollection<ObjectChangeConflict>, ICollection, IEnumerable<ObjectChangeConflict>, IEnumerable
	{
		private List<ObjectChangeConflict> conflicts;

		internal ChangeConflictCollection()
		{
			this.conflicts = new List<ObjectChangeConflict>();
		}

		/// <summary>
		/// The number of conflicts in the collection
		/// </summary>
		public int Count
		{
			get { return this.conflicts.Count; }
		}

		public ObjectChangeConflict this[int index]
		{
			get { return this.conflicts[index]; }
		}

		bool ICollection<ObjectChangeConflict>.IsReadOnly
		{
			get { return true; }
		}

		void ICollection<ObjectChangeConflict>.Add(ObjectChangeConflict item)
		{
			throw Error.CannotAddChangeConflicts();
		}

		/// <summary>
		/// Removes the specified conflict from the collection.
		/// </summary>
		/// <param name="item">The conflict to remove</param>
		/// <returns></returns>
		public bool Remove(ObjectChangeConflict item)
		{
			return this.conflicts.Remove(item);
		}

		/// <summary>
		/// Removes all conflicts from the collection
		/// </summary>
		public void Clear()
		{
			this.conflicts.Clear();
		}

		/// <summary>
		/// Returns true if the specified conflict is a member of the collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(ObjectChangeConflict item)
		{
			return this.conflicts.Contains(item);
		}

		public void CopyTo(ObjectChangeConflict[] array, int arrayIndex)
		{
			this.conflicts.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns the enumerator for the collection.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<ObjectChangeConflict> GetEnumerator()
		{
			return this.conflicts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.conflicts.GetEnumerator();
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return null; }
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.conflicts).CopyTo(array, index);
		}

		/// <summary>
		/// Resolves all conflicts in the collection using the specified strategy.
		/// </summary>
		/// <param name="mode">The strategy to use to resolve the conflicts.</param>
		public void ResolveAll(RefreshMode mode)
		{
			this.ResolveAll(mode, true);
		}

		/// <summary>
		/// Resolves all conflicts in the collection using the specified strategy.
		/// </summary>
		/// <param name="mode">The strategy to use to resolve the conflicts.</param>
		/// <param name="autoResolveDeletes">If true conflicts resulting from the modified
		/// object no longer existing in the database will be automatically resolved.</param>
		public void ResolveAll(RefreshMode mode, bool autoResolveDeletes)
		{
			foreach(ObjectChangeConflict c in this.conflicts)
			{
				if(!c.IsResolved)
				{
					c.Resolve(mode, autoResolveDeletes);
				}
			}
		}

		internal void Fill(List<ObjectChangeConflict> conflictList)
		{
			this.conflicts = conflictList;
		}
	}
}

