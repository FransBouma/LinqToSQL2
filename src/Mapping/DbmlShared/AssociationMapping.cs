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
	internal sealed class AssociationMapping : MemberMapping
	{
		string thisKey;
		string otherKey;
		string deleteRule;
		bool deleteOnNull;
		bool isForeignKey;
		bool isUnique;

		internal AssociationMapping()
		{
		}

		internal string ThisKey
		{
			get { return this.thisKey; }
			set { this.thisKey = value; }
		}

		internal string OtherKey
		{
			get { return this.otherKey; }
			set { this.otherKey = value; }
		}

		internal string DeleteRule
		{
			get { return this.deleteRule; }
			set { this.deleteRule = value; }
		}

		internal bool DeleteOnNull
		{
			get { return this.deleteOnNull; }
			set { this.deleteOnNull = value; }
		}

		internal bool IsForeignKey
		{
			get { return this.isForeignKey; }
			set { this.isForeignKey = value; }
		}

		internal string XmlIsForeignKey
		{
			get { return this.isForeignKey ? XmlMappingConstant.True : null; }
			set { this.isForeignKey = (value != null) ? bool.Parse(value) : false; }
		}

		internal string XmlDeleteOnNull
		{
			get { return this.deleteOnNull ? XmlMappingConstant.True : null; }
			set { this.deleteOnNull = (value != null) ? bool.Parse(value) : false; }
		}

		internal bool IsUnique
		{
			get { return this.isUnique; }
			set { this.isUnique = value; }
		}

		internal string XmlIsUnique
		{
			get { return this.isUnique ? XmlMappingConstant.True : null; }
			set { this.isUnique = (value != null) ? bool.Parse(value) : false; }
		}
	}
}

