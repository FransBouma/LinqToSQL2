using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// convert multiset & element expressions into separate queries
	/// </summary>
	internal class MultiSetMultiPlexer : SqlVisitor
	{
		#region Member Declarations
		SqlMultiplexerOptionType options;
		NodeFactory sql;
		SqlSelect outerSelect;
		bool hasBigJoin;
		bool canJoin;
		bool isTopLevel;
		IEnumerable<SqlParameter> parentParameters;
		#endregion

		internal MultiSetMultiPlexer(SqlMultiplexerOptionType options, IEnumerable<SqlParameter> parentParameters, NodeFactory sqlFactory)
		{
			this.options = options;
			this.sql = sqlFactory;
			this.canJoin = true;
			this.isTopLevel = true;
			this.parentParameters = parentParameters;
		}

		internal override SqlExpression VisitMultiset(SqlSubSelect sms)
		{
			// allow one big-join per query?
			if((this.options & SqlMultiplexerOptionType.EnableBigJoin) != 0 &&
			   !this.hasBigJoin && this.canJoin && this.isTopLevel && this.outerSelect != null
			   && !MultisetChecker.HasMultiset(sms.Select.Selection)
			   && BigJoinChecker.CanBigJoin(sms.Select))
			{

				sms.Select = this.VisitSelect(sms.Select);

				SqlAlias alias = new SqlAlias(sms.Select);
				SqlJoin join = new SqlJoin(SqlJoinType.OuterApply, this.outerSelect.From, alias, null, sms.SourceExpression);
				this.outerSelect.From = @join;
				this.outerSelect.OrderingType = SqlOrderingType.Always;

				// make joined expression
				SqlExpression expr = (SqlExpression)SqlDuplicator.Copy(sms.Select.Selection);

				// make count expression
				SqlSelect copySelect = (SqlSelect)SqlDuplicator.Copy(sms.Select);
				SqlAlias copyAlias = new SqlAlias(copySelect);
				SqlSelect countSelect = new SqlSelect(sql.Unary(SqlNodeType.Count, null, sms.SourceExpression), copyAlias, sms.SourceExpression);
				countSelect.OrderingType = SqlOrderingType.Never;
				SqlExpression count = sql.SubSelect(SqlNodeType.ScalarSubSelect, countSelect);

				// make joined collection
				SqlJoinedCollection jc = new SqlJoinedCollection(sms.ClrType, sms.SqlType, expr, count, sms.SourceExpression);
				this.hasBigJoin = true;
				return jc;
			}
			return QueryExtractor.Extract(sms, this.parentParameters);
		}


		internal override SqlExpression VisitElement(SqlSubSelect elem)
		{
			return QueryExtractor.Extract(elem, this.parentParameters);
		}

		internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss)
		{
			bool saveIsTopLevel = this.isTopLevel;
			this.isTopLevel = false;
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitScalarSubSelect(ss);
			}
			finally
			{
				this.isTopLevel = saveIsTopLevel;
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlExpression VisitExists(SqlSubSelect ss)
		{
			bool saveIsTopLevel = this.isTopLevel;
			this.isTopLevel = false;
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitExists(ss);
			}
			finally
			{
				this.isTopLevel = saveIsTopLevel;
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			SqlSelect saveSelect = this.outerSelect;
			this.outerSelect = select;

			// big-joins may need to lift PK's out for default ordering, so don't allow big-join if we see these
			this.canJoin &= select.GroupBy.Count == 0 && select.Top == null && !select.IsDistinct;

			bool saveIsTopLevel = this.isTopLevel;
			this.isTopLevel = false;

			select = this.VisitSelectCore(select);

			this.isTopLevel = saveIsTopLevel;
			select.Selection = this.VisitExpression(select.Selection);

			this.isTopLevel = saveIsTopLevel;
			this.outerSelect = saveSelect;

			if(select.IsDistinct && HierarchyChecker.HasHierarchy(select.Selection))
			{
				// distinct across heirarchy is a NO-OP
				select.IsDistinct = false;
			}
			return select;
		}

		internal override SqlNode VisitUnion(SqlUnion su)
		{
			this.canJoin = false;
			return base.VisitUnion(su);
		}

		internal override SqlExpression VisitClientCase(SqlClientCase c)
		{
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitClientCase(c);
			}
			finally
			{
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlExpression VisitSimpleCase(SqlSimpleCase c)
		{
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitSimpleCase(c);
			}
			finally
			{
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlExpression VisitSearchedCase(SqlSearchedCase c)
		{
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitSearchedCase(c);
			}
			finally
			{
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlExpression VisitTypeCase(SqlTypeCase tc)
		{
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitTypeCase(tc);
			}
			finally
			{
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlExpression VisitOptionalValue(SqlOptionalValue sov)
		{
			bool saveCanJoin = this.canJoin;
			this.canJoin = false;
			try
			{
				return base.VisitOptionalValue(sov);
			}
			finally
			{
				this.canJoin = saveCanJoin;
			}
		}

		internal override SqlUserQuery VisitUserQuery(SqlUserQuery suq)
		{
			this.canJoin = false;
			return base.VisitUserQuery(suq);
		}
	}
}