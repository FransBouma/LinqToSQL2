using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping
{
	/// <summary>
	/// A MetaTable represents an abstraction of a database table (or view)
	/// </summary>
	public abstract class MetaTable
	{
		/// <summary>
		/// The MetaModel containing this MetaTable.
		/// </summary>
		public abstract MetaModel Model { get; }
		/// <summary>
		/// The name of the table as defined by the database.
		/// </summary>
		public abstract string TableName { get; }
		/// <summary>
		/// The MetaType describing the type of the rows of the table.
		/// </summary>
		public abstract MetaType RowType { get; }
		/// <summary>
		/// The DataContext method used to perform insert operations
		/// </summary>
		public abstract MethodInfo InsertMethod { get; }
		/// <summary>
		/// The DataContext method used to perform update operations
		/// </summary>
		public abstract MethodInfo UpdateMethod { get; }
		/// <summary>
		/// The DataContext method used to perform delete operations
		/// </summary>
		public abstract MethodInfo DeleteMethod { get; }
	}
}

