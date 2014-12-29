using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Linq
{
	/// <summary>
	/// Describes the type of change the entity will undergo when submitted to the database.
	/// </summary>
	public enum ChangeAction
	{
		/// <summary>
		/// The entity will not be submitted.
		/// </summary>
		None = 0,
		/// <summary>
		/// The entity will be deleted.
		/// </summary>
		Delete,
		/// <summary>
		/// The entity will be inserted.
		/// </summary>
		Insert,
		/// <summary>
		/// The entity will be updated.
		/// </summary>
		Update
	}


	/// <summary>
	/// Used to specify how a submit should behave when one
	/// or more updates fail due to optimistic concurrency
	/// conflicts.
	/// </summary>
	public enum ConflictMode
	{
		/// <summary>
		/// Fail immediately when the first change conflict is encountered.
		/// </summary>
		FailOnFirstConflict,
		/// <summary>
		/// Only fail after all changes have been attempted.
		/// </summary>
		ContinueOnConflict
	}

	/// <summary>
	/// Used to specify a value synchronization strategy. 
	/// </summary>
	public enum RefreshMode
	{
		/// <summary>
		/// Keep the current values.
		/// </summary>
		KeepCurrentValues,
		/// <summary>
		/// Current values that have been changed are not modified, but
		/// any unchanged values are updated with the current database
		/// values.  No changes are lost in this merge.
		/// </summary>
		KeepChanges,
		/// <summary>
		/// All current values are overwritten with current database values,
		/// regardless of whether they have been changed.
		/// </summary>
		OverwriteCurrentValues
	}
}
