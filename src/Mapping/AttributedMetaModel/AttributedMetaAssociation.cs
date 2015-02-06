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
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;
using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Mapping
{
	internal class AttributedMetaAssociation : MetaAssociationImpl
	{
		AttributedMetaDataMember thisMember;
		MetaDataMember otherMember;
		ReadOnlyCollection<MetaDataMember> thisKey;
		ReadOnlyCollection<MetaDataMember> otherKey;
		MetaType otherType;
		bool isMany;
		bool isForeignKey;
		bool isUnique;
		bool isNullable = true;
		bool thisKeyIsPrimaryKey;
		bool otherKeyIsPrimaryKey;
		string deleteRule;
		bool deleteOnNull;

		internal AttributedMetaAssociation(AttributedMetaDataMember member, AssociationAttribute attr)
		{
			this.thisMember = member;

			this.isMany = TypeSystem.IsSequenceType(this.thisMember.Type);
			Type ot = this.isMany ? TypeSystem.GetElementType(this.thisMember.Type) : this.thisMember.Type;
			this.otherType = this.thisMember.DeclaringType.Model.GetMetaType(ot);
			this.thisKey = (attr.ThisKey != null) ? MakeKeys(this.thisMember.DeclaringType, attr.ThisKey) : this.thisMember.DeclaringType.IdentityMembers;
			this.otherKey = (attr.OtherKey != null) ? MakeKeys(otherType, attr.OtherKey) : this.otherType.IdentityMembers;
			this.thisKeyIsPrimaryKey = AreEqual(this.thisKey, this.thisMember.DeclaringType.IdentityMembers);
			this.otherKeyIsPrimaryKey = AreEqual(this.otherKey, this.otherType.IdentityMembers);
			this.isForeignKey = attr.IsForeignKey;

			this.isUnique = attr.IsUnique;
			this.deleteRule = attr.DeleteRule;
			this.deleteOnNull = attr.DeleteOnNull;

			// if any key members are not nullable, the association is not nullable
			foreach(MetaDataMember mm in thisKey)
			{
				if(!mm.CanBeNull)
				{
					this.isNullable = false;
					break;
				}
			}

			// validate DeleteOnNull specification
			if(deleteOnNull == true)
			{
				if(!(isForeignKey && !isMany && !isNullable))
				{
					throw Error.InvalidDeleteOnNullSpecification(member);
				}
			}

			//validate the number of ThisKey columns is the same as the number of OtherKey columns
			if(this.thisKey.Count != this.otherKey.Count && this.thisKey.Count > 0 && this.otherKey.Count > 0)
			{
				throw Error.MismatchedThisKeyOtherKey(member.Name, member.DeclaringType.Name);
			}

			// determine reverse reference member
			foreach(MetaDataMember omm in this.otherType.PersistentDataMembers)
			{
				AssociationAttribute oattr = (AssociationAttribute)Attribute.GetCustomAttribute(omm.Member, typeof(AssociationAttribute));
				if(oattr != null)
				{
					if(omm != this.thisMember && oattr.Name == attr.Name)
					{
						this.otherMember = omm;
						break;
					}
				}
			}
		}

		public override MetaType OtherType
		{
			get { return this.otherType; }
		}
		public override MetaDataMember ThisMember
		{
			get { return this.thisMember; }
		}
		public override MetaDataMember OtherMember
		{
			get { return this.otherMember; }
		}
		public override ReadOnlyCollection<MetaDataMember> ThisKey
		{
			get { return this.thisKey; }
		}
		public override ReadOnlyCollection<MetaDataMember> OtherKey
		{
			get { return this.otherKey; }
		}
		public override bool ThisKeyIsPrimaryKey
		{
			get { return this.thisKeyIsPrimaryKey; }
		}
		public override bool OtherKeyIsPrimaryKey
		{
			get { return this.otherKeyIsPrimaryKey; }
		}
		public override bool IsMany
		{
			get { return this.isMany; }
		}
		public override bool IsForeignKey
		{
			get { return this.isForeignKey; }
		}
		public override bool IsUnique
		{
			get { return this.isUnique; }
		}
		public override bool IsNullable
		{
			get { return this.isNullable; }
		}
		public override string DeleteRule
		{
			get { return this.deleteRule; }
		}
		public override bool DeleteOnNull
		{
			get { return this.deleteOnNull; }
		}
	}
}

