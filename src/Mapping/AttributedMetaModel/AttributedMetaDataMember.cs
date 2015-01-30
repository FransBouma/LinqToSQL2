using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Threading;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal sealed class AttributedMetaDataMember : MetaDataMember
	{
		AttributedMetaType metaType;
		MemberInfo member;
		MemberInfo storageMember;
		int ordinal;
		Type type;
		Type declaringType;
		bool hasAccessors;
		MetaAccessor accPublic;
		MetaAccessor accPrivate;
		MetaAccessor accDefValue;
		MetaAccessor accDefSource;
		DataAttribute attr;
		ColumnAttribute attrColumn;
		AssociationAttribute attrAssoc;
		AttributedMetaAssociation assoc;
		bool isNullableType;
		bool isDeferred;
		object locktarget = new object(); // Hold locks on private object rather than public MetaType.
		bool hasLoadMethod;
		MethodInfo loadMethod;

		internal AttributedMetaDataMember(AttributedMetaType metaType, MemberInfo mi, int ordinal)
		{
			this.declaringType = mi.DeclaringType;
			this.metaType = metaType;
			this.member = mi;
			this.ordinal = ordinal;
			this.type = TypeSystem.GetMemberType(mi);
			this.isNullableType = TypeSystem.IsNullableType(this.type);
			this.attrColumn = (ColumnAttribute)Attribute.GetCustomAttribute(mi, typeof(ColumnAttribute));
			this.attrAssoc = (AssociationAttribute)Attribute.GetCustomAttribute(mi, typeof(AssociationAttribute));
			this.attr = (this.attrColumn != null) ? (DataAttribute)this.attrColumn : (DataAttribute)this.attrAssoc;
			if(this.attr != null && this.attr.Storage != null)
			{
				MemberInfo[] mis = mi.DeclaringType.GetMember(this.attr.Storage, BindingFlags.Instance | BindingFlags.NonPublic);
				if(mis == null || mis.Length != 1)
				{
					throw Error.BadStorageProperty(this.attr.Storage, mi.DeclaringType, mi.Name);
				}
				this.storageMember = mis[0];
			}
			Type storageType = this.storageMember != null ? TypeSystem.GetMemberType(this.storageMember) : this.type;
			this.isDeferred = IsDeferredType(storageType);
			if(attrColumn != null && attrColumn.IsDbGenerated && attrColumn.IsPrimaryKey)
			{
				// auto-gen identities must be synced on insert
				if((attrColumn.AutoSync != AutoSync.Default) && (attrColumn.AutoSync != AutoSync.OnInsert))
				{
					throw Error.IncorrectAutoSyncSpecification(mi.Name);
				}
			}
		}
		private void InitAccessors()
		{
			if(!this.hasAccessors)
			{
				lock(this.locktarget)
				{
					if(!this.hasAccessors)
					{
						if(this.storageMember != null)
						{
							this.accPrivate = MakeMemberAccessor(this.member.ReflectedType, this.storageMember, null);
							if(this.isDeferred)
							{
								MakeDeferredAccessors(this.member.ReflectedType, this.accPrivate, out this.accPrivate, out this.accDefValue, out this.accDefSource);
							}
							this.accPublic = MakeMemberAccessor(this.member.ReflectedType, this.member, this.accPrivate);
						}
						else
						{
							this.accPublic = this.accPrivate = MakeMemberAccessor(this.member.ReflectedType, this.member, null);
							if(this.isDeferred)
							{
								MakeDeferredAccessors(this.member.ReflectedType, this.accPrivate, out this.accPrivate, out this.accDefValue, out this.accDefSource);
							}
						}
						this.hasAccessors = true;
					}
				}
			}
		}
		public override MetaType DeclaringType
		{
			get { return this.metaType; }
		}
		public override bool IsDeclaredBy(MetaType declaringMetaType)
		{
			if(declaringMetaType == null)
			{
				throw Error.ArgumentNull("declaringMetaType");
			}
			return declaringMetaType.Type == this.declaringType;
		}
		public override MemberInfo Member
		{
			get { return this.member; }
		}
		public override MemberInfo StorageMember
		{
			get { return this.storageMember; }
		}
		public override string Name
		{
			get { return this.member.Name; }
		}
		public override int Ordinal
		{
			get { return this.ordinal; }
		}
		public override Type Type
		{
			get { return this.type; }
		}
		public override MetaAccessor MemberAccessor
		{
			get
			{
				this.InitAccessors();
				return this.accPublic;
			}
		}
		public override MetaAccessor StorageAccessor
		{
			get
			{
				this.InitAccessors();
				return this.accPrivate;
			}
		}
		public override MetaAccessor DeferredValueAccessor
		{
			get
			{
				this.InitAccessors();
				return this.accDefValue;
			}
		}
		public override MetaAccessor DeferredSourceAccessor
		{
			get
			{
				this.InitAccessors();
				return this.accDefSource;
			}
		}
		public override bool IsDeferred
		{
			get { return this.isDeferred; }
		}
		public override bool IsPersistent
		{
			get { return this.attrColumn != null || this.attrAssoc != null; }
		}
		public override bool IsAssociation
		{
			get { return this.attrAssoc != null; }
		}
		public override bool IsPrimaryKey
		{
			get { return this.attrColumn != null && this.attrColumn.IsPrimaryKey; }
		}
		/// <summary>
		/// Returns true if the member is explicitly marked as auto gen, or if the
		/// member is computed or generated by the database server.
		/// </summary>
		public override bool IsDbGenerated
		{
			get
			{
				return this.attrColumn != null &&
				(this.attrColumn.IsDbGenerated || !string.IsNullOrEmpty(attrColumn.Expression)) || IsVersion;
			}
		}
		public override bool IsVersion
		{
			get { return this.attrColumn != null && this.attrColumn.IsVersion; }
		}
		public override bool IsDiscriminator
		{
			get { return attrColumn == null ? false : attrColumn.IsDiscriminator; }
		}
		public override bool CanBeNull
		{
			get
			{
				if(this.attrColumn == null)
				{
					return true;
				}
				if(!this.attrColumn.CanBeNullSet)
				{
					return this.isNullableType || !this.type.IsValueType;
				}
				return this.attrColumn.CanBeNull;
			}
		}
		public override string DbType
		{
			get
			{
				if(this.attrColumn != null)
				{
					return this.attrColumn.DbType;
				}
				return null;
			}
		}
		public override string Expression
		{
			get
			{
				if(this.attrColumn != null)
				{
					return this.attrColumn.Expression;
				}
				return null;
			}
		}
		public override string MappedName
		{
			get
			{
				if(this.attrColumn != null && this.attrColumn.Name != null)
				{
					return this.attrColumn.Name;
				}
				if(this.attrAssoc != null && this.attrAssoc.Name != null)
				{
					return this.attrAssoc.Name;
				}
				return this.member.Name;
			}
		}
		public override UpdateCheck UpdateCheck
		{
			get
			{
				if(this.attrColumn != null)
				{
					return this.attrColumn.UpdateCheck;
				}
				return UpdateCheck.Never;
			}
		}
		public override AutoSync AutoSync
		{
			get
			{
				if(this.attrColumn != null)
				{
					// auto-gen keys are always and only synced on insert
					if(this.IsDbGenerated && this.IsPrimaryKey)
					{
						return AutoSync.OnInsert;
					}
					// if the user has explicitly set it, use their value
					if(attrColumn.AutoSync != AutoSync.Default)
					{
						return attrColumn.AutoSync;
					}
					// database generated members default to always
					if(this.IsDbGenerated)
					{
						return AutoSync.Always;
					}
				}
				return AutoSync.Never;
			}
		}
		public override MetaAssociation Association
		{
			get
			{
				if(this.IsAssociation)
				{
					// LOCKING: This deferral isn't an optimization. It can't be done in the constructor
					// because there may be loops in the association graph.
					if(this.assoc == null)
					{
						lock(this.locktarget)
						{
							if(this.assoc == null)
							{
								this.assoc = new AttributedMetaAssociation(this, this.attrAssoc);
							}
						}
					}
				}
				return this.assoc;
			}
		}
		public override MethodInfo LoadMethod
		{
			get
			{
				if(this.hasLoadMethod == false && (this.IsDeferred || this.IsAssociation))
				{
					// defer searching for this access method until we really need to know
					this.loadMethod = MethodFinder.FindMethod(
						((AttributedMetaModel)this.metaType.Model).ContextType,
						"Load" + this.member.Name,
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
						new Type[] { this.DeclaringType.Type }
						);
					this.hasLoadMethod = true;
				}
				return this.loadMethod;
			}
		}
		private bool IsDeferredType(Type entityType)
		{
			if(entityType == null || entityType == typeof(object))
			{
				return false;
			}
			if(entityType.IsGenericType)
			{
				Type gtype = entityType.GetGenericTypeDefinition();
				return gtype == typeof(Link<>) ||
					typeof(EntitySet<>).IsAssignableFrom(gtype) ||
					typeof(EntityRef<>).IsAssignableFrom(gtype) ||
					IsDeferredType(entityType.BaseType);
			}
			return false;
		}
		private static MetaAccessor MakeMemberAccessor(Type accessorType, MemberInfo mi, MetaAccessor storage)
		{
			FieldInfo fi = mi as FieldInfo;
			MetaAccessor acc = null;
			if(fi != null)
			{
				acc = FieldAccessor.Create(accessorType, fi);
			}
			else
			{
				PropertyInfo pi = (PropertyInfo)mi;
				acc = PropertyAccessor.Create(accessorType, pi, storage);
			}
			return acc;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private static void MakeDeferredAccessors(
			Type objectDeclaringType, MetaAccessor accessor,
			out MetaAccessor accessorValue, out MetaAccessor accessorDeferredValue, out MetaAccessor accessorDeferredSource
			)
		{
			if(accessor.Type.IsGenericType)
			{
				Type gtype = accessor.Type.GetGenericTypeDefinition();
				Type itemType = accessor.Type.GetGenericArguments()[0];
				if(gtype == typeof(Link<>))
				{
					accessorValue = CreateAccessor(typeof(LinkValueAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					accessorDeferredValue = CreateAccessor(typeof(LinkDefValueAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					accessorDeferredSource = CreateAccessor(typeof(LinkDefSourceAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					return;
				}
				else if(typeof(EntityRef<>).IsAssignableFrom(gtype))
				{
					accessorValue = CreateAccessor(typeof(EntityRefValueAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					accessorDeferredValue = CreateAccessor(typeof(EntityRefDefValueAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					accessorDeferredSource = CreateAccessor(typeof(EntityRefDefSourceAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					return;
				}
				else if(typeof(EntitySet<>).IsAssignableFrom(gtype))
				{
					accessorValue = CreateAccessor(typeof(EntitySetValueAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					accessorDeferredValue = CreateAccessor(typeof(EntitySetDefValueAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					accessorDeferredSource = CreateAccessor(typeof(EntitySetDefSourceAccessor<,>).MakeGenericType(objectDeclaringType, itemType), accessor);
					return;
				}
			}
			throw Error.UnhandledDeferredStorageType(accessor.Type);
		}
		private static MetaAccessor CreateAccessor(Type accessorType, params object[] args)
		{
			return (MetaAccessor)Activator.CreateInstance(accessorType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);
		}
		public override string ToString()
		{
			return this.DeclaringType.ToString() + ":" + this.Member.ToString();
		}
	}
}

