using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;

namespace LinqToSqlShared.Mapping
{
	/// <summary>
	/// Constants in the mapping schema.
	/// DatabaseMapping and related classes represent a parsed version of the
	/// XML mapping string. This unvalidated intermediate representation is 
	/// necessary because unused mappings are intentially never validated.
	/// </summary>
	internal class XmlMappingConstant
	{
		internal const string Association = "Association";
		internal const string AutoSync = "AutoSync";
		internal const string Column = "Column";
		internal const string Database = "Database";
		internal const string DbType = "DbType";
		internal const string DeleteRule = "DeleteRule";
		internal const string DeleteOnNull = "DeleteOnNull";
		internal const string Direction = "Direction";
		internal const string ElementType = "ElementType";
		internal const string Expression = "Expression";
		internal const string False = "false";
		internal const string Function = "Function";
		internal const string InheritanceCode = "InheritanceCode";
		internal const string IsComposable = "IsComposable";
		internal const string IsDbGenerated = "IsDbGenerated";
		internal const string IsDiscriminator = "IsDiscriminator";
		internal const string IsPrimaryKey = "IsPrimaryKey";
		internal const string IsInheritanceDefault = "IsInheritanceDefault";
		internal const string IsForeignKey = "IsForeignKey";
		internal const string IsUnique = "IsUnique";
		internal const string IsVersion = "IsVersion";
		internal const string MappingNamespace = "http://schemas.microsoft.com/linqtosql/mapping/2007";
		internal const string Member = "Member";
		internal const string Method = "Method";
		internal const string Name = "Name";
		internal const string CanBeNull = "CanBeNull";
		internal const string OtherKey = "OtherKey";
		internal const string Parameter = "Parameter";
		internal const string Provider = "Provider";
		internal const string Return = "Return";
		internal const string Storage = "Storage";
		internal const string Table = "Table";
		internal const string ThisKey = "ThisKey";
		internal const string True = "true";
		internal const string Type = "Type";
		internal const string UpdateCheck = "UpdateCheck";
	}
}

