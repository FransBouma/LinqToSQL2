using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal class SqlParameterizer
	{
		#region Member Declarations
		private TypeSystemProvider _typeProvider;
		private SqlNodeAnnotations _annotations;
		private int _index;
		#endregion

		internal SqlParameterizer(TypeSystemProvider typeProvider, SqlNodeAnnotations annotations)
		{
			this._typeProvider = typeProvider;
			this._annotations = annotations;
		}

		internal ReadOnlyCollection<SqlParameterInfo> Parameterize(SqlNode node)
		{
			return this.ParameterizeInternal(node).AsReadOnly();
		}

		private List<SqlParameterInfo> ParameterizeInternal(SqlNode node)
		{
			SqlParameterInfoProducer v = new SqlParameterInfoProducer(this);
			v.Visit(node);
			return new List<SqlParameterInfo>(v.CurrentParams);
		}

		internal ReadOnlyCollection<ReadOnlyCollection<SqlParameterInfo>> ParameterizeBlock(SqlBlock block)
		{
			SqlParameterInfo rowStatus =
				new SqlParameterInfo(
					new SqlParameter(typeof(int), _typeProvider.From(typeof(int)), "@ROWCOUNT", block.SourceExpression)
					);
			List<ReadOnlyCollection<SqlParameterInfo>> list = new List<ReadOnlyCollection<SqlParameterInfo>>();
			for(int i = 0, n = block.Statements.Count; i < n; i++)
			{
				SqlNode statement = block.Statements[i];
				List<SqlParameterInfo> parameters = this.ParameterizeInternal(statement);
				if(i > 0)
				{
					parameters.Add(rowStatus);
				}
				list.Add(parameters.AsReadOnly());
			}
			return list.AsReadOnly();
		}

		internal virtual string CreateParameterName()
		{
			return "@p" + this._index++;
		}


		internal TypeSystemProvider TypeProvider
		{
			get { return _typeProvider; }
		}

		internal SqlNodeAnnotations Annotations
		{
			get { return _annotations; }
		}
	}
}
