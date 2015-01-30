using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal enum SqlServerProviderMode
	{
		NotYetDecided,
		Sql2000,
		Sql2005,
		Sql2008,
		SqlCE
		// Add more here.
	}
}
