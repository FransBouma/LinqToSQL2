using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Provider.Interfaces;

namespace System.Data.Linq.Provider.Common
{
	internal class CompiledSubQuery : ICompiledSubQuery
	{
		QueryInfo queryInfo;
		IObjectReaderFactory factory;
		ReadOnlyCollection<Provider.NodeTypes.SqlParameter> parameters;
		ICompiledSubQuery[] subQueries;

		internal CompiledSubQuery(QueryInfo queryInfo, IObjectReaderFactory factory, ReadOnlyCollection<Provider.NodeTypes.SqlParameter> parameters, 
								  ICompiledSubQuery[] subQueries)
		{
			this.queryInfo = queryInfo;
			this.factory = factory;
			this.parameters = parameters;
			this.subQueries = subQueries;
		}

		public IExecuteResult Execute(IProvider provider, object[] parentArgs, object[] userArgs)
		{
			if(parentArgs == null && !(this.parameters == null || this.parameters.Count == 0))
			{
				throw Error.ArgumentNull("arguments");
			}

			// construct new copy of query info
			List<SqlParameterInfo> spis = new List<SqlParameterInfo>(this.queryInfo.Parameters);

			// add call arguments
			for(int i = 0, n = this.parameters.Count; i < n; i++)
			{
				spis.Add(new SqlParameterInfo(this.parameters[i], parentArgs[i]));
			}

			QueryInfo qi = new QueryInfo(
				this.queryInfo.Query,
				this.queryInfo.CommandText,
				spis.AsReadOnly(),
				this.queryInfo.ResultShape,
				this.queryInfo.ResultType
				);

			// execute query
			return provider.Execute(null, qi, this.factory, parentArgs, userArgs, subQueries, null);
		}
	}
}