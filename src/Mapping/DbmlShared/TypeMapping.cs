using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;

namespace LinqToSqlShared.Mapping
{
	/// <summary>
	/// DatabaseMapping and related classes represent a parsed version of the
	/// XML mapping string. This unvalidated intermediate representation is 
	/// necessary because unused mappings are intentially never validated.
	/// </summary>
	internal class TypeMapping
	{
		string name;
		TypeMapping baseType;
		List<MemberMapping> members;
		string inheritanceCode;
		bool isInheritanceDefault;
		List<TypeMapping> derivedTypes;

		internal TypeMapping()
		{
			this.members = new List<MemberMapping>();
			this.derivedTypes = new List<TypeMapping>();
		}

		internal TypeMapping BaseType
		{
			get { return this.baseType; }
			set { this.baseType = value; }
		}

		internal string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		internal List<MemberMapping> Members
		{
			get { return this.members; }
		}

		internal string InheritanceCode
		{
			get { return this.inheritanceCode; }
			set { this.inheritanceCode = value; }
		}

		internal bool IsInheritanceDefault
		{
			get { return this.isInheritanceDefault; }
			set { this.isInheritanceDefault = value; }
		}

		internal string XmlIsInheritanceDefault
		{
			get { return this.isInheritanceDefault ? XmlMappingConstant.True : null; }
			set { this.isInheritanceDefault = (value != null) ? bool.Parse(value) : false; }
		}

		internal List<TypeMapping> DerivedTypes
		{
			get { return this.derivedTypes; }
		}
	}
}

