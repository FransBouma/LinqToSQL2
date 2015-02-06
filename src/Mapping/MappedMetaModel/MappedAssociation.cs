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
	internal class MappedAssociation : MetaAssociationImpl
	{
		MappedDataMember thisMember;
		MetaDataMember otherMember;
		MetaType otherType;
		ReadOnlyCollection<MetaDataMember> thisKey;
		ReadOnlyCollection<MetaDataMember> otherKey;
		bool isMany;
		bool isForeignKey;
		bool isNullable;
		bool thisKeyIsPrimaryKey;
		bool otherKeyIsPrimaryKey;
		AssociationMapping assocMap;

		internal MappedAssociation(MappedDataMember mm, AssociationMapping assocMap)
		{
			this.thisMember = mm;
			this.assocMap = assocMap;
			this.Init();
			this.InitOther();
			//validate the number of ThisKey columns is the same as the number of OtherKey columns
			if(this.thisKey.Count != this.otherKey.Count && this.thisKey.Count > 0 && this.otherKey.Count > 0)
			{
				throw Error.MismatchedThisKeyOtherKey(thisMember.Name, thisMember.DeclaringType.Name);
			}
		}
		#region Initialization
		private void Init()
		{
			this.isMany = TypeSystem.IsSequenceType(this.thisMember.Type);
			this.thisKey = (this.assocMap.ThisKey != null)
				? MakeKeys(this.thisMember.DeclaringType, this.assocMap.ThisKey)
				: this.thisMember.DeclaringType.IdentityMembers;
			// this association refers to the parent if thisKey is not our own identity
			this.thisKeyIsPrimaryKey = AreEqual(this.thisKey, this.thisMember.DeclaringType.IdentityMembers);
			this.isForeignKey = this.assocMap.IsForeignKey;

			// if any key members are not nullable, the association is not nullable
			this.isNullable = true;
			foreach(MetaDataMember mm in this.thisKey)
			{
				if(mm == null)
					throw Error.UnexpectedNull("MetaDataMember");

				if(!mm.CanBeNull)
				{
					this.isNullable = false;
					break;
				}
			}

			// validate DeleteOnNull specification
			if(assocMap.DeleteOnNull == true)
			{
				if(!(isForeignKey && !isMany && !isNullable))
				{
					throw Error.InvalidDeleteOnNullSpecification(thisMember);
				}
			}
		}
		private void InitOther()
		{
			if(this.otherType == null)
			{
				Type ot = this.isMany ? TypeSystem.GetElementType(this.thisMember.Type) : this.thisMember.Type;
				this.otherType = this.thisMember.DeclaringType.Model.GetMetaType(ot);
				System.Diagnostics.Debug.Assert(this.otherType.IsEntity);
				this.otherKey = (assocMap.OtherKey != null)
					? MakeKeys(this.otherType, this.assocMap.OtherKey)
					: this.otherType.IdentityMembers;
				this.otherKeyIsPrimaryKey = AreEqual(this.otherKey, this.otherType.IdentityMembers);
				foreach(MetaDataMember omm in this.otherType.DataMembers)
				{
					if(omm.IsAssociation && omm != this.thisMember && omm.MappedName == this.thisMember.MappedName)
					{
						this.otherMember = omm;
						break;
					}
				}
			}
		}
		#endregion
		public override MetaDataMember ThisMember
		{
			get { return this.thisMember; }
		}
		public override ReadOnlyCollection<MetaDataMember> ThisKey
		{
			get { return this.thisKey; }
		}
		public override MetaDataMember OtherMember
		{
			get { return this.otherMember; }
		}
		public override ReadOnlyCollection<MetaDataMember> OtherKey
		{
			get { return this.otherKey; }
		}
		public override MetaType OtherType
		{
			get { return this.otherType; }
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
			get { return this.assocMap.IsUnique; }
		}
		public override bool IsNullable
		{
			get { return this.isNullable; }
		}
		public override bool ThisKeyIsPrimaryKey
		{
			get { return this.thisKeyIsPrimaryKey; }
		}
		public override bool OtherKeyIsPrimaryKey
		{
			get { return this.otherKeyIsPrimaryKey; }
		}
		public override string DeleteRule
		{
			get
			{
				return this.assocMap.DeleteRule;
			}
		}
		public override bool DeleteOnNull
		{
			get
			{
				return this.assocMap.DeleteOnNull;
			}
		}
	}
}

