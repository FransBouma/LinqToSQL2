using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ObjectExpressionFlattener : SqlVisitor
	{
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "[....]: part of our standard visitor pattern")]
		NodeFactory sql;
		SqlColumnizer columnizer;
		bool isTopLevel;
		Dictionary<SqlColumn, SqlColumn> map = new Dictionary<SqlColumn, SqlColumn>();

		[SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Justification = "Unknown reason.")]
		internal ObjectExpressionFlattener(NodeFactory sql, SqlColumnizer columnizer)
		{
			this.sql = sql;
			this.columnizer = columnizer;
			this.isTopLevel = true;
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			SqlColumn mapped;
			if(this.map.TryGetValue(cref.Column, out mapped))
			{
				return new SqlColumnRef(mapped);
			}
			return cref;
		}

		internal override SqlSelect VisitSelectCore(SqlSelect select)
		{
			bool saveIsTopLevel = this.isTopLevel;
			this.isTopLevel = false;
			try
			{
				return base.VisitSelectCore(@select);
			}
			finally
			{
				this.isTopLevel = saveIsTopLevel;
			}
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			@select = base.VisitSelect(@select);

			@select.Selection = this.FlattenSelection(@select.Row, false, @select.Selection);

			if(@select.GroupBy.Count > 0)
			{
				this.FlattenGroupBy(@select.GroupBy);
			}

			if(@select.OrderBy.Count > 0)
			{
				this.FlattenOrderBy(@select.OrderBy);
			}

			if(!this.isTopLevel)
			{
				@select.Selection = new SqlNop(@select.Selection.ClrType, @select.Selection.SqlType, @select.SourceExpression);
			}

			return @select;
		}

		internal override SqlStatement VisitInsert(SqlInsert sin)
		{
			base.VisitInsert(sin);
			sin.Expression = this.FlattenSelection(sin.Row, true, sin.Expression);
			return sin;
		}

		private SqlExpression FlattenSelection(SqlRow row, bool isInput, SqlExpression selection)
		{
			selection = this.columnizer.ColumnizeSelection(selection);
			return new SelectionFlattener(row, this.map, isInput).VisitExpression(selection);
		}


		private void FlattenGroupBy(List<SqlExpression> exprs)
		{
			List<SqlExpression> list = new List<SqlExpression>(exprs.Count);
			foreach(SqlExpression gex in exprs)
			{
				if(TypeSystem.IsSequenceType(gex.ClrType))
				{
					throw Error.InvalidGroupByExpressionType(gex.ClrType.Name);
				}
				this.FlattenGroupByExpression(list, gex);
			}
			exprs.Clear();
			exprs.AddRange(list);
		}

		private void FlattenGroupByExpression(List<SqlExpression> exprs, SqlExpression expr)
		{
			SqlNew sn = expr as SqlNew;
			if(sn != null)
			{
				foreach(SqlMemberAssign ma in sn.Members)
				{
					this.FlattenGroupByExpression(exprs, ma.Expression);
				}
				foreach(SqlExpression arg in sn.Args)
				{
					this.FlattenGroupByExpression(exprs, arg);
				}
			}
			else if(expr.NodeType == SqlNodeType.TypeCase)
			{
				SqlTypeCase tc = (SqlTypeCase)expr;
				this.FlattenGroupByExpression(exprs, tc.Discriminator);
				foreach(SqlTypeCaseWhen when in tc.Whens)
				{
					this.FlattenGroupByExpression(exprs, when.TypeBinding);
				}
			}
			else if(expr.NodeType == SqlNodeType.Link)
			{
				SqlLink link = (SqlLink)expr;
				if(link.Expansion != null)
				{
					this.FlattenGroupByExpression(exprs, link.Expansion);
				}
				else
				{
					foreach(SqlExpression key in link.KeyExpressions)
					{
						this.FlattenGroupByExpression(exprs, key);
					}
				}
			}
			else if(expr.NodeType == SqlNodeType.OptionalValue)
			{
				SqlOptionalValue sop = (SqlOptionalValue)expr;
				this.FlattenGroupByExpression(exprs, sop.HasValue);
				this.FlattenGroupByExpression(exprs, sop.Value);
			}
			else if(expr.NodeType == SqlNodeType.OuterJoinedValue)
			{
				this.FlattenGroupByExpression(exprs, ((SqlUnary)expr).Operand);
			}
			else if(expr.NodeType == SqlNodeType.DiscriminatedType)
			{
				SqlDiscriminatedType dt = (SqlDiscriminatedType)expr;
				this.FlattenGroupByExpression(exprs, dt.Discriminator);
			}
			else
			{
				// this expression should have been 'pushed-down' in SqlBinder, so we
				// should only find column-references & expr-sets unless the expression could not
				// be columnized (in which case it was a bad group-by expression.)
				if(expr.NodeType != SqlNodeType.ColumnRef &&
				   expr.NodeType != SqlNodeType.ExprSet)
				{
					if(!expr.SqlType.CanBeColumn)
					{
						throw Error.InvalidGroupByExpressionType(expr.SqlType.ToQueryString());
					}
					throw Error.InvalidGroupByExpression();
				}
				exprs.Add(expr);
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private void FlattenOrderBy(List<SqlOrderExpression> exprs)
		{
			foreach(SqlOrderExpression obex in exprs)
			{
				if(!obex.Expression.SqlType.IsOrderable)
				{
					if(obex.Expression.SqlType.CanBeColumn)
					{
						throw Error.InvalidOrderByExpression(obex.Expression.SqlType.ToQueryString());
					}
					else
					{
						throw Error.InvalidOrderByExpression(obex.Expression.ClrType.Name);
					}
				}
			}
		}
	}
}