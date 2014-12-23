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
	internal class TableMapping
	{
		string tableName;
		string member;
		TypeMapping rowType;

		internal TableMapping()
		{
		}

		internal string TableName
		{
			get { return this.tableName; }
			set { this.tableName = value; }
		}

		internal string Member
		{
			get { return this.member; }
			set { this.member = value; }
		}

		internal TypeMapping RowType
		{
			get { return this.rowType; }
			set { this.rowType = value; }
		}
	}
}

