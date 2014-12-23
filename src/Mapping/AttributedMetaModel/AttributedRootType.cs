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
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal sealed class AttributedRootType : AttributedMetaType
	{
		Dictionary<Type, MetaType> types;
		Dictionary<object, MetaType> codeMap;
		ReadOnlyCollection<MetaType> inheritanceTypes;
		MetaType inheritanceDefault;

		internal AttributedRootType(AttributedMetaModel model, AttributedMetaTable table, Type type)
			: base(model, table, type, null)
		{

			// check for inheritance and create all other types
			InheritanceMappingAttribute[] inheritanceInfo = (InheritanceMappingAttribute[])type.GetCustomAttributes(typeof(InheritanceMappingAttribute), true);
			if(inheritanceInfo.Length > 0)
			{
				if(this.Discriminator == null)
				{
					throw Error.NoDiscriminatorFound(type);
				}
				if(!MappingSystem.IsSupportedDiscriminatorType(this.Discriminator.Type))
				{
					throw Error.DiscriminatorClrTypeNotSupported(this.Discriminator.DeclaringType.Name, this.Discriminator.Name, this.Discriminator.Type);
				}
				this.types = new Dictionary<Type, MetaType>();
				this.types.Add(type, this); // add self
				this.codeMap = new Dictionary<object, MetaType>();

				// initialize inheritance types
				foreach(InheritanceMappingAttribute attr in inheritanceInfo)
				{
					if(!type.IsAssignableFrom(attr.Type))
					{
						throw Error.InheritanceTypeDoesNotDeriveFromRoot(attr.Type, type);
					}
					if(attr.Type.IsAbstract)
					{
						throw Error.AbstractClassAssignInheritanceDiscriminator(attr.Type);
					}
					AttributedMetaType mt = this.CreateInheritedType(type, attr.Type);
					if(attr.Code == null)
					{
						throw Error.InheritanceCodeMayNotBeNull();
					}
					if(mt.inheritanceCode != null)
					{
						throw Error.InheritanceTypeHasMultipleDiscriminators(attr.Type);
					}
					object codeValue = DBConvert.ChangeType(attr.Code, this.Discriminator.Type);
					foreach(object d in codeMap.Keys)
					{
						// if the keys are equal, or if they are both strings containing only spaces
						// they are considered equal
						if((codeValue.GetType() == typeof(string) && ((string)codeValue).Trim().Length == 0 &&
							d.GetType() == typeof(string) && ((string)d).Trim().Length == 0) ||
							object.Equals(d, codeValue))
						{
							throw Error.InheritanceCodeUsedForMultipleTypes(codeValue);
						}
					}
					mt.inheritanceCode = codeValue;
					this.codeMap.Add(codeValue, mt);
					if(attr.IsDefault)
					{
						if(this.inheritanceDefault != null)
						{
							throw Error.InheritanceTypeHasMultipleDefaults(type);
						}
						this.inheritanceDefault = mt;
					}
				}

				if(this.inheritanceDefault == null)
				{
					throw Error.InheritanceHierarchyDoesNotDefineDefault(type);
				}
			}

			if(this.types != null)
			{
				this.inheritanceTypes = this.types.Values.ToList().AsReadOnly();
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
				if(type != this)
				{
					TableAttribute[] attrs = (TableAttribute[])type.Type.GetCustomAttributes(typeof(TableAttribute), false);
					if(attrs.Length > 0)
						throw Error.InheritanceSubTypeIsAlsoRoot(type.Type);
				}
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

		public override bool HasInheritance
		{
			get { return this.types != null; }
		}

		private AttributedMetaType CreateInheritedType(Type root, Type type)
		{
			MetaType metaType;
			if(!this.types.TryGetValue(type, out metaType))
			{
				metaType = new AttributedMetaType(this.Model, this.Table, type, this);
				this.types.Add(type, metaType);
				if(type != root && type.BaseType != typeof(object))
				{
					this.CreateInheritedType(root, type.BaseType);
				}
			}
			return (AttributedMetaType)metaType;
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
			if(this.types != null)
			{
				this.types.TryGetValue(type, out metaType);
			}
			return metaType;
		}

		public override MetaType InheritanceDefault
		{
			get { return this.inheritanceDefault; }
		}
	}
}

