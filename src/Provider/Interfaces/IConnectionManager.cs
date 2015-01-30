using System;
using System.Data;
using System.Data.Common;

namespace System.Data.Linq.Provider.Interfaces
{

	internal interface IConnectionManager
	{
		DbConnection UseConnection(IConnectionUser user);
		void ReleaseConnection(IConnectionUser user);
	}
}
