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
	internal class MappedType : MetaType
	{
		MetaModel model;
		MetaTable table;
		Type type;
		TypeMapping typeMapping;
		Dictionary<object, MetaDataMember> dataMemberMap;
		ReadOnlyCollection<MetaDataMember> dataMembers;
		ReadOnlyCollection<MetaDataMember> persistentDataMembers;
		ReadOnlyCollection<MetaDataMember> identities;
		MetaDataMember dbGeneratedIdentity;
		MetaDataMember version;
		MetaDataMember discriminator;
		MetaType inheritanceRoot;
		bool inheritanceBaseSet;
		MetaType inheritanceBase;
		internal object inheritanceCode;
		ReadOnlyCollection<MetaType> derivedTypes;
		ReadOnlyCollection<MetaAssociation> associations;
		bool hasMethods;
		bool hasAnyLoadMethod;
		bool hasAnyValidateMethod;
		MethodInfo onLoadedMethod;
		MethodInfo onValidateMethod;

		object locktarget = new object(); // Hold locks on private object rather than public MetaType.

		internal MappedType(MetaModel model, MetaTable table, TypeMapping typeMapping, Type type, MetaType inheritanceRoot)
		{
			this.model = model;
			this.table = table;
			this.typeMapping = typeMapping;
			this.type = type;
			this.inheritanceRoot = inheritanceRoot != null ? inheritanceRoot : this;
			this.InitDataMembers();

			this.identities = this.dataMembers.Where(m => m.IsPrimaryKey).ToList().AsReadOnly();
			this.persistentDataMembers = this.dataMembers.Where(m => m.IsPersistent).ToList().AsReadOnly();
		}
		#region Initialization
		private void ValidatePrimaryKeyMember(MetaDataMember mm)
		{
			//if the type is a sub-type, no member in the type can be primary key
			if(mm.IsPrimaryKey && this.inheritanceRoot != this && mm.Member.DeclaringType == this.type)
			{
				throw (Error.PrimaryKeyInSubTypeNotSupported(this.type.Name, mm.Name));
			}
		}

		private void InitMethods()
		{
			if(!this.hasMethods)
			{
				this.onLoadedMethod = MethodFinder.FindMethod(
					this.Type,
					"OnLoaded",
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					Type.EmptyTypes,
					false
					);
				this.onValidateMethod = MethodFinder.FindMethod(
					this.Type,
					"OnValidate",
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					new[] { typeof(ChangeAction) },
					false
					);

				this.hasAnyLoadMethod = (this.onLoadedMethod != null) || (this.InheritanceBase != null && this.InheritanceBase.HasAnyLoadMethod);
				this.hasAnyValidateMethod = (this.onValidateMethod != null) || (this.InheritanceBase != null && this.InheritanceBase.HasAnyValidateMethod);

				this.hasMethods = true;
			}
		}

		private void InitDataMembers()
		{
			if(this.dataMembers == null)
			{
				Dictionary<object, MetaDataMember> map = new Dictionary<object, MetaDataMember>();
				List<MetaDataMember> dMembers = new List<MetaDataMember>();
				int ordinal = 0;
				BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

				// Map of valid mapped names.
				Dictionary<string, MemberMapping> names = new Dictionary<string, MemberMapping>();
				Type currentType = this.type;
				for(TypeMapping tm = this.typeMapping; tm != null; tm = tm.BaseType)
				{
					foreach(MemberMapping mmap in tm.Members)
					{
						names[mmap.MemberName + ":" + currentType.Name] = mmap;
					}
					currentType = currentType.BaseType;
				}

				HashSet<string> namesSeen = new HashSet<string>(); // Keep track of which names from the mapping file have been seen.
				FieldInfo[] fis = TypeSystem.GetAllFields(this.type, flags).ToArray();
				if(fis != null)
				{
					foreach(FieldInfo fi in fis)
					{
						MemberMapping mmap;
						string name = fi.Name + ":" + fi.DeclaringType.Name;
						if(names.TryGetValue(name, out mmap))
						{
							namesSeen.Add(name);
							object dn = InheritanceRules.DistinguishedMemberName(fi);
							MetaDataMember mm;
							if(!map.TryGetValue(dn, out mm))
							{
								mm = new MappedDataMember(this, fi, mmap, ordinal);
								map.Add(InheritanceRules.DistinguishedMemberName(mm.Member), mm);
								dMembers.Add(mm);
								this.InitSpecialMember(mm);
							}
							ValidatePrimaryKeyMember(mm);
							ordinal++;
						}
					}
				}

				PropertyInfo[] pis = TypeSystem.GetAllProperties(this.type, flags).ToArray();
				if(pis != null)
				{
					foreach(PropertyInfo pi in pis)
					{
						MemberMapping mmap;
						string name = pi.Name + ":" + pi.DeclaringType.Name;
						if(names.TryGetValue(name, out mmap))
						{
							namesSeen.Add(name);
							MetaDataMember mm;
							object dn = InheritanceRules.DistinguishedMemberName(pi);
							if(!map.TryGetValue(dn, out mm))
							{
								mm = new MappedDataMember(this, pi, mmap, ordinal);
								map.Add(InheritanceRules.DistinguishedMemberName(mm.Member), mm);
								dMembers.Add(mm);
								this.InitSpecialMember(mm);
							}
							ValidatePrimaryKeyMember(mm);
							ordinal++;
						}
					}
				}

				this.dataMembers = dMembers.AsReadOnly();
				this.dataMemberMap = map;

				// Finally, make sure that all types in the mapping file were consumed.
				foreach(string name in namesSeen)
				{
					names.Remove(name);
				}
				foreach(var orphan in names)
				{
					Type aboveRoot = inheritanceRoot.Type.BaseType;
					while(aboveRoot != null)
					{
						foreach(MemberInfo mi in aboveRoot.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if(String.Compare(mi.Name, orphan.Value.MemberName, StringComparison.Ordinal) == 0)
							{
								throw Error.MappedMemberHadNoCorrespondingMemberInType(orphan.Value.MemberName, type.Name);
							}
						}
						aboveRoot = aboveRoot.BaseType;
					}
				}
			}
		}
		private void InitSpecialMember(MetaDataMember mm)
		{
			// Can only have one auto gen member that is also an identity member,
			// except if that member is a computed column (since they are implicitly auto gen)
			if(mm.IsDbGenerated && mm.IsPrimaryKey && string.IsNullOrEmpty(mm.Expression))
			{
				if(this.dbGeneratedIdentity != null)
				{
					throw Error.TwoMembersMarkedAsPrimaryKeyAndDBGenerated(mm.Member, this.dbGeneratedIdentity.Member);
				}
				this.dbGeneratedIdentity = mm;
			}
			if(mm.IsPrimaryKey && !MappingSystem.IsSupportedIdentityType(mm.Type))
			{
				throw Error.IdentityClrTypeNotSupported(mm.DeclaringType, mm.Name, mm.Type);
			}
			if(mm.IsVersion)
			{
				if(this.version != null)
				{
					throw Error.TwoMembersMarkedAsRowVersion(mm.Member, this.version.Member);
				}
				this.version = mm;
			}
			if(mm.IsDiscriminator)
			{
				if(this.discriminator != null)
				{
					if(!InheritanceRules.AreSameMember(this.discriminator.Member, mm.Member))
					{
						throw Error.TwoMembersMarkedAsInheritanceDiscriminator(mm.Member, this.discriminator.Member);
					}
				}
				else
				{
					this.discriminator = mm;
				}
			}
		}
		#endregion
		public override MetaModel Model
		{
			get { return this.model; }
		}
		public override MetaTable Table
		{
			get { return this.table; }
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
			get
			{
				if(this.table != null)
				{
					return table.RowType.IdentityMembers.Count > 0;
				}
				return false;
			}

		}
		public override bool CanInstantiate
		{
			get { return !this.type.IsAbstract && (this == this.InheritanceRoot || this.HasInheritanceCode); }
		}
		public override MetaDataMember DBGeneratedIdentityMember
		{
			get { return this.dbGeneratedIdentity; }
		}
		public override MetaDataMember VersionMember
		{
			get { return this.version; }
		}
		public override MetaDataMember Discriminator
		{
			get { return this.discriminator; }
		}
		public override bool HasUpdateCheck
		{
			get
			{
				foreach(MetaDataMember member in this.PersistentDataMembers)
				{
					if(member.UpdateCheck != UpdateCheck.Never)
					{
						return true;
					}
				}
				return false;
			}
		}
		public override bool HasInheritance
		{
			get { return this.inheritanceRoot.HasInheritance; }
		}
		public override object InheritanceCode
		{
			get { return this.inheritanceCode; }
		}
		public override bool HasInheritanceCode
		{
			get { return this.InheritanceCode != null; }
		}
		public override bool IsInheritanceDefault
		{
			get { return this.InheritanceDefault == this; }
		}
		public override MetaType InheritanceDefault
		{
			get
			{
				if(this.inheritanceRoot == this)
					throw Error.CannotGetInheritanceDefaultFromNonInheritanceClass();
				return this.InheritanceRoot.InheritanceDefault;
			}
		}
		public override MetaType InheritanceRoot
		{
			get { return this.inheritanceRoot; }
		}
		public override MetaType InheritanceBase
		{
			get
			{
				// LOCKING: Cannot initialize at construction
				if(!this.inheritanceBaseSet && this.inheritanceBase == null)
				{
					lock(this.locktarget)
					{
						if(this.inheritanceBase == null)
						{
							this.inheritanceBase = InheritanceBaseFinder.FindBase(this);
							this.inheritanceBaseSet = true;
						}
					}
				}
				return this.inheritanceBase;
			}
		}
		public override ReadOnlyCollection<MetaType> InheritanceTypes
		{
			get { return this.inheritanceRoot.InheritanceTypes; }
		}
		public override ReadOnlyCollection<MetaType> DerivedTypes
		{
			get
			{
				// LOCKING: Cannot initialize at construction because derived types
				// won't exist yet.
				if(this.derivedTypes == null)
				{
					lock(this.locktarget)
					{
						if(this.derivedTypes == null)
						{
							List<MetaType> dTypes = new List<MetaType>();
							foreach(MetaType mt in this.InheritanceTypes)
							{
								if(mt.Type.BaseType == this.type)
									dTypes.Add(mt);
							}
							this.derivedTypes = dTypes.AsReadOnly();
						}
					}
				}
				return this.derivedTypes;
			}
		}
		public override MetaType GetInheritanceType(Type inheritanceType)
		{
			foreach(MetaType mt in this.InheritanceTypes)
				if(mt.Type == inheritanceType)
					return mt;
			return null;
		}
		public override MetaType GetTypeForInheritanceCode(object key)
		{
			if(this.InheritanceRoot.Discriminator.Type == typeof(string))
			{
				string skey = (string)key;
				foreach(MetaType mt in this.InheritanceRoot.InheritanceTypes)
				{
					if(string.Compare((string)mt.InheritanceCode, skey, StringComparison.OrdinalIgnoreCase) == 0)
						return mt;
				}
			}
			else
			{
				foreach(MetaType mt in this.InheritanceRoot.InheritanceTypes)
				{
					if(object.Equals(mt.InheritanceCode, key))
						return mt;
				}
			}
			return null;
		}
		public override ReadOnlyCollection<MetaDataMember> DataMembers
		{
			get { return this.dataMembers; }
		}
		public override ReadOnlyCollection<MetaDataMember> PersistentDataMembers
		{
			get { return this.persistentDataMembers; }
		}
		public override ReadOnlyCollection<MetaDataMember> IdentityMembers
		{
			get { return this.identities; }
		}
		public override ReadOnlyCollection<MetaAssociation> Associations
		{
			get
			{
				// LOCKING: Associations are late-expanded so that cycles are broken.
				if(this.associations == null)
				{
					lock(this.locktarget)
					{
						if(this.associations == null)
						{
							this.associations = this.dataMembers.Where(m => m.IsAssociation).Select(m => m.Association).ToList().AsReadOnly();
						}
					}
				}
				return this.associations;
			}
		}
		public override MetaDataMember GetDataMember(MemberInfo mi)
		{
			if(mi == null)
				throw Error.ArgumentNull("mi");
			MetaDataMember mm;
			if(this.dataMemberMap.TryGetValue(InheritanceRules.DistinguishedMemberName(mi), out mm))
			{
				return mm;
			}
			else
			{
				if(mi.DeclaringType.IsInterface)
				{
					throw Error.MappingOfInterfacesMemberIsNotSupported(mi.DeclaringType.Name, mi.Name);
				}
				else
				{ //the member is not mapped in the base class
					throw Error.UnmappedClassMember(mi.DeclaringType.Name, mi.Name);
				}
			}
		}

		public override MethodInfo OnLoadedMethod
		{
			get
			{
				this.InitMethods();
				return this.onLoadedMethod;
			}
		}

		public override MethodInfo OnValidateMethod
		{
			get
			{
				this.InitMethods();
				return this.onValidateMethod;
			}
		}
		public override bool HasAnyValidateMethod
		{
			get
			{
				this.InitMethods();
				return this.hasAnyValidateMethod;
			}
		}
		public override bool HasAnyLoadMethod
		{
			get
			{
				this.InitMethods();
				return this.hasAnyLoadMethod;
			}
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}

