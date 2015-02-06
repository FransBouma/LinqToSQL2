using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SubSelectDuplicator : DuplicatingVisitor
	{
		List<SqlExpression> externals;
		List<SqlParameter> parameters;

		internal SubSelectDuplicator(List<SqlExpression> externals, List<SqlParameter> parameters)
			: base(true)
		{
			this.externals = externals;
			this.parameters = parameters;
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			SqlExpression result = base.VisitColumnRef(cref);
			if(result == cref)
			{ // must be external
				return ExtractParameter(result);
			}
			return result;
		}

		internal override SqlExpression VisitUserColumn(SqlUserColumn suc)
		{
			SqlExpression result = base.VisitUserColumn(suc);
			if(result == suc)
			{ // must be external
				return ExtractParameter(result);
			}
			return result;
		}

		private SqlExpression ExtractParameter(SqlExpression expr)
		{
			Type clrType = expr.ClrType;
			if(expr.ClrType.IsValueType && !TypeSystem.IsNullableType(expr.ClrType))
			{
				clrType = typeof(Nullable<>).MakeGenericType(expr.ClrType);
			}
			this.externals.Add(expr);
			SqlParameter sp = new SqlParameter(clrType, expr.SqlType, "@x" + (this.parameters.Count + 1), expr.SourceExpression);
			this.parameters.Add(sp);
			return sp;
		}

		internal override SqlNode VisitLink(SqlLink link)
		{
			// Don't visit the Expression/Expansion for this link.
			// Any additional external refs in these expressions
			// should be ignored
			SqlExpression[] exprs = new SqlExpression[link.KeyExpressions.Count];
			for(int i = 0, n = exprs.Length; i < n; i++)
			{
				exprs[i] = this.VisitExpression(link.KeyExpressions[i]);
			}
			return new SqlLink(new object(), link.RowType, link.ClrType, link.SqlType, null, link.Member, exprs, null, link.SourceExpression);
		}
	}
}