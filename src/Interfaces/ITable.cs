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
	/// ITable is the common interface for DataContext tables. It can be used as the source
	/// of a dynamic/runtime-generated query.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "[....]: Meant to represent a database table which is delayed loaded and doesn't provide collection semantics.")]
	public interface ITable : IQueryable
	{
		/// <summary>
		/// The DataContext containing this Table.
		/// </summary>
		DataContext Context { get; }
		/// <summary>
		/// Adds an entity in a 'pending insert' state to this table.  The added entity will not be observed
		/// in query results from this table until after SubmitChanges has been called. Any untracked
		/// objects referenced directly or transitively by the entity will also be inserted.
		/// </summary>
		/// <param name="entity"></param>
		void InsertOnSubmit(object entity);
		/// <summary>
		/// Adds all entities of a collection to the DataContext in a 'pending insert' state.
		/// The added entities will not be observed in query results until after SubmitChanges() 
		/// has been called. Any untracked objects referenced directly or transitively by the
		/// the inserted entities will also be inserted.
		/// </summary>
		/// <param name="entities"></param>
		void InsertAllOnSubmit(IEnumerable entities);
		/// <summary>
		/// Attaches an entity to the DataContext in an unmodified state, similiar to as if it had been 
		/// retrieved via a query. Other entities accessible from this entity are attached as unmodified 
		/// but may subsequently be transitioned to other states by performing table operations on them
		/// individually.
		/// </summary>
		/// <param name="entity"></param>
		void Attach(object entity);
		/// <summary>
		/// Attaches an entity to the DataContext in either a modified or unmodified state.
		/// If attaching as modified, the entity must either declare a version member or must 
		/// not participate in update conflict checking. Other entities accessible from this 
		/// entity are attached as unmodified but may subsequently be transitioned to other 
		/// states by performing table operations on them individually.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="asModified"></param>
		void Attach(object entity, bool asModified);
		/// <summary>
		/// Attaches an entity to the DataContext in either a modified or unmodified state by specifying both the entity
		/// and its original state. Other entities accessible from this 
		/// entity are attached as unmodified but may subsequently be transitioned to other 
		/// states by performing table operations on them individually.
		/// </summary>
		/// <param name="entity">The entity to attach.</param>
		/// <param name="original">An instance of the same entity type with data members containing
		/// the original values.</param>
		void Attach(object entity, object original);
		/// <summary>
		/// Attaches all entities of a collection to the DataContext in an unmodified state, 
		/// similiar to as if each had been retrieved via a query. Other entities accessible from these 
		/// entities are attached as unmodified but may subsequently be transitioned to other 
		/// states by performing table operations on them individually.
		/// </summary>
		/// <param name="entities"></param>
		void AttachAll(IEnumerable entities);
		/// <summary>
		/// Attaches all entities of a collection to the DataContext in either a modified or unmodified state.
		/// If attaching as modified, the entity must either declare a version member or must not participate in update conflict checking.
		/// Other entities accessible from these 
		/// entities are attached as unmodified but may subsequently be transitioned to other 
		/// states by performing table operations on them individually.
		/// </summary>
		/// <param name="entities">The collection of entities.</param>
		/// <param name="asModified">True if the entities are to be attach as modified.</param>
		void AttachAll(IEnumerable entities, bool asModified);
		/// <summary>
		/// Puts an entity from this table into a 'pending delete' state.  The removed entity will not be observed
		/// missing from query results until after SubmitChanges() has been called.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		void DeleteOnSubmit(object entity);
		/// <summary>
		/// Puts all entities from the collection 'entities' into a 'pending delete' state.  The removed entities will
		/// not be observed missing from the query results until after SubmitChanges() is called.
		/// </summary>
		/// <param name="entities"></param>
		void DeleteAllOnSubmit(IEnumerable entities);
		/// <summary>
		/// Returns an instance containing the original state of the entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		object GetOriginalEntityState(object entity);
		/// <summary>
		/// Returns an array of modified members containing their current and original values
		/// for the entity specified.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		ModifiedMemberInfo[] GetModifiedMembers(object entity);
		/// <summary>
		/// True if the table is read-only.
		/// </summary>
		bool IsReadOnly { get; }
	}
}

