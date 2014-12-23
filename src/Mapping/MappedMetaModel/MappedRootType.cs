using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Threading;
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal sealed class MappedRootType : MappedType
	{
		Dictionary<Type, MetaType> derivedTypes;
		Dictionary<object, MetaType> inheritanceCodes;
		ReadOnlyCollection<MetaType> inheritanceTypes;
		MetaType inheritanceDefault;
		bool hasInheritance;

		[ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // Various parameters can contain type names.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // InitInheritedType method call.
		public MappedRootType(MappedMetaModel model, MappedTable table, TypeMapping typeMapping, Type type)
			: base(model, table, typeMapping, type, null)
		{
			if(typeMapping == null)
				throw Error.ArgumentNull("typeMapping");

			if(typeMapping.InheritanceCode != null || typeMapping.DerivedTypes.Count > 0)
			{
				if(this.Discriminator == null)
				{
					throw Error.NoDiscriminatorFound(type.Name);
				}
				this.hasInheritance = true;
				if(!MappingSystem.IsSupportedDiscriminatorType(this.Discriminator.Type))
				{
					throw Error.DiscriminatorClrTypeNotSupported(this.Discriminator.DeclaringType.Name, this.Discriminator.Name, this.Discriminator.Type);
				}
				this.derivedTypes = new Dictionary<Type, MetaType>();
				this.inheritanceCodes = new Dictionary<object, MetaType>();
				this.InitInheritedType(typeMapping, this);
			}

			if(this.inheritanceDefault == null && (this.inheritanceCode != null || this.inheritanceCodes != null && this.inheritanceCodes.Count > 0))
				throw Error.InheritanceHierarchyDoesNotDefineDefault(type);

			if(this.derivedTypes != null)
			{
				this.inheritanceTypes = this.derivedTypes.Values.ToList().AsReadOnly();
			}
			else
			{
				this.inheritanceTypes = new MetaType[] { this }.ToList().AsReadOnly();
			}

			this.Validate();
		}

		private void Validate()
		{
			Dictionary<object, string> memberToColumn = new Dictionary<object, string>();
			foreach(MetaType type in this.InheritanceTypes)
			{
				// NB: Table node in XML can have only one Type node -- enforced by XSD

				foreach(MetaDataMember mem in type.PersistentDataMembers)
				{
					if(mem.IsDeclaredBy(type))
					{
						if(mem.IsDiscriminator && !this.HasInheritance)
						{
							throw Error.NonInheritanceClassHasDiscriminator(type);
						}
						if(!mem.IsAssociation)
						{
							// validate that no database column is mapped twice
							if(!string.IsNullOrEmpty(mem.MappedName))
							{
								string column;
								object dn = InheritanceRules.DistinguishedMemberName(mem.Member);
								if(memberToColumn.TryGetValue(dn, out column))
								{
									if(column != mem.MappedName)
									{
										throw Error.MemberMappedMoreThanOnce(mem.Member.Name);
									}
								}
								else
								{
									memberToColumn.Add(dn, mem.MappedName);
								}
							}
						}
					}
				}
			}
		}

		[ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // typeMap parameter's Name property.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // FindType method call.
		private MetaType InitDerivedTypes(TypeMapping typeMap)
		{
			Type type = ((MappedMetaModel)Model).FindType(typeMap.Name);
			if(type == null)
				throw Error.CouldNotFindRuntimeTypeForMapping(typeMap.Name);
			MappedType rowType = new MappedType(this.Model, this.Table, typeMap, type, this);
			return this.InitInheritedType(typeMap, rowType);
		}

		[ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // typeMap parameter's Name property.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // InitDerivedTypes method call.
		private MetaType InitInheritedType(TypeMapping typeMap, MappedType type)
		{
			this.derivedTypes.Add(type.Type, type);

			if(typeMap.InheritanceCode != null)
			{ // Mapping with no inheritance code: For example, an unmapped intermediate class in a hierarchy.
				if(this.Discriminator == null)
					throw Error.NoDiscriminatorFound(type.Name);

				if(type.Type.IsAbstract)
					throw Error.AbstractClassAssignInheritanceDiscriminator(type.Type);

				object keyValue = DBConvert.ChangeType(typeMap.InheritanceCode, this.Discriminator.Type);
				foreach(object d in inheritanceCodes.Keys)
				{
					// if the keys are equal, or if they are both strings containing only spaces
					// they are considered equal
					if((keyValue.GetType() == typeof(string) && ((string)keyValue).Trim().Length == 0 &&
						d.GetType() == typeof(string) && ((string)d).Trim().Length == 0) ||
						object.Equals(d, keyValue))
					{
						throw Error.InheritanceCodeUsedForMultipleTypes(keyValue);
					}
				}
				if(type.inheritanceCode != null)
					throw Error.InheritanceTypeHasMultipleDiscriminators(type);
				type.inheritanceCode = keyValue;
				this.inheritanceCodes.Add(keyValue, type);
				if(typeMap.IsInheritanceDefault)
				{
					if(this.inheritanceDefault != null)
						throw Error.InheritanceTypeHasMultipleDefaults(type);
					this.inheritanceDefault = type;
				}
			}

			// init sub-inherited types
			foreach(TypeMapping tm in typeMap.DerivedTypes)
			{
				this.InitDerivedTypes(tm);
			}

			return type;
		}

		public override bool HasInheritance
		{
			get { return this.hasInheritance; }
		}

		public override bool HasInheritanceCode
		{
			get { return this.InheritanceCode != null; }
		}

		public override ReadOnlyCollection<MetaType> InheritanceTypes
		{
			get { return this.inheritanceTypes; }
		}

		public override MetaType GetInheritanceType(Type type)
		{
			if(type == this.Type)
				return this;
			MetaType metaType = null;
			if(this.derivedTypes != null)
			{
				this.derivedTypes.TryGetValue(type, out metaType);
			}
			return metaType;
		}

		public override MetaType InheritanceDefault
		{
			get { return this.inheritanceDefault; }
		}
	}
}

