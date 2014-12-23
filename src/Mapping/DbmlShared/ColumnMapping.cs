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
	internal sealed class ColumnMapping : MemberMapping
	{
		string dbType;
		string expression;
		bool isPrimaryKey;
		bool isDBGenerated;
		bool isVersion;
		bool isDiscriminator;
		bool? canBeNull = null;
		UpdateCheck updateCheck;
		AutoSync autoSync;

		internal ColumnMapping()
		{
		}

		internal string DbType
		{
			get { return this.dbType; }
			set { this.dbType = value; }
		}

		internal bool? CanBeNull
		{
			get { return this.canBeNull; }
			set { this.canBeNull = value; }
		}

		internal string XmlCanBeNull
		{
			get
			{
				if(this.canBeNull == null) return null;
				return this.canBeNull == true ? null : XmlMappingConstant.False;
			}
			set { this.canBeNull = (value != null) ? bool.Parse(value) : true; }
		}

		internal string Expression
		{
			get { return this.expression; }
			set { this.expression = value; }
		}

		internal bool IsPrimaryKey
		{
			get { return this.isPrimaryKey; }
			set { this.isPrimaryKey = value; }
		}

		internal string XmlIsPrimaryKey
		{
			get { return this.isPrimaryKey ? XmlMappingConstant.True : null; }
			set { this.isPrimaryKey = (value != null) ? bool.Parse(value) : false; }
		}

		internal bool IsDbGenerated
		{
			get { return this.isDBGenerated; }
			set { this.isDBGenerated = value; }
		}

		internal string XmlIsDbGenerated
		{
			get { return this.isDBGenerated ? XmlMappingConstant.True : null; }
			set { this.isDBGenerated = (value != null) ? bool.Parse(value) : false; }
		}

		internal bool IsVersion
		{
			get { return this.isVersion; }
			set { this.isVersion = value; }
		}

		internal string XmlIsVersion
		{
			get { return this.isVersion ? XmlMappingConstant.True : null; }
			set { this.isVersion = (value != null) ? bool.Parse(value) : false; }
		}

		internal bool IsDiscriminator
		{
			get { return this.isDiscriminator; }
			set { this.isDiscriminator = value; }
		}

		internal string XmlIsDiscriminator
		{
			get { return this.isDiscriminator ? XmlMappingConstant.True : null; }
			set { this.isDiscriminator = (value != null) ? bool.Parse(value) : false; }
		}

		internal UpdateCheck UpdateCheck
		{
			get { return this.updateCheck; }
			set { this.updateCheck = value; }
		}

		internal string XmlUpdateCheck
		{
			get { return this.updateCheck != UpdateCheck.Always ? this.updateCheck.ToString() : null; }
			set { this.updateCheck = (value == null) ? UpdateCheck.Always : (UpdateCheck)Enum.Parse(typeof(UpdateCheck), value); }
		}

		internal AutoSync AutoSync
		{
			get { return this.autoSync; }
			set { this.autoSync = value; }
		}

		internal string XmlAutoSync
		{
			get { return this.autoSync != AutoSync.Default ? this.autoSync.ToString() : null; }
			set { this.autoSync = (value != null) ? (AutoSync)Enum.Parse(typeof(AutoSync), value) : AutoSync.Default; }
		}
	}
}

