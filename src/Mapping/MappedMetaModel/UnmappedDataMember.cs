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
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal sealed class UnmappedDataMember : MetaDataMember
	{
		MetaType declaringType;
		MemberInfo member;
		int ordinal;
		Type type;
		MetaAccessor accPublic;
		object lockTarget = new object();

		internal UnmappedDataMember(MetaType declaringType, MemberInfo mi, int ordinal)
		{
			this.declaringType = declaringType;
			this.member = mi;
			this.ordinal = ordinal;
			this.type = TypeSystem.GetMemberType(mi);
		}
		private void InitAccessors()
		{
			if(this.accPublic == null)
			{
				lock(this.lockTarget)
				{
					if(this.accPublic == null)
					{
						this.accPublic = MakeMemberAccessor(this.member.ReflectedType, this.member);
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
			get { return this.member; }
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
				return this.accPublic;
			}
		}
		public override MetaAccessor DeferredValueAccessor
		{
			get { return null; }
		}
		public override MetaAccessor DeferredSourceAccessor
		{
			get { return null; }
		}
		public override bool IsDeferred
		{
			get { return false; }
		}
		public override bool IsPersistent
		{
			get { return false; }
		}
		public override bool IsAssociation
		{
			get { return false; }
		}
		public override bool IsPrimaryKey
		{
			get { return false; }
		}
		public override bool IsDbGenerated
		{
			get { return false; }
		}
		public override bool IsVersion
		{
			get { return false; }
		}
		public override bool IsDiscriminator
		{
			get { return false; }
		}
		public override bool CanBeNull
		{
			get { return !this.type.IsValueType || TypeSystem.IsNullableType(this.type); }
		}
		public override string DbType
		{
			get { return null; }
		}
		public override string Expression
		{
			get { return null; }
		}
		public override string MappedName
		{
			get { return this.member.Name; }
		}
		public override UpdateCheck UpdateCheck
		{
			get { return UpdateCheck.Never; }
		}
		public override AutoSync AutoSync
		{
			get { return AutoSync.Never; }
		}
		public override MetaAssociation Association
		{
			get { return null; }
		}
		public override MethodInfo LoadMethod
		{
			get { return null; }
		}
		private static MetaAccessor MakeMemberAccessor(Type accessorType, MemberInfo mi)
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
				acc = PropertyAccessor.Create(accessorType, pi, null);
			}
			return acc;
		}
	}
}

