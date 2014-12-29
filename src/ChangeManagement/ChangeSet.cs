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

	[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ChangeSet", Justification = "The capitalization was deliberately chosen.")]
	public sealed class ChangeSet
	{
		ReadOnlyCollection<object> inserts;
		ReadOnlyCollection<object> deletes;
		ReadOnlyCollection<object> updates;

		internal ChangeSet(
			ReadOnlyCollection<object> inserts,
			ReadOnlyCollection<object> deletes,
			ReadOnlyCollection<object> updates
			)
		{
			this.inserts = inserts;
			this.deletes = deletes;
			this.updates = updates;
		}

		public IList<object> Inserts
		{
			get { return this.inserts; }
		}

		public IList<object> Deletes
		{
			get { return this.deletes; }
		}

		public IList<object> Updates
		{
			get { return this.updates; }
		}

		public override string ToString()
		{
			return "{" +
				string.Format(
					Globalization.CultureInfo.InvariantCulture,
					"Inserts: {0}, Deletes: {1}, Updates: {2}",
					this.Inserts.Count,
					this.Deletes.Count,
					this.Updates.Count
					) + "}";
		}
	}
}

