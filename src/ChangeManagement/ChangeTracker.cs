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

	internal abstract class ChangeTracker
	{
		/// <summary>
		/// Starts tracking an object as 'unchanged'
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		internal abstract TrackedObject Track(object obj);
		/// <summary>
		/// Starts tracking an object as 'unchanged', and optionally
		/// 'weakly' tracks all other referenced objects recursively.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="recurse">True if all untracked objects in the graph
		/// should be tracked recursively.</param>
		/// <returns></returns>
		internal abstract TrackedObject Track(object obj, bool recurse);
		/// <summary>
		/// Fast-tracks an object that is already in identity cache
		/// </summary>
		/// <param name="obj"></param>
		internal abstract void FastTrack(object obj);
		internal abstract bool IsTracked(object obj);
		internal abstract TrackedObject GetTrackedObject(object obj);
		internal abstract void StopTracking(object obj);
		internal abstract void AcceptChanges();
		internal abstract IEnumerable<TrackedObject> GetInterestingObjects();

		internal static ChangeTracker CreateChangeTracker(CommonDataServices dataServices, bool asReadOnly)
		{
			if(asReadOnly)
			{
				return new ReadOnlyChangeTracker();
			}
			else
			{
				return new StandardChangeTracker(dataServices);
			}
		}

	}
}