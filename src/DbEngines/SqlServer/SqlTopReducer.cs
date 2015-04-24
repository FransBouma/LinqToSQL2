using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.DbEngines.SqlServer
{
	/// <summary>
	/// REduces TOP to literal value if required. 
	/// </summary>
	internal class SqlTopReducer
	{
		#region Private Classes
		private class Visitor : SqlVisitor
		{
			SqlNodeAnnotations annotations;
			NodeFactory sql;

			internal Visitor(SqlNodeAnnotations annotations, NodeFactory sql)
			{
				this.annotations = annotations;
				this.sql = sql;
			}

			internal override SqlSelect VisitSelect(SqlSelect select)
			{
				base.VisitSelect(select);
				if(select.Top != null)
				{
					if(select.Top.NodeType == SqlNodeType.Value)
					{
						SqlValue val = (SqlValue)select.Top;
						// convert to literal value for SQL2K compatibility
						if(val.IsClientSpecified)
						{
							select.Top = sql.Value(val.ClrType, val.SqlType, val.Value, false, val.SourceExpression);
						}
					}
					else
					{
						// cannot be converted to literal value. note that this select is not SQL2K compatible
						this.annotations.Add(select.Top, new CompatibilityAnnotation(Strings.SourceExpressionAnnotation(select.Top.SourceExpression), SqlServerProviderMode.Sql2000));
					}
				}
				return select;
			}
		}
		#endregion

		internal static SqlNode Reduce(SqlNode node, SqlNodeAnnotations annotations, NodeFactory sql)
		{
			return new Visitor(annotations, sql).Visit(node);
		}
	}
}
