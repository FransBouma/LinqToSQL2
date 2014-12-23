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
	internal abstract class MemberMapping
	{
		string name;
		string member;
		string storageMember;

		internal MemberMapping()
		{
		}

		internal string DbName
		{
			get { return this.name; }
			set { this.name = value; }
		}

		internal string MemberName
		{
			get { return this.member; }
			set { this.member = value; }
		}

		internal string StorageMemberName
		{
			get { return this.storageMember; }
			set { this.storageMember = value; }
		}
	}
}

