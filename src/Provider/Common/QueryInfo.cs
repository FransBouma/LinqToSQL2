using System.Collections.ObjectModel;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Common
{
	internal class QueryInfo
	{
		SqlNode query;
		string commandText;
		ReadOnlyCollection<SqlParameterInfo> parameters;
		ResultShape resultShape;
		Type resultType;

		internal QueryInfo(SqlNode query, string commandText, ReadOnlyCollection<SqlParameterInfo> parameters, ResultShape resultShape, Type resultType)
		{
			this.query = query;
			this.commandText = commandText;
			this.parameters = parameters;
			this.resultShape = resultShape;
			this.resultType = resultType;
		}
		internal SqlNode Query
		{
			get { return this.query; }
		}
		internal string CommandText
		{
			get { return this.commandText; }
		}
		internal ReadOnlyCollection<SqlParameterInfo> Parameters
		{
			get { return this.parameters; }
		}
		internal ResultShape ResultShape
		{
			get { return this.resultShape; }
		}
		internal Type ResultType
		{
			get { return this.resultType; }
		}
	}
}