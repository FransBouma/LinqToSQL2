using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class QueryExtractor
	{
		internal static SqlClientQuery Extract(SqlSubSelect subquery, IEnumerable<SqlParameter> parentParameters)
		{
			SqlClientQuery cq = new SqlClientQuery(subquery);
			if(parentParameters != null)
			{
				cq.Parameters.AddRange(parentParameters);
			}
			SubSelectDuplicator v = new SubSelectDuplicator(cq.Arguments, cq.Parameters);
			cq.Query = (SqlSubSelect)v.Visit(subquery);
			return cq;
		}
	}
}