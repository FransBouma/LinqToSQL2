using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;
using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Mapping
{
	internal sealed class MappedDataMember : MetaDataMember
	{
		MetaType declaringType;
		MemberInfo member;
		MemberInfo storageMember;
		int ordinal;
		Type type;
		bool hasAccessors;
		MetaAccessor accPublic;
		MetaAccessor accPrivate;
		MetaAccessor accDefValue;
		MetaAccessor accDefSource;
		MemberMapping memberMap;
		MappedAssociation assoc;
		bool isNullableType;
		bool isDeferred;
		bool isPrimaryKey;
		bool isVersion;
		bool isDBGenerated;
		bool isDiscriminator;
		bool canBeNull = true;
		string dbType;
		string expression;
		string mappedName;
		UpdateCheck updateCheck = UpdateCheck.Never;
		AutoSync autoSync = AutoSync.Never;
		object locktarget = new object(); // Hold locks on private object rather than public MetaType.
		bool hasLoadMethod;
		MethodInfo loadMethod;

		internal MappedDataMember(MetaType declaringType, MemberInfo mi, MemberMapping map, int ordinal)
		{
			this.declaringType = declaringType;
			this.member = mi;
			this.ordinal = ordinal;
			this.type = TypeSystem.GetMemberType(mi);
			this.isNullableType = TypeSystem.IsNullableType(this.type);
			this.memberMap = map;
			if(this.memberMap != null && this.memberMap.StorageMemberName != null)
			{
				MemberInfo[] mis = mi.DeclaringType.GetMember(this.memberMap.StorageMemberName, BindingFlags.Instance | BindingFlags.NonPublic);
				if(mis == null || mis.Length != 1)
				{
					throw Error.BadStorageProperty(this.memberMap.StorageMemberName, mi.DeclaringType, mi.Name);
				}
				this.storageMember = mis[0];
			}
			Type storageType = this.storageMember != null ? TypeSystem.GetMemberType(this.storageMember) : this.type;
			this.isDeferred = IsDeferredType(storageType);
			ColumnMapping cmap = map as ColumnMapping;
			if(cmap != null && cmap.IsDbGenerated && cmap.IsPrimaryKey)
			{
				// auto-gen identities must be synced on insert
				if((cmap.AutoSync != AutoSync.Default) && (cmap.AutoSync != AutoSync.OnInsert))
				{
					throw Error.IncorrectAutoSyncSpecification(mi.Name);
				}
			}
			if(cmap != null)
			{
				this.isPrimaryKey = cmap.IsPrimaryKey;
				this.isVersion = cmap.IsVersion;
				this.isDBGenerated = cmap.IsDbGenerated || !string.IsNullOrEmpty(cmap.Expression) || this.isVersion;
				this.isDiscriminator = cmap.IsDiscriminator;
				this.canBeNull = cmap.CanBeNull == null ? this.isNullableType || !this.type.IsValueType : (bool)cmap.CanBeNull;
				this.dbType = cmap.DbType;
				this.expression = cmap.Expression;
				this.updateCheck = cmap.UpdateCheck;
				// auto-gen keys are always and only synced on insert
				if(this.IsDbGenerated && this.IsPrimaryKey)
				{
					this.autoSync = AutoSync.OnInsert;
				}
				else if(cmap.AutoSync != AutoSync.Default)
				{
					// if the user has explicitly set it, use their value
					this.autoSync = cmap.AutoSync;
				}
				else if(this.IsDbGenerated)
				{
					// database generated members default to always
					this.autoSync = AutoSync.Always;
				}
			}
			this.mappedName = this.memberMap.DbName != null ? this.memberMap.DbName : this.member.Name;
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
			get { return this.declaringType; }
		}
		public override bool IsDeclaredBy(MetaType metaType)
		{
			if(metaType == null)
			{
				throw Error.ArgumentNull("metaType");
			}
			return metaType.Type == this.member.DeclaringType;
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
			get { return this.memberMap != null; }
		}
		public override bool IsAssociation
		{
			get { return this.memberMap is AssociationMapping; }
		}
		public override bool IsPrimaryKey
		{
			get { return this.isPrimaryKey; }
		}
		/// <summary>
		/// Returns true if the member is explicitly marked as auto gen, or if the
		/// member is computed or generated by the database server.
		/// </summary>
		public override bool IsDbGenerated
		{
			get { return this.isDBGenerated; }
		}
		public override bool IsVersion
		{
			get { return this.isVersion; }
		}
		public override bool IsDiscriminator
		{
			get { return this.isDiscriminator; }
		}
		public override bool CanBeNull
		{
			get { return this.canBeNull; }
		}
		public override string DbType
		{
			get { return this.dbType; }
		}
		public override string Expression
		{
			get { return this.expression; }
		}
		public override string MappedName
		{
			get { return this.mappedName; }
		}
		public override UpdateCheck UpdateCheck
		{
			get { return this.updateCheck; }
		}
		public override AutoSync AutoSync
		{
			get { return this.autoSync; }
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
								this.assoc = new MappedAssociation(this, (AssociationMapping)this.memberMap);
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
				if(this.hasLoadMethod == false && this.IsDeferred)
				{
					// defer searching for this access method until we really need to know
					this.loadMethod = MethodFinder.FindMethod(
						((MappedMetaModel)this.declaringType.Model).ContextType,
						"Load" + this.member.Name,
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
						new Type[] { this.DeclaringType.Type }
						);
					this.hasLoadMethod = true;
				}
				return this.loadMethod;
			}
		}
		private bool IsDeferredType(Type clrType)
		{
			if(clrType == null || clrType == typeof(object))
			{
				return false;
			}
			if(clrType.IsGenericType)
			{
				Type gtype = clrType.GetGenericTypeDefinition();
				return gtype == typeof(Link<>) ||
					typeof(EntitySet<>).IsAssignableFrom(gtype) ||
					typeof(EntityRef<>).IsAssignableFrom(gtype) ||
					IsDeferredType(clrType.BaseType);
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
			Type declaringType, MetaAccessor accessor,
			out MetaAccessor accessorValue, out MetaAccessor accessorDeferredValue, out MetaAccessor accessorDeferredSource
			)
		{
			if(accessor.Type.IsGenericType)
			{
				Type gtype = accessor.Type.GetGenericTypeDefinition();
				Type itemType = accessor.Type.GetGenericArguments()[0];
				if(gtype == typeof(Link<>))
				{
					accessorValue = CreateAccessor(typeof(LinkValueAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					accessorDeferredValue = CreateAccessor(typeof(LinkDefValueAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					accessorDeferredSource = CreateAccessor(typeof(LinkDefSourceAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					return;
				}
				else if(typeof(EntityRef<>).IsAssignableFrom(gtype))
				{
					accessorValue = CreateAccessor(typeof(EntityRefValueAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					accessorDeferredValue = CreateAccessor(typeof(EntityRefDefValueAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					accessorDeferredSource = CreateAccessor(typeof(EntityRefDefSourceAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					return;
				}
				else if(typeof(EntitySet<>).IsAssignableFrom(gtype))
				{
					accessorValue = CreateAccessor(typeof(EntitySetValueAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					accessorDeferredValue = CreateAccessor(typeof(EntitySetDefValueAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					accessorDeferredSource = CreateAccessor(typeof(EntitySetDefSourceAccessor<,>).MakeGenericType(declaringType, itemType), accessor);
					return;
				}
			}
			throw Error.UnhandledDeferredStorageType(accessor.Type);
		}
		private static MetaAccessor CreateAccessor(Type accessorType, params object[] args)
		{
			return (MetaAccessor)Activator.CreateInstance(accessorType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);
		}
	}
}

