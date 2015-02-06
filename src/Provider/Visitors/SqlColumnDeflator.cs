using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Removes unreferenced items in projection list
	/// </summary>
	internal class SqlColumnDeflator : SqlVisitor
	{
		#region Member Declarations
		private Dictionary<SqlNode, SqlNode> _referenceMap;
		private bool _isTopLevel;
		private bool _forceReferenceAll;
		private SqlAggregateChecker _aggregateChecker;
		#endregion

		internal SqlColumnDeflator()
		{
			_referenceMap = new Dictionary<SqlNode, SqlNode>();
			_aggregateChecker = new SqlAggregateChecker();
			_isTopLevel = true;
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			_referenceMap[cref.Column] = cref.Column;
			return cref;
		}

		internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss)
		{
			bool saveIsTopLevel = _isTopLevel;
			_isTopLevel = false;
			bool saveForceReferenceAll = _forceReferenceAll;
			_forceReferenceAll = true;
			try
			{
				return base.VisitScalarSubSelect(ss);
			}
			finally
			{
				_isTopLevel = saveIsTopLevel;
				_forceReferenceAll = saveForceReferenceAll;
			}
		}

		internal override SqlExpression VisitExists(SqlSubSelect ss)
		{
			bool saveIsTopLevel = _isTopLevel;
			_isTopLevel = false;
			try
			{
				return base.VisitExists(ss);
			}
			finally
			{
				_isTopLevel = saveIsTopLevel;
			}
		}

		internal override SqlNode VisitUnion(SqlUnion su)
		{
			bool saveForceReferenceAll = _forceReferenceAll;
			_forceReferenceAll = true;
			su.Left = this.Visit(su.Left);
			su.Right = this.Visit(su.Right);
			_forceReferenceAll = saveForceReferenceAll;
			return su;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			bool saveForceReferenceAll = _forceReferenceAll;
			_forceReferenceAll = false;
			bool saveIsTopLevel = _isTopLevel;

			try
			{
				if(_isTopLevel)
				{
					// top-level projection references columns!
					@select.Selection = this.VisitExpression(@select.Selection);
				}
				_isTopLevel = false;

				for(int i = @select.Row.Columns.Count - 1; i >= 0; i--)
				{
					SqlColumn c = @select.Row.Columns[i];

					bool safeToRemove =
						!saveForceReferenceAll
						&& !_referenceMap.ContainsKey(c)
							// don't remove anything from a distinct select (except maybe a literal value) since it would change the meaning of the comparison
						&& !@select.IsDistinct
							// don't remove an aggregate expression that may be the only expression that forces the grouping (since it would change the cardinality of the results)
						&& !(@select.GroupBy.Count == 0 && _aggregateChecker.HasAggregates(c.Expression));

					if(safeToRemove)
					{
						@select.Row.Columns.RemoveAt(i);
					}
					else
					{
						this.VisitExpression(c.Expression);
					}
				}

				@select.Top = this.VisitExpression(@select.Top);
				for(int i = @select.OrderBy.Count - 1; i >= 0; i--)
				{
					@select.OrderBy[i].Expression = this.VisitExpression(@select.OrderBy[i].Expression);
				}

				@select.Having = this.VisitExpression(@select.Having);
				for(int i = @select.GroupBy.Count - 1; i >= 0; i--)
				{
					@select.GroupBy[i] = this.VisitExpression(@select.GroupBy[i]);
				}

				@select.Where = this.VisitExpression(@select.Where);
				@select.From = this.VisitSource(@select.From);
			}
			finally
			{
				_isTopLevel = saveIsTopLevel;
				_forceReferenceAll = saveForceReferenceAll;
			}

			return @select;
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			@join.Condition = this.VisitExpression(@join.Condition);
			@join.Right = this.VisitSource(@join.Right);
			@join.Left = this.VisitSource(@join.Left);
			return @join;
		}

		internal override SqlNode VisitLink(SqlLink link)
		{
			// don't visit expansion...
			for(int i = 0, n = link.KeyExpressions.Count; i < n; i++)
			{
				link.KeyExpressions[i] = this.VisitExpression(link.KeyExpressions[i]);
			}
			return link;
		}
	}
}