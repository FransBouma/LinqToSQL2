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
	/// A MetaType represents the mapping of a domain object type onto a database table's columns.
	/// </summary>
	public abstract class MetaType
	{
		/// <summary>
		/// The MetaModel containing this MetaType.
		/// </summary>
		public abstract MetaModel Model { get; }
		/// <summary>
		/// The MetaTable using this MetaType for row definition.
		/// </summary>
		public abstract MetaTable Table { get; }
		/// <summary>
		/// The underlying CLR type.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "The contexts in which this is available are fairly specific.")]
		public abstract Type Type { get; }
		/// <summary>
		/// The name of the MetaType (same as the CLR type's name).
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// True if the MetaType is an entity type.
		/// </summary>
		public abstract bool IsEntity { get; }
		/// <summary>
		/// True if the underlying type can be instantiated as the result of a query.
		/// </summary>
		public abstract bool CanInstantiate { get; }
		/// <summary>
		/// The member that represents the auto-generated identity column, or null if there is none.
		/// </summary>
		public abstract MetaDataMember DBGeneratedIdentityMember { get; }
		/// <summary>
		/// The member that represents the row-version or timestamp column, or null if there is none.
		/// </summary>
		public abstract MetaDataMember VersionMember { get; }
		/// <summary>
		/// The member that represents the inheritance discriminator column, or null if there is none.
		/// </summary>
		public abstract MetaDataMember Discriminator { get; }
		/// <summary>
		/// True if the type has any persistent member with an UpdateCheck policy other than Never.
		/// </summary>
		public abstract bool HasUpdateCheck { get; }
		/// <summary>
		/// True if the type is part of a mapped inheritance hierarchy.
		/// </summary>
		public abstract bool HasInheritance { get; }
		/// <summary>
		/// True if this type defines an inheritance code.
		/// </summary>
		public abstract bool HasInheritanceCode { get; }
		/// <summary>
		/// The inheritance code defined by this type.
		/// </summary>
		public abstract object InheritanceCode { get; }
		/// <summary>
		/// True if this type is used as the default of an inheritance hierarchy.
		/// </summary>
		public abstract bool IsInheritanceDefault { get; }
		/// <summary>
		/// The root type of the inheritance hierarchy.
		/// </summary>
		public abstract MetaType InheritanceRoot { get; }
		/// <summary>
		/// The base metatype in the inheritance hierarchy.
		/// </summary>
		public abstract MetaType InheritanceBase { get; }
		/// <summary>
		/// The type that is the default of the inheritance hierarchy.
		/// </summary>
		public abstract MetaType InheritanceDefault { get; }
		/// <summary>
		/// Gets the MetaType for an inheritance sub type.
		/// </summary>
		/// <param name="type">The root or sub type of the inheritance hierarchy.</param>
		/// <returns>The MetaType.</returns>
		public abstract MetaType GetInheritanceType(Type type);
		/// <summary>
		/// Gets type associated with the specified inheritance code.
		/// </summary>
		/// <param name="code">The inheritance code</param>
		/// <returns>The MetaType.</returns>
		public abstract MetaType GetTypeForInheritanceCode(object code);
		/// <summary>
		/// Gets an enumeration of all types defined by an inheritance hierarchy.
		/// </summary>
		/// <returns>Enumeration of MetaTypes.</returns>
		public abstract ReadOnlyCollection<MetaType> InheritanceTypes { get; }
		/// <summary>
		/// Returns true if the MetaType or any base MetaType has an OnLoaded method.
		/// </summary>
		public abstract bool HasAnyLoadMethod { get; }
		/// <summary>
		/// Returns true if the MetaType or any base MetaType has an OnValidate method.
		/// </summary>
		public abstract bool HasAnyValidateMethod { get; }
		/// <summary>
		/// Gets an enumeration of the immediate derived types in an inheritance hierarchy.
		/// </summary>
		/// <returns>Enumeration of MetaTypes.</returns>
		public abstract ReadOnlyCollection<MetaType> DerivedTypes { get; }
		/// <summary>
		/// Gets an enumeration of all the data members (fields and properties).
		/// </summary>
		public abstract ReadOnlyCollection<MetaDataMember> DataMembers { get; }
		/// <summary>
		/// Gets an enumeration of all the persistent data members (fields and properties mapped into database columns).
		/// </summary>
		public abstract ReadOnlyCollection<MetaDataMember> PersistentDataMembers { get; }
		/// <summary>
		/// Gets an enumeration of all the data members that define up the unique identity of the type.
		/// </summary>
		public abstract ReadOnlyCollection<MetaDataMember> IdentityMembers { get; }
		/// <summary>
		/// Gets an enumeration of all the associations.
		/// </summary>
		public abstract ReadOnlyCollection<MetaAssociation> Associations { get; }
		/// <summary>
		/// Gets the MetaDataMember associated with the specified member.
		/// </summary>
		/// <param name="member">The CLR member.</param>
		/// <returns>The MetaDataMember if there is one, otherwise null.</returns>
		public abstract MetaDataMember GetDataMember(MemberInfo member);
		/// <summary>
		/// The method called when the entity is first loaded.
		/// </summary>
		public abstract MethodInfo OnLoadedMethod { get; }
		/// <summary>
		/// The method called to ensure the entity is in a valid state.
		/// </summary>
		public abstract MethodInfo OnValidateMethod { get; }
	}
}

