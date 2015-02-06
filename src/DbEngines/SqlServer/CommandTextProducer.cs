using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using SqlParameter = System.Data.SqlClient.SqlParameter;

namespace System.Data.Linq.DbEngines.SqlServer
{
	[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
	internal class CommandTextProducer : SqlVisitor
	{
		private StringBuilder _commandStringBuilder;
		private bool _isDebugMode;
		private List<SqlSource> _suppressedAliases = new List<SqlSource>();
		private Dictionary<SqlNode, string> _names = new Dictionary<SqlNode, string>();
		private Dictionary<SqlColumn, SqlAlias> _aliasMap = new Dictionary<SqlColumn, SqlAlias>();
		private int _depth;
		private bool _parenthesizeTop;

		internal string Format(SqlNode node, bool isDebug)
		{
			_commandStringBuilder = new StringBuilder();
			_isDebugMode = isDebug;
			_aliasMap.Clear();
			if(isDebug)
			{
				new AliasMapper(_aliasMap).Visit(node);
			}
			this.Visit(node);
			return _commandStringBuilder.ToString();
		}

		internal string Format(SqlNode node)
		{
			return this.Format(node, false);
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal virtual void VisitWithParens(SqlNode node, SqlNode outer)
		{
			if(node == null)
				return;
			switch(node.NodeType)
			{
				case SqlNodeType.ColumnRef:
				case SqlNodeType.Value:
				case SqlNodeType.Member:
				case SqlNodeType.Parameter:
				case SqlNodeType.FunctionCall:
				case SqlNodeType.TableValuedFunctionCall:
				case SqlNodeType.OuterJoinedValue:
					this.Visit(node);
					break;
				case SqlNodeType.Add:
				case SqlNodeType.Mul:
				case SqlNodeType.And:
				case SqlNodeType.Or:
				case SqlNodeType.Not:
				case SqlNodeType.Not2V:
				case SqlNodeType.BitAnd:
				case SqlNodeType.BitOr:
				case SqlNodeType.BitXor:
				case SqlNodeType.BitNot:
					if(outer.NodeType != node.NodeType)
						goto default;
					this.Visit(node);
					break;

				default:
					_commandStringBuilder.Append("(");
					this.Visit(node);
					_commandStringBuilder.Append(")");
					break;
			}
		}

		internal override SqlExpression VisitNop(SqlNop nop)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("NOP");
			}
			_commandStringBuilder.Append("NOP()");
			return nop;
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override SqlExpression VisitUnaryOperator(SqlUnary uo)
		{
			switch(uo.NodeType)
			{
				case SqlNodeType.Not:
				case SqlNodeType.Not2V:
					_commandStringBuilder.Append(GetOperator(uo.NodeType));
					_commandStringBuilder.Append(" ");
					this.VisitWithParens(uo.Operand, uo);
					break;
				case SqlNodeType.Negate:
				case SqlNodeType.BitNot:
					_commandStringBuilder.Append(GetOperator(uo.NodeType));
					this.VisitWithParens(uo.Operand, uo);
					break;
				case SqlNodeType.Count:
				case SqlNodeType.LongCount:
				case SqlNodeType.Max:
				case SqlNodeType.Min:
				case SqlNodeType.Sum:
				case SqlNodeType.Avg:
				case SqlNodeType.Stddev:
				case SqlNodeType.ClrLength:
				{
					_commandStringBuilder.Append(GetOperator(uo.NodeType));
					_commandStringBuilder.Append("(");
					if(uo.Operand == null)
					{
						_commandStringBuilder.Append("*");
					}
					else
					{
						this.Visit(uo.Operand);
					}
					_commandStringBuilder.Append(")");
					break;
				}
				case SqlNodeType.IsNull:
				case SqlNodeType.IsNotNull:
				{
					this.VisitWithParens(uo.Operand, uo);
					_commandStringBuilder.Append(" ");
					_commandStringBuilder.Append(GetOperator(uo.NodeType));
					break;
				}
				case SqlNodeType.Convert:
				{
					_commandStringBuilder.Append("CONVERT(");
					QueryFormatOptions options = QueryFormatOptions.None;
					if(uo.Operand.SqlType.CanSuppressSizeForConversionToString)
					{
						options = QueryFormatOptions.SuppressSize;
					}
					_commandStringBuilder.Append(uo.SqlType.ToQueryString(options));
					_commandStringBuilder.Append(",");
					this.Visit(uo.Operand);
					_commandStringBuilder.Append(")");
					break;
				}
				case SqlNodeType.ValueOf:
				case SqlNodeType.OuterJoinedValue:
					this.Visit(uo.Operand); // no op
					break;
				default:
					throw Error.InvalidFormatNode(uo.NodeType);
			}
			return uo;
		}

		internal override SqlRowNumber VisitRowNumber(SqlRowNumber rowNumber)
		{
			_commandStringBuilder.Append("ROW_NUMBER() OVER (ORDER BY ");

			for(int i = 0, n = rowNumber.OrderBy.Count; i < n; i++)
			{
				SqlOrderExpression exp = rowNumber.OrderBy[i];

				if(i > 0) _commandStringBuilder.Append(", ");

				this.Visit(exp.Expression);

				if(exp.OrderType == SqlOrderType.Descending)
				{
					_commandStringBuilder.Append(" DESC");
				}
			}

			_commandStringBuilder.Append(")");

			return rowNumber;
		}

		internal override SqlExpression VisitLift(SqlLift lift)
		{
			this.Visit(lift.Expression);
			return lift;
		}

		internal override SqlExpression VisitBinaryOperator(SqlBinary bo)
		{
			switch(bo.NodeType)
			{
				case SqlNodeType.Coalesce:
					_commandStringBuilder.Append("COALESCE(");
					this.Visit(bo.Left);
					_commandStringBuilder.Append(",");
					this.Visit(bo.Right);
					_commandStringBuilder.Append(")");
					break;
				default:
					this.VisitWithParens(bo.Left, bo);
					_commandStringBuilder.Append(" ");
					_commandStringBuilder.Append(GetOperator(bo.NodeType));
					_commandStringBuilder.Append(" ");
					this.VisitWithParens(bo.Right, bo);
					break;
			}
			return bo;
		}

		internal override SqlExpression VisitBetween(SqlBetween between)
		{
			this.VisitWithParens(between.Expression, between);
			_commandStringBuilder.Append(" BETWEEN ");
			this.Visit(between.Start);
			_commandStringBuilder.Append(" AND ");
			this.Visit(between.End);
			return between;
		}

		internal override SqlExpression VisitIn(SqlIn sin)
		{
			this.VisitWithParens(sin.Expression, sin);
			_commandStringBuilder.Append(" IN (");
			for(int i = 0, n = sin.Values.Count; i < n; i++)
			{
				if(i > 0)
				{
					_commandStringBuilder.Append(", ");
				}
				this.Visit(sin.Values[i]);
			}
			_commandStringBuilder.Append(")");
			return sin;
		}

		internal override SqlExpression VisitLike(SqlLike like)
		{
			this.VisitWithParens(like.Expression, like);
			_commandStringBuilder.Append(" LIKE ");
			this.Visit(like.Pattern);
			if(like.Escape != null)
			{
				_commandStringBuilder.Append(" ESCAPE ");
				this.Visit(like.Escape);
			}
			return like;
		}

		internal override SqlExpression VisitFunctionCall(SqlFunctionCall fc)
		{
			if(fc.Name.Contains("."))
			{
				// Assume UDF -- bracket the name.
				this.WriteName(fc.Name);
			}
			else
			{
				// No ".", so we assume it's a system function name and leave it alone.
				_commandStringBuilder.Append(fc.Name);
			}

			_commandStringBuilder.Append("(");
			for(int i = 0, n = fc.Arguments.Count; i < n; i++)
			{
				if(i > 0)
					_commandStringBuilder.Append(", ");
				this.Visit(fc.Arguments[i]);
			}
			_commandStringBuilder.Append(")");
			return fc;
		}

		internal override SqlExpression VisitTableValuedFunctionCall(SqlTableValuedFunctionCall fc)
		{
			// both scalar and table valued functions are formatted the same
			return VisitFunctionCall(fc);
		}

		internal override SqlExpression VisitCast(SqlUnary c)
		{
			_commandStringBuilder.Append("CAST(");
			this.Visit(c.Operand);
			_commandStringBuilder.Append(" AS ");
			QueryFormatOptions options = QueryFormatOptions.None;
			if(c.Operand.SqlType.CanSuppressSizeForConversionToString)
			{
				options = QueryFormatOptions.SuppressSize;
			}
			_commandStringBuilder.Append(c.SqlType.ToQueryString(options));
			_commandStringBuilder.Append(")");
			return c;
		}

		internal override SqlExpression VisitTreat(SqlUnary t)
		{
			_commandStringBuilder.Append("TREAT(");
			this.Visit(t.Operand);
			_commandStringBuilder.Append(" AS ");
			this.FormatType(t.SqlType);
			_commandStringBuilder.Append(")");
			return t;
		}

		internal override SqlExpression VisitColumn(SqlColumn c)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("Column");
			}
			_commandStringBuilder.Append("COLUMN(");
			if(c.Expression != null)
			{
				this.Visit(c.Expression);
			}
			else
			{
				string aliasName = null;
				if(c.Alias != null)
				{
					if(c.Alias.Name == null)
					{
						if(!_names.TryGetValue(c.Alias, out aliasName))
						{
							aliasName = "A" + _names.Count;
							_names[c.Alias] = aliasName;
						}
					}
					else
					{
						aliasName = c.Alias.Name;
					}
				}
				_commandStringBuilder.Append(aliasName);
				_commandStringBuilder.Append(".");
				_commandStringBuilder.Append(c.Name);
			}
			_commandStringBuilder.Append(")");
			return c;
		}

		internal override SqlExpression VisitDiscriminatedType(SqlDiscriminatedType dt)
		{
			if(_isDebugMode)
			{
				_commandStringBuilder.Append("DTYPE(");
			}
			base.VisitDiscriminatedType(dt);
			if(_isDebugMode)
			{
				_commandStringBuilder.Append(")");
			}
			return dt;
		}

		internal override SqlExpression VisitDiscriminatorOf(SqlDiscriminatorOf dof)
		{
			if(_isDebugMode)
			{
				_commandStringBuilder.Append("DISCO(");
			}
			base.VisitDiscriminatorOf(dof);
			if(_isDebugMode)
			{
				_commandStringBuilder.Append(")");
			}
			return dof;
		}

		internal override SqlExpression VisitSimpleExpression(SqlSimpleExpression simple)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("SIMPLE");
			}
			_commandStringBuilder.Append("SIMPLE(");
			base.VisitSimpleExpression(simple);
			_commandStringBuilder.Append(")");
			return simple;
		}

		internal override SqlExpression VisitSharedExpression(SqlSharedExpression shared)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("Shared");
			}
			_commandStringBuilder.Append("SHARED(");
			this.Visit(shared.Expression);
			_commandStringBuilder.Append(")");
			return shared;
		}

		internal override SqlExpression VisitSharedExpressionRef(SqlSharedExpressionRef sref)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("SharedRef");
			}
			_commandStringBuilder.Append("SHAREDREF(");
			this.Visit(sref.SharedExpression.Expression);
			_commandStringBuilder.Append(")");
			return sref;
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			string aliasName = null;
			SqlColumn c = cref.Column;
			SqlAlias alias = c.Alias;
			if(alias == null)
			{
				_aliasMap.TryGetValue(c, out alias);
			}
			if(alias != null)
			{
				if(alias.Name == null)
				{
					if(!_names.TryGetValue(alias, out aliasName))
					{
						aliasName = "A" + _names.Count;
						_names[c.Alias] = aliasName;
					}
				}
				else
				{
					aliasName = c.Alias.Name;
				}
			}
			if(!_suppressedAliases.Contains(c.Alias) && aliasName != null && aliasName.Length != 0)
			{
				this.WriteName(aliasName);
				_commandStringBuilder.Append(".");
			}
			string name = c.Name;
			string inferredName = this.InferName(c.Expression, null);
			if(name == null)
				name = inferredName;
			if(name == null)
			{
				if(!_names.TryGetValue(c, out name))
				{
					name = "C" + _names.Count;
					_names[c] = name;
				}
			}
			this.WriteName(name);
			return cref;
		}

		internal virtual void WriteName(string s)
		{
			_commandStringBuilder.Append(SqlIdentifier.QuoteCompoundIdentifier(s));
		}

		internal virtual void WriteVariableName(string s)
		{
			if(s.StartsWith("@", StringComparison.Ordinal))
				_commandStringBuilder.Append(SqlIdentifier.QuoteCompoundIdentifier(s));
			else
				_commandStringBuilder.Append(SqlIdentifier.QuoteCompoundIdentifier("@" + s));
		}

		internal override SqlExpression VisitParameter(Provider.NodeTypes.SqlParameter p)
		{
			_commandStringBuilder.Append(p.Name);
			return p;
		}

		internal override SqlExpression VisitValue(SqlValue value)
		{
			if(value.IsClientSpecified && !_isDebugMode)
			{
				throw Error.InvalidFormatNode("Value");
			}
			this.FormatValue(value.Value);
			return value;
		}

		internal override SqlExpression VisitClientParameter(SqlClientParameter cp)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("ClientParameter");
			}
			else
			{
				_commandStringBuilder.Append("client-parameter(");
				object value;
				try
				{
					value = cp.Accessor.Compile().DynamicInvoke(new object[] { null });
				}
				catch(Reflection.TargetInvocationException e)
				{
					throw e.InnerException;
				}

				_commandStringBuilder.Append(value);
				_commandStringBuilder.Append(")");
			}
			return cp;
		}

		internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss)
		{
			int saveDepth = _depth;
			_depth++;
			if(_isDebugMode)
			{
				_commandStringBuilder.Append("SCALAR");
			}
			_commandStringBuilder.Append("(");
			this.NewLine();
			this.Visit(ss.Select);
			this.NewLine();
			_commandStringBuilder.Append(")");
			_depth = saveDepth;
			return ss;
		}

		internal override SqlExpression VisitElement(SqlSubSelect elem)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("Element");
			}
			int saveDepth = _depth;
			_depth++;
			_commandStringBuilder.Append("ELEMENT(");
			this.NewLine();
			this.Visit(elem.Select);
			this.NewLine();
			_commandStringBuilder.Append(")");
			_depth = saveDepth;
			return elem;
		}

		internal override SqlExpression VisitMultiset(SqlSubSelect sms)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("Multiset");
			}
			int saveDepth = _depth;
			_depth++;
			_commandStringBuilder.Append("MULTISET(");
			this.NewLine();
			this.Visit(sms.Select);
			this.NewLine();
			_commandStringBuilder.Append(")");
			_depth = saveDepth;
			return sms;
		}

		internal override SqlExpression VisitExists(SqlSubSelect sqlExpr)
		{
			int saveDepth = _depth;
			_depth++;
			_commandStringBuilder.Append("EXISTS(");
			this.NewLine();
			this.Visit(sqlExpr.Select);
			this.NewLine();
			_commandStringBuilder.Append(")");
			_depth = saveDepth;
			return sqlExpr;
		}

		internal override SqlTable VisitTable(SqlTable tab)
		{
			string name = tab.Name;
			this.WriteName(name);
			return tab;
		}

		internal override SqlUserQuery VisitUserQuery(SqlUserQuery suq)
		{
			if(suq.Arguments.Count > 0)
			{
				// compute all the arg values...
				StringBuilder savesb = _commandStringBuilder;
				_commandStringBuilder = new StringBuilder();
				object[] args = new object[suq.Arguments.Count];
				for(int i = 0, n = args.Length; i < n; i++)
				{
					this.Visit(suq.Arguments[i]);
					args[i] = _commandStringBuilder.ToString();
					_commandStringBuilder.Length = 0;
				}
				_commandStringBuilder = savesb;
				// append query with args...
				_commandStringBuilder.Append(String.Format(CultureInfo.InvariantCulture, suq.QueryText, args));
			}
			else
			{
				_commandStringBuilder.Append(suq.QueryText);
			}
			return suq;
		}

		internal override SqlExpression VisitUserColumn(SqlUserColumn suc)
		{
			_commandStringBuilder.Append(suc.Name);
			return suc;
		}

		internal override SqlStoredProcedureCall VisitStoredProcedureCall(SqlStoredProcedureCall spc)
		{
			_commandStringBuilder.Append("EXEC @RETURN_VALUE = ");
			this.WriteName(spc.Function.MappedName);
			_commandStringBuilder.Append(" ");

			int pc = spc.Function.Parameters.Count;
			Diagnostics.Debug.Assert(spc.Arguments.Count >= pc);

			for(int i = 0; i < pc; i++)
			{
				MetaParameter mp = spc.Function.Parameters[i];
				if(i > 0) _commandStringBuilder.Append(", ");
				this.WriteVariableName(mp.MappedName);
				_commandStringBuilder.Append(" = ");
				this.Visit(spc.Arguments[i]);
				if(mp.Parameter.IsOut || mp.Parameter.ParameterType.IsByRef)
					_commandStringBuilder.Append(" OUTPUT");
			}

			if(spc.Arguments.Count > pc)
			{
				if(pc > 0) _commandStringBuilder.Append(", ");
				this.WriteVariableName(spc.Function.ReturnParameter.MappedName);
				_commandStringBuilder.Append(" = ");
				this.Visit(spc.Arguments[pc]);
				_commandStringBuilder.Append(" OUTPUT");
			}

			return spc;
		}

		internal override SqlAlias VisitAlias(SqlAlias alias)
		{
			bool isSelect = alias.Node is SqlSelect;
			int saveDepth = _depth;
			string aliasName = null;
			string name = "";
			SqlTable table = alias.Node as SqlTable;
			if(table != null)
			{
				name = table.Name;
			}
			if(alias.Name == null)
			{
				if(!_names.TryGetValue(alias, out aliasName))
				{
					aliasName = "A" + _names.Count;
					_names[alias] = aliasName;
				}
			}
			else
			{
				aliasName = alias.Name;
			}
			if(isSelect)
			{
				_depth++;
				_commandStringBuilder.Append("(");
				this.NewLine();
			}
			this.Visit(alias.Node);
			if(isSelect)
			{
				this.NewLine();
				_commandStringBuilder.Append(")");
				_depth = saveDepth;
			}
			if(!_suppressedAliases.Contains(alias) && aliasName != null && name != aliasName)
			{
				_commandStringBuilder.Append(" AS ");
				this.WriteName(aliasName);
			}
			return alias;
		}

		internal override SqlExpression VisitAliasRef(SqlAliasRef aref)
		{
			_commandStringBuilder.Append("AREF(");
			this.WriteAliasName(aref.Alias);
			_commandStringBuilder.Append(")");
			return aref;
		}

		private void WriteAliasName(SqlAlias alias)
		{
			string aliasName = null;
			if(alias.Name == null)
			{
				if(!_names.TryGetValue(alias, out aliasName))
				{
					aliasName = "A" + _names.Count;
					_names[alias] = aliasName;
				}
			}
			else
			{
				aliasName = alias.Name;
			}
			this.WriteName(aliasName);
		}

		internal override SqlNode VisitUnion(SqlUnion su)
		{
			_commandStringBuilder.Append("(");
			int saveDepth = _depth;
			_depth++;
			this.NewLine();
			this.Visit(su.Left);
			this.NewLine();
			_commandStringBuilder.Append("UNION");
			if(su.All)
			{
				_commandStringBuilder.Append(" ALL");
			}
			this.NewLine();
			this.Visit(su.Right);
			this.NewLine();
			_commandStringBuilder.Append(")");
			_depth = saveDepth;
			return su;
		}

		internal override SqlExpression VisitExprSet(SqlExprSet xs)
		{
			if(_isDebugMode)
			{
				_commandStringBuilder.Append("ES(");
				for(int i = 0, n = xs.Expressions.Count; i < n; i++)
				{
					if(i > 0)
						_commandStringBuilder.Append(", ");
					this.Visit(xs.Expressions[i]);
				}
				_commandStringBuilder.Append(")");
			}
			else
			{
				// only show the first one
				this.Visit(xs.GetFirstExpression());
			}
			return xs;
		}

		internal override SqlRow VisitRow(SqlRow row)
		{
			for(int i = 0, n = row.Columns.Count; i < n; i++)
			{
				SqlColumn c = row.Columns[i];
				if(i > 0)
					_commandStringBuilder.Append(", ");
				this.Visit(c.Expression);
				string name = c.Name;
				string inferredName = this.InferName(c.Expression, null);
				if(name == null)
					name = inferredName;
				if(name == null)
				{
					if(!_names.TryGetValue(c, out name))
					{
						name = "C" + _names.Count;
						_names[c] = name;
					}
				}
				if(name != inferredName && !String.IsNullOrEmpty(name))
				{
					_commandStringBuilder.Append(" AS ");
					this.WriteName(name);
				}
			}
			return row;
		}

		internal override SqlExpression VisitNew(SqlNew sox)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("New");
			}
			_commandStringBuilder.Append("new ");
			_commandStringBuilder.Append(sox.ClrType.Name);
			_commandStringBuilder.Append("{ ");
			// Visit Args
			for(int i = 0, n = sox.Args.Count; i < n; i++)
			{
				SqlExpression argExpr = sox.Args[i];
				if(i > 0) _commandStringBuilder.Append(", ");
				_commandStringBuilder.Append(sox.ArgMembers[i].Name);
				_commandStringBuilder.Append(" = ");
				this.Visit(argExpr);
			}
			// Visit Members
			for(int i = 0, n = sox.Members.Count; i < n; i++)
			{
				SqlMemberAssign ma = sox.Members[i];
				if(i > 0) _commandStringBuilder.Append(", ");
				string ename = this.InferName(ma.Expression, null);
				if(ename != ma.Member.Name)
				{
					_commandStringBuilder.Append(ma.Member.Name);
					_commandStringBuilder.Append(" = ");
				}
				this.Visit(ma.Expression);
			}
			_commandStringBuilder.Append(" }");
			return sox;
		}

		internal override SqlExpression VisitClientArray(SqlClientArray scar)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("ClientArray");
			}
			_commandStringBuilder.Append("new []{");
			for(int i = 0, n = scar.Expressions.Count; i < n; i++)
			{
				if(i > 0) _commandStringBuilder.Append(", ");
				this.Visit(scar.Expressions[i]);
			}
			_commandStringBuilder.Append("}");
			return scar;
		}

		internal override SqlNode VisitMember(SqlMember m)
		{
			this.Visit(m.Expression);
			_commandStringBuilder.Append(".");
			_commandStringBuilder.Append(m.Member.Name);
			return m;
		}

		internal virtual void NewLine()
		{
			if(_commandStringBuilder.Length > 0)
			{
				_commandStringBuilder.AppendLine();
			}
			for(int i = 0; i < _depth; i++)
			{
				_commandStringBuilder.Append("    ");
			}
		}

		internal override SqlSelect VisitSelect(SqlSelect ss)
		{
			if(ss.DoNotOutput)
			{
				return ss;
			}
			string from = null;
			if(ss.From != null)
			{
				StringBuilder savesb = _commandStringBuilder;
				_commandStringBuilder = new StringBuilder();
				if(this.IsSimpleCrossJoinList(ss.From))
				{
					this.VisitCrossJoinList(ss.From);
				}
				else
				{
					this.Visit(ss.From);
				}
				@from = _commandStringBuilder.ToString();
				_commandStringBuilder = savesb;
			}

			_commandStringBuilder.Append("SELECT ");

			if(ss.IsDistinct)
			{
				_commandStringBuilder.Append("DISTINCT ");
			}

			if(ss.Top != null)
			{
				_commandStringBuilder.Append("TOP ");
				if(_parenthesizeTop)
				{
					_commandStringBuilder.Append("(");
				}
				this.Visit(ss.Top);
				if(_parenthesizeTop)
				{
					_commandStringBuilder.Append(")");
				}
				_commandStringBuilder.Append(" ");
				if(ss.IsPercent)
				{
					_commandStringBuilder.Append(" PERCENT ");
				}
			}

			if(ss.Row.Columns.Count > 0)
			{
				this.VisitRow(ss.Row);
			}
			else if(_isDebugMode)
			{
				this.Visit(ss.Selection);
			}
			else
			{
				_commandStringBuilder.Append("NULL AS [EMPTY]");
			}

			if(@from != null)
			{
				this.NewLine();
				_commandStringBuilder.Append("FROM ");
				_commandStringBuilder.Append(@from);
			}
			if(ss.Where != null)
			{
				this.NewLine();
				_commandStringBuilder.Append("WHERE ");
				this.Visit(ss.Where);
			}
			if(ss.GroupBy.Count > 0)
			{
				this.NewLine();
				_commandStringBuilder.Append("GROUP BY ");
				for(int i = 0, n = ss.GroupBy.Count; i < n; i++)
				{
					SqlExpression exp = ss.GroupBy[i];
					if(i > 0)
						_commandStringBuilder.Append(", ");
					this.Visit(exp);
				}
				if(ss.Having != null)
				{
					this.NewLine();
					_commandStringBuilder.Append("HAVING ");
					this.Visit(ss.Having);
				}
			}
			if(ss.OrderBy.Count > 0 && ss.OrderingType != SqlOrderingType.Never)
			{
				this.NewLine();
				_commandStringBuilder.Append("ORDER BY ");
				for(int i = 0, n = ss.OrderBy.Count; i < n; i++)
				{
					SqlOrderExpression exp = ss.OrderBy[i];
					if(i > 0)
						_commandStringBuilder.Append(", ");
					this.Visit(exp.Expression);
					if(exp.OrderType == SqlOrderType.Descending)
					{
						_commandStringBuilder.Append(" DESC");
					}
				}
			}

			return ss;
		}

		internal virtual bool IsSimpleCrossJoinList(SqlNode node)
		{
			SqlJoin join = node as SqlJoin;
			if(@join != null)
			{
				return @join.JoinType == SqlJoinType.Cross &&
					   this.IsSimpleCrossJoinList(@join.Left) &&
					   this.IsSimpleCrossJoinList(@join.Right);
			}
			SqlAlias alias = node as SqlAlias;
			return (alias != null && alias.Node is SqlTable);
		}

		internal virtual void VisitCrossJoinList(SqlNode node)
		{
			SqlJoin join = node as SqlJoin;
			if(@join != null)
			{
				this.VisitCrossJoinList(@join.Left);
				_commandStringBuilder.Append(", ");
				this.VisitCrossJoinList(@join.Right);
			}
			else
			{
				this.Visit(node);
			}
		}

		internal void VisitJoinSource(SqlSource src)
		{
			if(src.NodeType == SqlNodeType.Join)
			{
				_depth++;
				_commandStringBuilder.Append("(");
				this.Visit(src);
				_commandStringBuilder.Append(")");
				_depth--;
			}
			else
			{
				this.Visit(src);
			}
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			this.Visit(@join.Left);
			this.NewLine();
			switch(@join.JoinType)
			{
				case SqlJoinType.CrossApply:
					_commandStringBuilder.Append("CROSS APPLY ");
					break;
				case SqlJoinType.Cross:
					_commandStringBuilder.Append("CROSS JOIN ");
					break;
				case SqlJoinType.Inner:
					_commandStringBuilder.Append("INNER JOIN ");
					break;
				case SqlJoinType.LeftOuter:
					_commandStringBuilder.Append("LEFT OUTER JOIN ");
					break;
				case SqlJoinType.OuterApply:
					_commandStringBuilder.Append("OUTER APPLY ");
					break;
			}
			SqlJoin rightJoin = @join.Right as SqlJoin;
			if(rightJoin == null ||
			   (rightJoin.JoinType == SqlJoinType.Cross
				&& @join.JoinType != SqlJoinType.CrossApply
				&& @join.JoinType != SqlJoinType.OuterApply))
			{
				this.Visit(@join.Right);
			}
			else
			{
				this.VisitJoinSource(@join.Right);
			}
			if(@join.Condition != null)
			{
				_commandStringBuilder.Append(" ON ");
				this.Visit(@join.Condition);
			}
			else if(this.RequiresOnCondition(@join.JoinType))
			{
				_commandStringBuilder.Append(" ON 1=1 ");
			}
			return @join;
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		internal bool RequiresOnCondition(SqlJoinType joinType)
		{
			switch(joinType)
			{
				case SqlJoinType.CrossApply:
				case SqlJoinType.Cross:
				case SqlJoinType.OuterApply:
					return false;
				case SqlJoinType.Inner:
				case SqlJoinType.LeftOuter:
					return true;
				default:
					throw Error.InvalidFormatNode(joinType);
			}
		}

		internal override SqlBlock VisitBlock(SqlBlock block)
		{
			for(int i = 0, n = block.Statements.Count; i < n; i++)
			{
				this.Visit(block.Statements[i]);
				if(i < n - 1)
				{
					SqlSelect select = block.Statements[i + 1] as SqlSelect;
					if(@select == null || !@select.DoNotOutput)
					{
						this.NewLine();
						this.NewLine();
					}
				}
			}
			return block;
		}

		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("ClientQuery");
			}
			_commandStringBuilder.Append("client(");
			for(int i = 0, n = cq.Arguments.Count; i < n; i++)
			{
				if(i > 0) _commandStringBuilder.Append(", ");
				this.Visit(cq.Arguments[i]);
			}
			_commandStringBuilder.Append("; ");
			this.Visit(cq.Query);
			_commandStringBuilder.Append(")");
			return cq;
		}

		internal override SqlExpression VisitJoinedCollection(SqlJoinedCollection jc)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("JoinedCollection");
			}
			_commandStringBuilder.Append("big-join(");
			this.Visit(jc.Expression);
			_commandStringBuilder.Append(", ");
			this.Visit(jc.Count);
			_commandStringBuilder.Append(")");
			return jc;
		}

		internal override SqlStatement VisitDelete(SqlDelete sd)
		{
			_commandStringBuilder.Append("DELETE FROM ");
			_suppressedAliases.Add(sd.Select.From);
			this.Visit(sd.Select.From);
			if(sd.Select.Where != null)
			{
				_commandStringBuilder.Append(" WHERE ");
				this.Visit(sd.Select.Where);
			}
			_suppressedAliases.Remove(sd.Select.From);
			return sd;
		}

		internal override SqlStatement VisitInsert(SqlInsert si)
		{

			if(si.OutputKey != null)
			{
				_commandStringBuilder.Append("DECLARE @output TABLE(");
				this.WriteName(si.OutputKey.Name);
				_commandStringBuilder.Append(" ");
				_commandStringBuilder.Append(si.OutputKey.SqlType.ToQueryString());
				_commandStringBuilder.Append(")");
				this.NewLine();
				if(si.OutputToLocal)
				{
					_commandStringBuilder.Append("DECLARE @id ");
					_commandStringBuilder.Append(si.OutputKey.SqlType.ToQueryString());
					this.NewLine();
				}
			}

			_commandStringBuilder.Append("INSERT INTO ");
			this.Visit(si.Table);

			if(si.Row.Columns.Count != 0)
			{
				// INSERT INTO table (...columns...) VALUES (...values...)
				_commandStringBuilder.Append("(");
				for(int i = 0, n = si.Row.Columns.Count; i < n; i++)
				{
					if(i > 0) _commandStringBuilder.Append(", ");
					this.WriteName(si.Row.Columns[i].Name);
				}
				_commandStringBuilder.Append(")");
			}

			if(si.OutputKey != null)
			{
				this.NewLine();
				_commandStringBuilder.Append("OUTPUT INSERTED.");
				this.WriteName(si.OutputKey.MetaMember.MappedName);
				_commandStringBuilder.Append(" INTO @output");
			}

			if(si.Row.Columns.Count == 0)
			{
				_commandStringBuilder.Append(" DEFAULT VALUES");
			}
			else
			{
				// VALUES (...values...)
				this.NewLine();
				_commandStringBuilder.Append("VALUES (");
				if(_isDebugMode && si.Row.Columns.Count == 0)
				{
					this.Visit(si.Expression);
				}
				else
				{
					for(int i = 0, n = si.Row.Columns.Count; i < n; i++)
					{
						if(i > 0) _commandStringBuilder.Append(", ");
						this.Visit(si.Row.Columns[i].Expression);
					}
				}
				_commandStringBuilder.Append(")");
			}

			if(si.OutputKey != null)
			{
				this.NewLine();
				if(si.OutputToLocal)
				{
					_commandStringBuilder.Append("SELECT @id = ");
					_commandStringBuilder.Append(si.OutputKey.Name);
					_commandStringBuilder.Append(" FROM @output");
				}
				else
				{
					_commandStringBuilder.Append("SELECT ");
					this.WriteName(si.OutputKey.Name);
					_commandStringBuilder.Append(" FROM @output");
				}
			}

			return si;
		}

		internal override SqlStatement VisitUpdate(SqlUpdate su)
		{
			_commandStringBuilder.Append("UPDATE ");
			_suppressedAliases.Add(su.Select.From);
			this.Visit(su.Select.From);
			this.NewLine();
			_commandStringBuilder.Append("SET ");

			for(int i = 0, n = su.Assignments.Count; i < n; i++)
			{
				if(i > 0) _commandStringBuilder.Append(", ");
				SqlAssign sa = su.Assignments[i];
				this.Visit(sa.LValue);
				_commandStringBuilder.Append(" = ");
				this.Visit(sa.RValue);
			}
			if(su.Select.Where != null)
			{
				this.NewLine();
				_commandStringBuilder.Append("WHERE ");
				this.Visit(su.Select.Where);
			}
			_suppressedAliases.Remove(su.Select.From);
			return su;
		}

		internal override SqlStatement VisitAssign(SqlAssign sa)
		{
			_commandStringBuilder.Append("SET ");
			this.Visit(sa.LValue);
			_commandStringBuilder.Append(" = ");
			this.Visit(sa.RValue);
			return sa;
		}

		internal override SqlExpression VisitSearchedCase(SqlSearchedCase c)
		{
			_depth++;
			this.NewLine();
			_commandStringBuilder.Append("(CASE ");
			_depth++;
			for(int i = 0, n = c.Whens.Count; i < n; i++)
			{
				SqlWhen when = c.Whens[i];
				this.NewLine();
				_commandStringBuilder.Append("WHEN ");
				this.Visit(when.Match);
				_commandStringBuilder.Append(" THEN ");
				this.Visit(when.Value);
			}
			if(c.Else != null)
			{
				this.NewLine();
				_commandStringBuilder.Append("ELSE ");
				this.Visit(c.Else);
			}
			_depth--;
			this.NewLine();
			_commandStringBuilder.Append(" END)");
			_depth--;
			return c;
		}

		internal override SqlExpression VisitSimpleCase(SqlSimpleCase c)
		{
			_depth++;
			this.NewLine();
			_commandStringBuilder.Append("(CASE");
			_depth++;
			if(c.Expression != null)
			{
				_commandStringBuilder.Append(" ");
				this.Visit(c.Expression);
			}
			for(int i = 0, n = c.Whens.Count; i < n; i++)
			{
				SqlWhen when = c.Whens[i];
				if(i == n - 1 && when.Match == null)
				{
					this.NewLine();
					_commandStringBuilder.Append("ELSE ");
					this.Visit(when.Value);
				}
				else
				{
					this.NewLine();
					_commandStringBuilder.Append("WHEN ");
					this.Visit(when.Match);
					_commandStringBuilder.Append(" THEN ");
					this.Visit(when.Value);
				}
			}
			_depth--;
			this.NewLine();
			_commandStringBuilder.Append(" END)");
			_depth--;
			return c;
		}

		internal override SqlExpression VisitClientCase(SqlClientCase c)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("ClientCase");
			}
			_depth++;
			this.NewLine();
			_commandStringBuilder.Append("(CASE");
			_depth++;
			if(c.Expression != null)
			{
				_commandStringBuilder.Append(" ");
				this.Visit(c.Expression);
			}
			for(int i = 0, n = c.Whens.Count; i < n; i++)
			{
				SqlClientWhen when = c.Whens[i];
				if(i == n - 1 && when.Match == null)
				{
					this.NewLine();
					_commandStringBuilder.Append("ELSE ");
					this.Visit(when.Value);
				}
				else
				{
					this.NewLine();
					_commandStringBuilder.Append("WHEN ");
					this.Visit(when.Match);
					_commandStringBuilder.Append(" THEN ");
					this.Visit(when.Value);
				}
			}
			_depth--;
			this.NewLine();
			_commandStringBuilder.Append(" END)");
			_depth--;
			return c;
		}

		internal override SqlExpression VisitTypeCase(SqlTypeCase c)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("TypeCase");
			}
			_depth++;
			this.NewLine();
			_commandStringBuilder.Append("(CASE");
			_depth++;
			if(c.Discriminator != null)
			{
				_commandStringBuilder.Append(" ");
				this.Visit(c.Discriminator);
			}
			for(int i = 0, n = c.Whens.Count; i < n; i++)
			{
				SqlTypeCaseWhen when = c.Whens[i];
				if(i == n - 1 && when.Match == null)
				{
					this.NewLine();
					_commandStringBuilder.Append("ELSE ");
					this.Visit(when.TypeBinding);
				}
				else
				{
					this.NewLine();
					_commandStringBuilder.Append("WHEN ");
					this.Visit(when.Match);
					_commandStringBuilder.Append(" THEN ");
					this.Visit(when.TypeBinding);
				}
			}
			_depth--;
			this.NewLine();
			_commandStringBuilder.Append(" END)");
			_depth--;
			return c;
		}

		internal override SqlExpression VisitVariable(SqlVariable v)
		{
			_commandStringBuilder.Append(v.Name);
			return v;
		}

		private string InferName(SqlExpression exp, string def)
		{
			if(exp == null) return null;
			switch(exp.NodeType)
			{
				case SqlNodeType.Member:
					return ((SqlMember)exp).Member.Name;
				case SqlNodeType.Column:
					return ((SqlColumn)exp).Name;
				case SqlNodeType.ColumnRef:
					return ((SqlColumnRef)exp).Column.Name;
				case SqlNodeType.ExprSet:
					return this.InferName(((SqlExprSet)exp).Expressions[0], def);
				default:
					return def;
			}
		}

		private void FormatType(ProviderType type)
		{
			_commandStringBuilder.Append(type.ToQueryString());
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal virtual void FormatValue(object value)
		{
			if(value == null)
			{
				_commandStringBuilder.Append("NULL");
			}
			else
			{
				Type t = value.GetType();
				if(t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
					t = t.GetGenericArguments()[0];
				TypeCode tc = Type.GetTypeCode(t);
				switch(tc)
				{
					case TypeCode.Char:
					case TypeCode.String:
					case TypeCode.DateTime:
						_commandStringBuilder.Append("'");
						_commandStringBuilder.Append(this.EscapeSingleQuotes(value.ToString()));
						_commandStringBuilder.Append("'");
						return;
					case TypeCode.Boolean:
						_commandStringBuilder.Append(this.GetBoolValue((bool)value));
						return;
					case TypeCode.Byte:
					case TypeCode.Decimal:
					case TypeCode.Double:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.SByte:
					case TypeCode.Single:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:
						_commandStringBuilder.Append(value);
						return;
					case TypeCode.Object:
					{
						if(value is Guid)
						{
							_commandStringBuilder.Append("'");
							_commandStringBuilder.Append(value);
							_commandStringBuilder.Append("'");
							return;
						}
						Type valueType = value as Type;
						if(valueType != null)
						{
							if(_isDebugMode)
							{
								_commandStringBuilder.Append("typeof(");
								_commandStringBuilder.Append(valueType.Name);
								_commandStringBuilder.Append(")");
							}
							else
							{
								this.FormatValue("");
							}
							return;
						}
						break;
					}
				}
				if(_isDebugMode)
				{
					_commandStringBuilder.Append("value(");
					_commandStringBuilder.Append(value.ToString());
					_commandStringBuilder.Append(")");
				}
				else
				{
					throw Error.ValueHasNoLiteralInSql(value);
				}
			}
		}

		internal virtual string GetBoolValue(bool value)
		{
			return value ? "1" : "0";
		}

		internal virtual string EscapeSingleQuotes(string str)
		{
			if(str.IndexOf('\'') < 0) return str;
			StringBuilder tempStringBuilder = new StringBuilder();
			foreach(char c in str)
			{
				if(c == '\'')
				{
					tempStringBuilder.Append("''");
				}
				else
				{
					tempStringBuilder.Append("'");
				}
			}
			return tempStringBuilder.ToString();
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal virtual string GetOperator(SqlNodeType nt)
		{
			switch(nt)
			{
				case SqlNodeType.Add: return "+";
				case SqlNodeType.Sub: return "-";
				case SqlNodeType.Mul: return "*";
				case SqlNodeType.Div: return "/";
				case SqlNodeType.Mod: return "%";
				case SqlNodeType.Concat: return "+";
				case SqlNodeType.BitAnd: return "&";
				case SqlNodeType.BitOr: return "|";
				case SqlNodeType.BitXor: return "^";
				case SqlNodeType.And: return "AND";
				case SqlNodeType.Or: return "OR";
				case SqlNodeType.GE: return ">=";
				case SqlNodeType.GT: return ">";
				case SqlNodeType.LE: return "<=";
				case SqlNodeType.LT: return "<";
				case SqlNodeType.EQ: return "=";
				case SqlNodeType.EQ2V: return "=";
				case SqlNodeType.NE: return "<>";
				case SqlNodeType.NE2V: return "<>";
				case SqlNodeType.Not: return "NOT";
				case SqlNodeType.Not2V: return "NOT";
				case SqlNodeType.BitNot: return "~";
				case SqlNodeType.Negate: return "-";
				case SqlNodeType.IsNull: return "IS NULL";
				case SqlNodeType.IsNotNull: return "IS NOT NULL";
				case SqlNodeType.Count: return "COUNT";
				case SqlNodeType.LongCount: return "COUNT_BIG";
				case SqlNodeType.Min: return "MIN";
				case SqlNodeType.Max: return "MAX";
				case SqlNodeType.Sum: return "SUM";
				case SqlNodeType.Avg: return "AVG";
				case SqlNodeType.Stddev: return "STDEV";
				case SqlNodeType.ClrLength: return "CLRLENGTH";
				default:
					throw Error.InvalidFormatNode(nt);
			}
		}

		internal override SqlNode VisitLink(SqlLink link)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("Link");
			}
			if(link.Expansion != null)
			{
				_commandStringBuilder.Append("LINK(");
				this.Visit(link.Expansion);
				_commandStringBuilder.Append(")");
			}
			else
			{
				_commandStringBuilder.Append("LINK(");
				for(int i = 0, n = link.KeyExpressions.Count; i < n; i++)
				{
					if(i > 0) _commandStringBuilder.Append(", ");
					this.Visit(link.KeyExpressions[i]);
				}
				_commandStringBuilder.Append(")");
			}
			return link;
		}

		internal override SqlMemberAssign VisitMemberAssign(SqlMemberAssign ma)
		{
			throw Error.InvalidFormatNode("MemberAssign");
		}

		internal override SqlExpression VisitMethodCall(SqlMethodCall mc)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("MethodCall");
			}
			if(mc.Method.IsStatic)
			{
				_commandStringBuilder.Append(mc.Method.DeclaringType);
			}
			else
			{
				this.Visit(mc.Object);
			}
			_commandStringBuilder.Append(".");
			_commandStringBuilder.Append(mc.Method.Name);
			_commandStringBuilder.Append("(");
			for(int i = 0, n = mc.Arguments.Count; i < n; i++)
			{
				if(i > 0) _commandStringBuilder.Append(", ");
				this.Visit(mc.Arguments[i]);
			}
			_commandStringBuilder.Append(")");
			return mc;
		}

		internal override SqlExpression VisitOptionalValue(SqlOptionalValue sov)
		{
			if(_isDebugMode)
			{
				_commandStringBuilder.Append("opt(");
				this.Visit(sov.HasValue);
				_commandStringBuilder.Append(", ");
				this.Visit(sov.Value);
				_commandStringBuilder.Append(")");
				return sov;
			}
			else
			{
				throw Error.InvalidFormatNode("OptionalValue");
			}
		}

		internal override SqlExpression VisitUserRow(SqlUserRow row)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("UserRow");
			}
			return row;
		}

		internal override SqlExpression VisitGrouping(SqlGrouping g)
		{
			if(!_isDebugMode)
			{
				throw Error.InvalidFormatNode("Grouping");
			}
			_commandStringBuilder.Append("Group(");
			this.Visit(g.Key);
			_commandStringBuilder.Append(", ");
			this.Visit(g.Group);
			_commandStringBuilder.Append(")");
			return g;
		}


		#region Member Declarations
		internal bool ParenthesizeTop
		{
			get {  return _parenthesizeTop;}
			set { _parenthesizeTop = value; }
		}
		#endregion
	}
}