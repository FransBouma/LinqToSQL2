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
	using System.Data.Linq.Provider;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Represents an update with one or more optimistic concurrency conflicts.
	/// </summary>
	public sealed class ObjectChangeConflict
	{
		private ChangeConflictSession session;
		private TrackedObject trackedObject;
		private bool isResolved;
		private ReadOnlyCollection<MemberChangeConflict> memberConflicts;
		private object database;
		private object original;
		private bool? isDeleted;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="session">The session in which the conflicts occurred.</param>
		/// <param name="trackedObject">The tracked item in conflict.</param>
		internal ObjectChangeConflict(ChangeConflictSession session, TrackedObject trackedObject)
		{
			this.session = session;
			this.trackedObject = trackedObject;
			this.original = trackedObject.CreateDataCopy(trackedObject.Original);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="session">The session in which the conflicts occurred.</param>
		/// <param name="trackedObject">The tracked item in conflict.</param>
		/// <param name="isDeleted">True if the item in conflict no longer exists in the database.</param>
		internal ObjectChangeConflict(ChangeConflictSession session, TrackedObject trackedObject, bool isDeleted)
			: this(session, trackedObject)
		{
			this.isDeleted = isDeleted;
		}

		internal ChangeConflictSession Session
		{
			get { return this.session; }
		}

		internal TrackedObject TrackedObject
		{
			get { return this.trackedObject; }
		}

		/// <summary>
		/// The object in conflict.
		/// </summary>
		public object Object
		{
			get { return this.trackedObject.Current; }
		}

		/// <summary>
		/// An instance containing the baseline original values used to perform the concurrency check.
		/// </summary>
		internal object Original
		{
			get { return this.original; }
		}

		/// <summary>
		/// True if the conflicts for this object have already been resovled.
		/// </summary>
		public bool IsResolved
		{
			get { return this.isResolved; }
		}

		/// <summary>
		/// True if the object in conflict has been deleted from the database.
		/// </summary>
		public bool IsDeleted
		{
			get
			{
				if(this.isDeleted.HasValue)
				{
					return this.isDeleted.Value;
				}
				return (this.Database == null);
			}
		}

		/// <summary>
		/// An instance containing the most recent values from the database
		/// </summary>
		internal object Database
		{
			get
			{
				if(this.database == null)
				{
					// use the 'refresh' context to retrieve the current database state
					DataContext ctxt = this.session.RefreshContext;
					object[] keyValues = CommonDataServices.GetKeyValues(this.trackedObject.Type, this.original);
					this.database = ctxt.Services.GetObjectByKey(this.trackedObject.Type, keyValues);
				}
				return this.database;
			}
		}

		/// <summary>
		/// Resolve member conflicts keeping current values and resetting the baseline 'Original' values
		/// to match the more recent 'Database' values.
		/// </summary>
		public void Resolve()
		{
			this.Resolve(RefreshMode.KeepCurrentValues, true);
		}

		/// <summary>
		/// Resolve member conflicts using the mode specified and resetting the baseline 'Original' values
		/// to match the more recent 'Database' values.
		/// </summary>  
		/// <param name="refreshMode">The mode that determines how the current values are 
		/// changed in order to resolve the conflict</param>
		public void Resolve(RefreshMode refreshMode)
		{
			this.Resolve(refreshMode, false);
		}

		/// <summary>
		/// Resolve member conflicts using the mode specified and resetting the baseline 'Original' values
		/// to match the more recent 'Database' values.
		/// </summary>
		/// <param name="refreshMode">The mode that determines how the current values are 
		/// changed in order to resolve the conflict</param>
		/// <param name="autoResolveDeletes">If true conflicts resulting from the modified
		/// object no longer existing in the database will be automatically resolved.</param>
		public void Resolve(RefreshMode refreshMode, bool autoResolveDeletes)
		{
			if(autoResolveDeletes && this.IsDeleted)
			{
				this.ResolveDelete();
			}
			else
			{
				// We make these calls explicity rather than simply calling
				// DataContext.Refresh (which does virtually the same thing)
				// since we want to cache the database value read.
				if(this.Database == null)
				{
					throw Error.RefreshOfDeletedObject();
				}
				trackedObject.Refresh(refreshMode, this.Database);
				this.isResolved = true;
			}
		}

		/// <summary>
		/// Resolve a conflict where we have updated an entity that no longer exists
		/// in the database.
		/// </summary>
		private void ResolveDelete()
		{
			Debug.Assert(this.IsDeleted);
			// If the user is attempting to update an entity that no longer exists 
			// in the database, we first need to [....] the delete into the local cache.
			if(!trackedObject.IsDeleted)
			{
				trackedObject.ConvertToDeleted();
			}

			// As the object have been deleted, it needs to leave the cache
			this.Session.Context.Services.RemoveCachedObjectLike(trackedObject.Type, trackedObject.Original);

			// Now that our cache is in [....], we accept the changes
			this.trackedObject.AcceptChanges();
			this.isResolved = true;
		}

		/// <summary>
		/// Returns a collection of all member conflicts that caused the update to fail.
		/// </summary>       
		public ReadOnlyCollection<MemberChangeConflict> MemberConflicts
		{
			get
			{
				if(this.memberConflicts == null)
				{
					var list = new List<MemberChangeConflict>();
					if(this.Database != null)
					{
						// determine which members are in conflict                   
						foreach(MetaDataMember metaMember in trackedObject.Type.PersistentDataMembers)
						{
							if(!metaMember.IsAssociation && this.HasMemberConflict(metaMember))
							{
								list.Add(new MemberChangeConflict(this, metaMember));
							}
						}
					}
					this.memberConflicts = list.AsReadOnly();
				}
				return this.memberConflicts;
			}
		}

		private bool HasMemberConflict(MetaDataMember member)
		{
			object oValue = member.StorageAccessor.GetBoxedValue(this.original);
			if(!member.DeclaringType.Type.IsAssignableFrom(this.database.GetType()))
			{
				return false;
			}
			object dValue = member.StorageAccessor.GetBoxedValue(this.database);
			return !this.AreEqual(member, oValue, dValue);
		}

		private bool AreEqual(MetaDataMember member, object v1, object v2)
		{
			if(v1 == null && v2 == null)
				return true;
			if(v1 == null || v2 == null)
				return false;
			if(member.Type == typeof(char[]))
			{
				return this.AreEqual((char[])v1, (char[])v2);
			}
			else if(member.Type == typeof(byte[]))
			{
				return this.AreEqual((byte[])v1, (byte[])v2);
			}
			else
			{
				return object.Equals(v1, v2);
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private bool AreEqual(char[] a1, char[] a2)
		{
			if(a1.Length != a2.Length)
				return false;
			for(int i = 0, n = a1.Length; i < n; i++)
			{
				if(a1[i] != a2[i])
					return false;
			}
			return true;
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private bool AreEqual(byte[] a1, byte[] a2)
		{
			if(a1.Length != a2.Length)
				return false;
			for(int i = 0, n = a1.Length; i < n; i++)
			{
				if(a1[i] != a2[i])
					return false;
			}
			return true;
		}

		internal void OnMemberResolved()
		{
			if(!this.IsResolved)
			{
				int nResolved = this.memberConflicts.AsEnumerable().Count(m => m.IsResolved);
				if(nResolved == this.memberConflicts.Count)
				{
					this.Resolve(RefreshMode.KeepCurrentValues, false);
				}
			}
		}
	}
}

