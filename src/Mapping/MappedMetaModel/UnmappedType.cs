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
	internal sealed class UnmappedType : MetaType
	{
		MetaModel model;
		Type type;
		Dictionary<object, MetaDataMember> dataMemberMap;
		ReadOnlyCollection<MetaDataMember> dataMembers;
		ReadOnlyCollection<MetaType> inheritanceTypes;
		object locktarget = new object(); // Hold locks on private object rather than public MetaType.

		private static ReadOnlyCollection<MetaType> _emptyTypes = new List<MetaType>().AsReadOnly();
		private static ReadOnlyCollection<MetaDataMember> _emptyDataMembers = new List<MetaDataMember>().AsReadOnly();
		private static ReadOnlyCollection<MetaAssociation> _emptyAssociations = new List<MetaAssociation>().AsReadOnly();

		internal UnmappedType(MetaModel model, Type type)
		{
			this.model = model;
			this.type = type;
		}

		public override MetaModel Model
		{
			get { return this.model; }
		}
		public override MetaTable Table
		{
			get { return null; }
		}
		public override Type Type
		{
			get { return this.type; }
		}
		public override string Name
		{
			get { return this.type.Name; }
		}
		public override bool IsEntity
		{
			get { return false; }
		}
		public override bool CanInstantiate
		{
			get { return !this.type.IsAbstract; }
		}
		public override MetaDataMember DBGeneratedIdentityMember
		{
			get { return null; }
		}
		public override MetaDataMember VersionMember
		{
			get { return null; }
		}
		public override MetaDataMember Discriminator
		{
			get { return null; }
		}
		public override bool HasUpdateCheck
		{
			get { return false; }
		}
		public override ReadOnlyCollection<MetaType> InheritanceTypes
		{
			get
			{
				if(this.inheritanceTypes == null)
				{
					lock(this.locktarget)
					{
						if(this.inheritanceTypes == null)
						{
							this.inheritanceTypes = new MetaType[] { this }.ToList().AsReadOnly();
						}
					}
				}
				return this.inheritanceTypes;
			}
		}
		public override MetaType GetInheritanceType(Type inheritanceType)
		{
			if(inheritanceType == this.type)
				return this;
			return null;
		}
		public override ReadOnlyCollection<MetaType> DerivedTypes
		{
			get { return _emptyTypes; }
		}
		public override MetaType GetTypeForInheritanceCode(object key)
		{
			return null;
		}
		public override bool HasInheritance
		{
			get { return false; }
		}
		public override bool HasInheritanceCode
		{
			get { return false; }
		}
		public override object InheritanceCode
		{
			get { return null; }
		}
		public override MetaType InheritanceRoot
		{
			get { return this; }
		}
		public override MetaType InheritanceBase
		{
			get { return null; }
		}
		public override MetaType InheritanceDefault
		{
			get { return null; }
		}
		public override bool IsInheritanceDefault
		{
			get { return false; }
		}
		public override ReadOnlyCollection<MetaDataMember> DataMembers
		{
			get
			{
				this.InitDataMembers();
				return this.dataMembers;
			}
		}
		public override ReadOnlyCollection<MetaDataMember> PersistentDataMembers
		{
			get { return _emptyDataMembers; }
		}
		public override ReadOnlyCollection<MetaDataMember> IdentityMembers
		{
			get
			{
				this.InitDataMembers();
				return this.dataMembers;
			}
		}
		public override ReadOnlyCollection<MetaAssociation> Associations
		{
			get { return _emptyAssociations; }
		}
		public override MetaDataMember GetDataMember(MemberInfo mi)
		{
			if(mi == null)
				throw Error.ArgumentNull("mi");
			this.InitDataMembers();
			if(this.dataMemberMap == null)
			{
				lock(this.locktarget)
				{
					if(this.dataMemberMap == null)
					{
						Dictionary<object, MetaDataMember> map = new Dictionary<object, MetaDataMember>();
						foreach(MetaDataMember mm in this.dataMembers)
						{
							map.Add(InheritanceRules.DistinguishedMemberName(mm.Member), mm);
						}
						this.dataMemberMap = map;
					}
				}
			}
			object dn = InheritanceRules.DistinguishedMemberName(mi);
			MetaDataMember mdm;
			this.dataMemberMap.TryGetValue(dn, out mdm);
			return mdm;
		}

		private void InitDataMembers()
		{
			if(this.dataMembers == null)
			{
				lock(this.locktarget)
				{
					if(this.dataMembers == null)
					{
						List<MetaDataMember> dMembers = new List<MetaDataMember>();
						int ordinal = 0;
						BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
						foreach(FieldInfo fi in this.type.GetFields(flags))
						{
							MetaDataMember mm = new UnmappedDataMember(this, fi, ordinal);
							dMembers.Add(mm);
							ordinal++;
						}
						foreach(PropertyInfo pi in this.type.GetProperties(flags))
						{
							MetaDataMember mm = new UnmappedDataMember(this, pi, ordinal);
							dMembers.Add(mm);
							ordinal++;
						}
						this.dataMembers = dMembers.AsReadOnly();
					}
				}
			}
		}

		public override string ToString()
		{
			return this.Name;
		}

		public override MethodInfo OnLoadedMethod
		{
			get { return null; }
		}

		public override MethodInfo OnValidateMethod
		{
			get { return null; }
		}
		public override bool HasAnyValidateMethod
		{
			get
			{
				return false;
			}
		}
		public override bool HasAnyLoadMethod
		{
			get
			{
				return false;
			}
		}
	}
}

