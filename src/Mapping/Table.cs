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

	/// <summary>
	/// Table is a collection of persistent entities. It always contains the set of entities currently 
	/// persisted in the database. Use it as a source of queries and to add/insert and remove/delete entities.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "[....]: Meant to represent a database table which is delayed loaded and doesn't provide collection semantics.")]
	public sealed class Table<TEntity> : IQueryable<TEntity>, IQueryProvider, IEnumerable<TEntity>, IQueryable, IEnumerable, ITable, IListSource, ITable<TEntity>
		where TEntity : class
	{
		DataContext context;
		MetaTable metaTable;

		internal Table(DataContext context, MetaTable metaTable)
		{
			System.Diagnostics.Debug.Assert(metaTable != null);
			this.context = context;
			this.metaTable = metaTable;
		}

		/// <summary>
		/// The DataContext containing this Table.
		/// </summary>
		public DataContext Context
		{
			get { return this.context; }
		}

		/// <summary>
		/// True if the table is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return !metaTable.RowType.IsEntity; }
		}

		Expression IQueryable.Expression
		{
			get { return Expression.Constant(this); }
		}

		Type IQueryable.ElementType
		{
			get { return typeof(TEntity); }
		}

		IQueryProvider IQueryable.Provider
		{
			get
			{
				return (IQueryProvider)this;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		IQueryable IQueryProvider.CreateQuery(Expression expression)
		{
			if(expression == null)
			{
				throw Error.ArgumentNull("expression");
			}
			Type eType = System.Data.Linq.SqlClient.TypeSystem.GetElementType(expression.Type);
			Type qType = typeof(IQueryable<>).MakeGenericType(eType);
			if(!qType.IsAssignableFrom(expression.Type))
			{
				throw Error.ExpectedQueryableArgument("expression", qType);
			}
			Type dqType = typeof(DataQuery<>).MakeGenericType(eType);
			return (IQueryable)Activator.CreateInstance(dqType, new object[] { this.context, expression });
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		IQueryable<TResult> IQueryProvider.CreateQuery<TResult>(Expression expression)
		{
			if(expression == null)
			{
				throw Error.ArgumentNull("expression");
			}
			if(!typeof(IQueryable<TResult>).IsAssignableFrom(expression.Type))
			{
				throw Error.ExpectedQueryableArgument("expression", typeof(IEnumerable<TResult>));
			}
			return new DataQuery<TResult>(this.context, expression);
		}

		object IQueryProvider.Execute(Expression expression)
		{
			return this.context.Provider.Execute(expression).ReturnValue;
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		TResult IQueryProvider.Execute<TResult>(Expression expression)
		{
			return (TResult)this.context.Provider.Execute(expression).ReturnValue;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<TEntity> GetEnumerator()
		{
			return ((IEnumerable<TEntity>)this.context.Provider.Execute(Expression.Constant(this)).ReturnValue).GetEnumerator();
		}

		bool IListSource.ContainsListCollection
		{
			get { return false; }
		}

		private IBindingList cachedList;

		IList IListSource.GetList()
		{
			if(cachedList == null)
			{
				cachedList = GetNewBindingList();
			}
			return cachedList;
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method doesn't represent a property of the type.")]
		public IBindingList GetNewBindingList()
		{
			return BindingList.Create<TEntity>(this.context, this);
		}

		/// <summary>
		/// Adds an entity in a 'pending insert' state to this table.  The added entity will not be observed
		/// in query results from this table until after SubmitChanges() has been called.  Any untracked
		/// objects referenced directly or transitively by the entity will also be inserted.
		/// </summary>
		/// <param name="entity"></param>
		public void InsertOnSubmit(TEntity entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			MetaType type = this.metaTable.RowType.GetInheritanceType(entity.GetType());
			if(!IsTrackableType(type))
			{
				throw Error.TypeCouldNotBeAdded(type.Type);
			}
			TrackedObject tracked = this.context.Services.ChangeTracker.GetTrackedObject(entity);
			if(tracked == null)
			{
				tracked = this.context.Services.ChangeTracker.Track(entity);
				tracked.ConvertToNew();
			}
			else if(tracked.IsWeaklyTracked)
			{
				tracked.ConvertToNew();
			}
			else if(tracked.IsDeleted)
			{
				tracked.ConvertToPossiblyModified();
			}
			else if(tracked.IsRemoved)
			{
				tracked.ConvertToNew();
			}
			else if(!tracked.IsNew)
			{
				throw Error.CantAddAlreadyExistingItem();
			}
		}

		void ITable.InsertOnSubmit(object entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			this.InsertOnSubmit(tEntity);
		}

		/// <summary>
		/// Adds all entities of a collection to the DataContext in a 'pending insert' state.
		/// The added entities will not be observed in query results until after SubmitChanges() 
		/// has been called.
		/// </summary>
		/// <param name="entities"></param>
		public void InsertAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			List<TSubEntity> list = entities.ToList();
			foreach(TEntity entity in list)
			{
				this.InsertOnSubmit(entity);
			}
		}

		void ITable.InsertAllOnSubmit(IEnumerable entities)
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			List<object> list = entities.Cast<object>().ToList();
			ITable itable = this;
			foreach(object entity in list)
			{
				itable.InsertOnSubmit(entity);
			}
		}

		/// <summary>
		/// Returns true if this specific type is mapped into the database.
		/// For example, an abstract type can't be present because it can not be instantiated.
		/// </summary>
		private static bool IsTrackableType(MetaType type)
		{
			if(type == null)
			{
				return false;
			}
			if(!type.CanInstantiate)
			{
				return false;
			}
			if(type.HasInheritance && !type.HasInheritanceCode)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Puts an entity from this table into a 'pending delete' state.  The removed entity will not be observed
		/// missing from query results until after SubmitChanges() has been called.
		/// </summary>
		/// <param name="item"></param>
		public void DeleteOnSubmit(TEntity entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			TrackedObject tracked = this.context.Services.ChangeTracker.GetTrackedObject(entity);
			if(tracked != null)
			{
				if(tracked.IsNew)
				{
					tracked.ConvertToRemoved();
				}
				else if(tracked.IsPossiblyModified || tracked.IsModified)
				{
					tracked.ConvertToDeleted();
				}
			}
			else
			{
				throw Error.CannotRemoveUnattachedEntity();
			}
		}

		void ITable.DeleteOnSubmit(object entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			this.DeleteOnSubmit(tEntity);
		}

		/// <summary>
		/// Puts all entities from the collection 'entities' into a 'pending delete' state.  The removed entities will
		/// not be observed missing from the query results until after SubmitChanges() is called.
		/// </summary>
		/// <param name="entities"></param>
		public void DeleteAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			List<TSubEntity> list = entities.ToList();
			foreach(TEntity entity in list)
			{
				this.DeleteOnSubmit(entity);
			}
		}

		void ITable.DeleteAllOnSubmit(IEnumerable entities)
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			List<object> list = entities.Cast<object>().ToList();
			ITable itable = this;
			foreach(object entity in list)
			{
				itable.DeleteOnSubmit(entity);
			}
		}

		/// <summary>
		/// Attaches an entity to the DataContext in an unmodified state, similiar to as if it had been 
		/// retrieved via a query. Deferred loading is not enabled. Other entities accessible from this
		/// entity are not automatically attached.
		/// </summary>
		/// <param name="entity"></param>
		public void Attach(TEntity entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			this.Attach(entity, false);
		}

		void ITable.Attach(object entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			this.Attach(tEntity, false);
		}

		/// <summary>
		/// Attaches an entity to the DataContext in either a modified or unmodified state.
		/// If attaching as modified, the entity must either declare a version member or must not participate in update conflict checking.
		/// Deferred loading is not enabled. Other entities accessible from this entity are not automatically attached.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="asModified"></param>
		public void Attach(TEntity entity, bool asModified)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			MetaType type = this.metaTable.RowType.GetInheritanceType(entity.GetType());
			if(!IsTrackableType(type))
			{
				throw Error.TypeCouldNotBeTracked(type.Type);
			}
			if(asModified)
			{
				bool canAttach = type.VersionMember != null || !type.HasUpdateCheck;
				if(!canAttach)
				{
					throw Error.CannotAttachAsModifiedWithoutOriginalState();
				}
			}
			TrackedObject tracked = this.Context.Services.ChangeTracker.GetTrackedObject(entity);
			if(tracked == null || tracked.IsWeaklyTracked)
			{
				if(tracked == null)
				{
					tracked = this.context.Services.ChangeTracker.Track(entity, true);
				}
				if(asModified)
				{
					tracked.ConvertToModified();
				}
				else
				{
					tracked.ConvertToUnmodified();
				}
				if(this.Context.Services.InsertLookupCachedObject(type, entity) != entity)
				{
					throw new DuplicateKeyException(entity, Strings.CantAddAlreadyExistingKey);
				}
				tracked.InitializeDeferredLoaders();
			}
			else
			{
				throw Error.CannotAttachAlreadyExistingEntity();
			}
		}

		void ITable.Attach(object entity, bool asModified)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			this.Attach(tEntity, asModified);
		}

		/// <summary>
		/// Attaches an entity to the DataContext in either a modified or unmodified state by specifying both the entity
		/// and its original state.
		/// </summary>
		/// <param name="entity">The entity to attach.</param>
		/// <param name="original">An instance of the same entity type with data members containing
		/// the original values.</param>
		public void Attach(TEntity entity, TEntity original)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			if(original == null)
			{
				throw Error.ArgumentNull("original");
			}
			if(entity.GetType() != original.GetType())
			{
				throw Error.OriginalEntityIsWrongType();
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			MetaType type = this.metaTable.RowType.GetInheritanceType(entity.GetType());
			if(!IsTrackableType(type))
			{
				throw Error.TypeCouldNotBeTracked(type.Type);
			}
			TrackedObject tracked = this.context.Services.ChangeTracker.GetTrackedObject(entity);
			if(tracked == null || tracked.IsWeaklyTracked)
			{
				if(tracked == null)
				{
					tracked = this.context.Services.ChangeTracker.Track(entity, true);
				}
				tracked.ConvertToPossiblyModified(original);
				if(this.Context.Services.InsertLookupCachedObject(type, entity) != entity)
				{
					throw new DuplicateKeyException(entity, Strings.CantAddAlreadyExistingKey);
				}
				tracked.InitializeDeferredLoaders();
			}
			else
			{
				throw Error.CannotAttachAlreadyExistingEntity();
			}
		}

		void ITable.Attach(object entity, object original)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			if(original == null)
			{
				throw Error.ArgumentNull("original");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			if(entity.GetType() != original.GetType())
			{
				throw Error.OriginalEntityIsWrongType();
			}
			this.Attach(tEntity, (TEntity)original);
		}

		/// <summary>
		/// Attaches all entities of a collection to the DataContext in an unmodified state, 
		/// similiar to as if each had been retrieved via a query. Deferred loading is not enabled. 
		/// Other entities accessible from these entities are not automatically attached.
		/// </summary>
		/// <param name="entities"></param>
		public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			this.AttachAll(entities, false);
		}

		void ITable.AttachAll(IEnumerable entities)
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			((ITable)this).AttachAll(entities, false);
		}

		/// <summary>
		/// Attaches all entities of a collection to the DataContext in either a modified or unmodified state.
		/// If attaching as modified, the entity must either declare a version member or must not participate in update conflict checking.
		/// Deferred loading is not enabled.  Other entities accessible from these entities are not automatically attached.
		/// </summary>
		/// <param name="entities">The collection of entities.</param>
		/// <param name="asModified">True if the entities are to be attach as modified.</param>
		public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities, bool asModified) where TSubEntity : TEntity
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			List<TSubEntity> list = entities.ToList();
			foreach(TEntity entity in list)
			{
				this.Attach(entity, asModified);
			}
		}

		void ITable.AttachAll(IEnumerable entities, bool asModified)
		{
			if(entities == null)
			{
				throw Error.ArgumentNull("entities");
			}
			CheckReadOnly();
			context.CheckNotInSubmitChanges();
			context.VerifyTrackingEnabled();
			List<object> list = entities.Cast<object>().ToList();
			ITable itable = this;
			foreach(object entity in list)
			{
				itable.Attach(entity, asModified);
			}
		}

		/// <summary>
		/// Returns an instance containing the original state of the entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public TEntity GetOriginalEntityState(TEntity entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			MetaType type = this.Context.Mapping.GetMetaType(entity.GetType());
			if(type == null || !type.IsEntity)
			{
				throw Error.EntityIsTheWrongType();
			}
			TrackedObject tracked = this.Context.Services.ChangeTracker.GetTrackedObject(entity);
			if(tracked != null)
			{
				if(tracked.Original != null)
				{
					return (TEntity)tracked.CreateDataCopy(tracked.Original);
				}
				else
				{
					return (TEntity)tracked.CreateDataCopy(tracked.Current);
				}
			}
			return null;
		}

		object ITable.GetOriginalEntityState(object entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			return this.GetOriginalEntityState(tEntity);
		}

		/// <summary>
		/// Returns an array of modified members containing their current and original values
		/// for the entity specified.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public ModifiedMemberInfo[] GetModifiedMembers(TEntity entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			MetaType type = this.Context.Mapping.GetMetaType(entity.GetType());
			if(type == null || !type.IsEntity)
			{
				throw Error.EntityIsTheWrongType();
			}
			TrackedObject tracked = this.Context.Services.ChangeTracker.GetTrackedObject(entity);
			if(tracked != null)
			{
				return tracked.GetModifiedMembers().ToArray();
			}
			return new ModifiedMemberInfo[] { };
		}

		ModifiedMemberInfo[] ITable.GetModifiedMembers(object entity)
		{
			if(entity == null)
			{
				throw Error.ArgumentNull("entity");
			}
			TEntity tEntity = entity as TEntity;
			if(tEntity == null)
			{
				throw Error.EntityIsTheWrongType();
			}
			return this.GetModifiedMembers(tEntity);
		}

		private void CheckReadOnly()
		{
			if(this.IsReadOnly)
			{
				throw Error.CannotPerformCUDOnReadOnlyTable(ToString());
			}
		}

		public override string ToString()
		{
			return "Table(" + typeof(TEntity).Name + ")";
		}
	}
}

