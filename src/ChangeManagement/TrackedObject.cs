using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using System.Data.Linq.Provider;

	internal abstract class TrackedObject
	{
		internal abstract MetaType Type { get; }
		/// <summary>
		/// The current client value.
		/// </summary>
		internal abstract object Current { get; }
		/// <summary>
		/// The last read database value.  This is updated whenever the
		/// item is refreshed.
		/// </summary>
		internal abstract object Original { get; }
		internal abstract bool IsInteresting { get; } // new, deleted or possibly changed
		internal abstract bool IsNew { get; }
		internal abstract bool IsDeleted { get; }
		internal abstract bool IsModified { get; }
		internal abstract bool IsUnmodified { get; }
		internal abstract bool IsPossiblyModified { get; }
		internal abstract bool IsRemoved { get; }
		internal abstract bool IsDead { get; }
		/// <summary>
		/// True if the object is being tracked (perhaps during a recursive
		/// attach operation) but can be transitioned to other states.
		/// </summary>
		internal abstract bool IsWeaklyTracked { get; }
		internal abstract bool HasDeferredLoaders { get; }
		internal abstract bool HasChangedValues();
		internal abstract IEnumerable<ModifiedMemberInfo> GetModifiedMembers();
		internal abstract bool HasChangedValue(MetaDataMember mm);
		internal abstract bool CanInferDelete();
		internal abstract void AcceptChanges();
		internal abstract void ConvertToNew();
		internal abstract void ConvertToPossiblyModified();
		internal abstract void ConvertToPossiblyModified(object original);
		internal abstract void ConvertToUnmodified();
		internal abstract void ConvertToModified();
		internal abstract void ConvertToDeleted();
		internal abstract void ConvertToRemoved();
		internal abstract void ConvertToDead();
		/// <summary>
		/// Refresh the item by making the value passed in the current 
		/// Database value, and refreshing the current values using the
		/// mode specified.
		/// </summary>       
		internal abstract void Refresh(RefreshMode mode, object freshInstance);
		/// <summary>
		/// Does the refresh operation for a single member.  This method does not 
		/// update the baseline 'original' value.  You must call 
		/// Refresh(RefreshMode.KeepCurrentValues, freshInstance) to finish the refresh 
		/// after refreshing individual members.
		/// </summary>
		/// <param name="member"></param>
		/// <param name="mode"></param>
		/// <param name="freshValue"></param>
		internal abstract void RefreshMember(MetaDataMember member, RefreshMode mode, object freshValue);
		/// <summary>
		/// Create a data-member only copy of the instance (no associations)
		/// </summary>
		/// <returns></returns>
		internal abstract object CreateDataCopy(object instance);

		internal abstract bool SynchDependentData();

		internal abstract bool IsPendingGeneration(IEnumerable<MetaDataMember> keyMembers);
		internal abstract bool IsMemberPendingGeneration(MetaDataMember keyMember);

		internal abstract void InitializeDeferredLoaders();
	}
}

