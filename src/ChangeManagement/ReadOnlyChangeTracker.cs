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
	using Linq;

	/// <summary>
	/// This is the implementation used when change tracking is disabled.
	/// </summary>
	internal class ReadOnlyChangeTracker : ChangeTracker
	{
		internal override TrackedObject Track(object obj) { return null; }
		internal override TrackedObject Track(object obj, bool recurse) { return null; }
		internal override void FastTrack(object obj) 
		{ 
			// nop
		}
		internal override bool IsTracked(object obj) { return false; }
		internal override TrackedObject GetTrackedObject(object obj) { return null; }
		internal override void StopTracking(object obj) { }
		internal override void AcceptChanges() 
		{ 
			// nop
		}
		internal override IEnumerable<TrackedObject> GetInterestingObjects() { return new TrackedObject[0]; }
	}
}

