using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlGatherColumnsProduced
	{
		static internal List<SqlColumn> GatherColumns(SqlSource source)
		{
			List<SqlColumn> columns = new List<SqlColumn>();
			new ProducedColumnsGatherer(columns).Visit(source);
			return columns;
		}
	}
}