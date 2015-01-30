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
	using Linq;
	using System.Diagnostics.CodeAnalysis;
	using System.Data.Linq.BindingLists;

	/// <summary>
	/// Defines behavior for implementations of IQueryable that allow modifications to the membership of the resulting set.
	/// </summary>
	/// <typeparam name="TEntity">Type of entities returned from the queryable.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public interface ITable<TEntity> : IQueryable<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Notify the set that an object representing a new entity should be added to the set.
		/// Depending on the implementation, the change to the set may not be visible in an enumeration of the set 
		/// until changes to that set have been persisted in some manner.
		/// </summary>
		/// <param name="entity">Entity object to be added.</param>
		void InsertOnSubmit(TEntity entity);

		/// <summary>
		/// Notify the set that an object representing a new entity should be added to the set.
		/// Depending on the implementation, the change to the set may not be visible in an enumeration of the set 
		/// until changes to that set have been persisted in some manner.
		/// </summary>
		/// <param name="entity">Entity object to be attached.</param>
		void Attach(TEntity entity);

		/// <summary>
		/// Notify the set that an object representing an entity should be removed from the set.
		/// Depending on the implementation, the change to the set may not be visible in an enumeration of the set 
		/// until changes to that set have been persisted in some manner.
		/// </summary>
		/// <param name="entity">Entity object to be removed.</param>
		/// <exception cref="InvalidOperationException">Throws if the specified object is not in the set.</exception>
		void DeleteOnSubmit(TEntity entity);
	}
}

