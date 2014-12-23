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
	/// A MetaAssociation represents an association relationship between two entity types.
	/// </summary>
	public abstract class MetaAssociation
	{
		/// <summary>
		/// The type on the other end of the association.
		/// </summary>
		public abstract MetaType OtherType { get; }
		/// <summary>
		/// The member on this side that represents the association.
		/// </summary>
		public abstract MetaDataMember ThisMember { get; }
		/// <summary>
		/// The member on the other side of this association that represents the reverse association (may be null).
		/// </summary>
		public abstract MetaDataMember OtherMember { get; }
		/// <summary>
		/// A list of members representing the values on this side of the association.
		/// </summary>
		public abstract ReadOnlyCollection<MetaDataMember> ThisKey { get; }
		/// <summary>
		/// A list of members representing the values on the other side of the association.
		/// </summary>
		public abstract ReadOnlyCollection<MetaDataMember> OtherKey { get; }
		/// <summary>
		/// True if the association is OneToMany.
		/// </summary>
		public abstract bool IsMany { get; }
		/// <summary>
		/// True if the other type is the parent of this type.
		/// </summary>
		public abstract bool IsForeignKey { get; }
		/// <summary>
		/// True if the association is unique (defines a uniqueness constraint).
		/// </summary>
		public abstract bool IsUnique { get; }
		/// <summary>
		/// True if the association may be null (key values).
		/// </summary>
		public abstract bool IsNullable { get; }
		/// <summary>
		/// True if the ThisKey forms the identity (primary key) of the this type.
		/// </summary>
		public abstract bool ThisKeyIsPrimaryKey { get; }
		/// <summary>
		/// True if the OtherKey forms the identity (primary key) of the other type.
		/// </summary>
		public abstract bool OtherKeyIsPrimaryKey { get; }
		/// <summary>
		/// Specifies the behavior when the child is deleted (e.g. CASCADE, SET NULL).
		/// Returns null if no action is specified on delete.
		/// </summary>
		public abstract string DeleteRule { get; }
		/// <summary>
		/// Specifies whether the object should be deleted when this association
		/// is set to null.
		/// </summary>
		public abstract bool DeleteOnNull { get; }
	}
}

