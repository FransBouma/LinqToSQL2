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
	internal class ReturnMapping
	{
#warning [FB] THIS TYPE IS A BIT USELESS, AS IT ONLY WRAPS A STRING VALUE. SEE WHETHER IT'S REALLY NEEDED. 
		string dbType;

		internal string DbType
		{
			get { return this.dbType; }
			set { this.dbType = value; }
		}
	}
}

