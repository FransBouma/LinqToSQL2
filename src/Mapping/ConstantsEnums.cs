using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSqlShared.Mapping
{
	/// <summary>
	/// DatabaseMapping and related classes represent a parsed version of the
	/// XML mapping string. This unvalidated intermediate representation is 
	/// necessary because unused mappings are intentially never validated.
	/// </summary>
	internal enum MappingParameterDirection
	{
		In,
		Out,
		InOut
	}
}
