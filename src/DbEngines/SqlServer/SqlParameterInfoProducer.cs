using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal class SqlParameterInfoProducer : SqlVisitor
	{
		private SqlParameterizer parameterizer;
		private Dictionary<object, SqlParameterInfo> map;
		private List<SqlParameterInfo> currentParams;
		private bool topLevel;
		private ProviderType timeProviderType;  // for special case handling of DateTime parameters

		internal SqlParameterInfoProducer(SqlParameterizer parameterizer)
		{
			this.parameterizer = parameterizer;
			this.topLevel = true;
			this.map = new Dictionary<object, SqlParameterInfo>();
			this.currentParams = new List<SqlParameterInfo>();
		}

		private SqlParameter InsertLookup(SqlValue cp)
		{
			SqlParameterInfo pi = null;
			if(!this.map.TryGetValue(cp, out pi))
			{
				SqlParameter p;
				if(this.timeProviderType == null)
				{
					p = new SqlParameter(cp.ClrType, cp.SqlType, this.parameterizer.CreateParameterName(), cp.SourceExpression);
					pi = new SqlParameterInfo(p, cp.Value);
				}
				else
				{
					p = new SqlParameter(cp.ClrType, this.timeProviderType, this.parameterizer.CreateParameterName(), cp.SourceExpression);
					pi = new SqlParameterInfo(p, ((DateTime)cp.Value).TimeOfDay);
				}
				this.map.Add(cp, pi);
				this.currentParams.Add(pi);
			}
			return pi.Parameter;
		}

		internal override SqlExpression VisitBinaryOperator(SqlBinary bo)
		{
			//
			// Special case to allow DateTime CLR type to be passed as a paramater where
			// a SQL type TIME is expected. We do this only for the equality/inequality
			// comparisons.
			//
			switch(bo.NodeType)
			{
				case SqlNodeType.EQ:
				case SqlNodeType.EQ2V:
				case SqlNodeType.NE:
				case SqlNodeType.NE2V:
				{
					SqlDbType leftSqlDbType = ((SqlType)(bo.Left.SqlType)).SqlDbType;
					SqlDbType rightSqlDbType = ((SqlType)(bo.Right.SqlType)).SqlDbType;
					if(leftSqlDbType == rightSqlDbType)
						break;

					bool isLeftColRef = bo.Left is SqlColumnRef;
					bool isRightColRef = bo.Right is SqlColumnRef;
					if(isLeftColRef == isRightColRef)
						break;

					if(isLeftColRef && leftSqlDbType == SqlDbType.Time && bo.Right.ClrType == typeof(DateTime))
						this.timeProviderType = bo.Left.SqlType;
					else if(isRightColRef && rightSqlDbType == SqlDbType.Time && bo.Left.ClrType == typeof(DateTime))
						this.timeProviderType = bo.Left.SqlType;
					break;
				}
			}
			base.VisitBinaryOperator(bo);
			return bo;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			bool saveTop = this.topLevel;
			this.topLevel = false;
			@select = this.VisitSelectCore(@select);
			this.topLevel = saveTop;
			@select.Selection = this.VisitExpression(@select.Selection);
			return @select;
		}

		internal override SqlUserQuery VisitUserQuery(SqlUserQuery suq)
		{
			bool saveTop = this.topLevel;
			this.topLevel = false;
			for(int i = 0, n = suq.Arguments.Count; i < n; i++)
			{
				suq.Arguments[i] = this.VisitParameter(suq.Arguments[i]);
			}
			this.topLevel = saveTop;
			suq.Projection = this.VisitExpression(suq.Projection);
			return suq;
		}

		[SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Justification = "Unknown reason.")]
		internal SqlExpression VisitParameter(SqlExpression expr)
		{
			SqlExpression result = this.VisitExpression(expr);
			switch(result.NodeType)
			{
				case SqlNodeType.Parameter:
					return (SqlParameter)result;
				case SqlNodeType.Value:
					// force even literal values to become parameters
					return this.InsertLookup((SqlValue)result);
				default:
					Diagnostics.Debug.Assert(false);
					return result;
			}
		}

		internal override SqlStoredProcedureCall VisitStoredProcedureCall(SqlStoredProcedureCall spc)
		{
			this.VisitUserQuery(spc);

			for(int i = 0, n = spc.Function.Parameters.Count; i < n; i++)
			{
				MetaParameter mp = spc.Function.Parameters[i];
				SqlParameter arg = spc.Arguments[i] as SqlParameter;
				if(arg != null)
				{
					arg.Direction = this.GetParameterDirection(mp);
					if(arg.Direction == ParameterDirection.InputOutput ||
					   arg.Direction == ParameterDirection.Output)
					{
						// Text, NText and Image parameters cannot be used as output parameters
						// so we retype them if necessary.
						RetypeOutParameter(arg);
					}
				}
			}

			// add default return value 
			SqlParameter p = new SqlParameter(typeof(int?), this.parameterizer.TypeProvider.From(typeof(int)), "@RETURN_VALUE", spc.SourceExpression);
			p.Direction = Data.ParameterDirection.Output;
			this.currentParams.Add(new SqlParameterInfo(p));

			return spc;
		}

		private bool RetypeOutParameter(SqlParameter node)
		{
			if(!node.SqlType.IsLargeType)
			{
				return false;
			}
			ProviderType newType = this.parameterizer.TypeProvider.GetBestLargeType(node.SqlType);
			if(node.SqlType != newType)
			{
				node.SetSqlType(newType);
				return true;
			}
			// Since we are dealing with a long out parameter that hasn't been
			// retyped, we need to annotate
			this.parameterizer.Annotations.Add(
											   node,
				new SqlServerCompatibilityAnnotation(
					Strings.MaxSizeNotSupported(node.SourceExpression), SqlServerProviderMode.Sql2000));
			return false;
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private Data.ParameterDirection GetParameterDirection(MetaParameter p)
		{
			if(p.Parameter.IsRetval)
			{
				return Data.ParameterDirection.ReturnValue;
			}
			if(p.Parameter.IsOut)
			{
				return Data.ParameterDirection.Output;
			}
			if(p.Parameter.ParameterType.IsByRef)
			{
				return Data.ParameterDirection.InputOutput;
			}
			return Data.ParameterDirection.Input;
		}


		internal override SqlStatement VisitInsert(SqlInsert sin)
		{
			bool saveTop = this.topLevel;
			this.topLevel = false;
			base.VisitInsert(sin);
			this.topLevel = saveTop;
			return sin;
		}

		internal override SqlStatement VisitUpdate(SqlUpdate sup)
		{
			bool saveTop = this.topLevel;
			this.topLevel = false;
			base.VisitUpdate(sup);
			this.topLevel = saveTop;
			return sup;
		}

		internal override SqlStatement VisitDelete(SqlDelete sd)
		{
			bool saveTop = this.topLevel;
			this.topLevel = false;
			base.VisitDelete(sd);
			this.topLevel = saveTop;
			return sd;
		}

		internal override SqlExpression VisitValue(SqlValue value)
		{
			if(this.topLevel || !value.IsClientSpecified || !value.SqlType.CanBeParameter)
			{
				return value;
			}
			return this.InsertLookup(value);
		}


		internal override SqlExpression VisitClientParameter(SqlClientParameter cp)
		{
			if(cp.SqlType.CanBeParameter)
			{
				SqlParameter p = new SqlParameter(cp.ClrType, cp.SqlType, this.parameterizer.CreateParameterName(), cp.SourceExpression);
				this.currentParams.Add(new SqlParameterInfo(p, cp.Accessor.Compile()));
				return p;
			}
			return cp;
		}


		#region Property Declarations
		internal List<SqlParameterInfo> CurrentParams
		{
			get { return this.currentParams; }
		}
		#endregion
	}
}