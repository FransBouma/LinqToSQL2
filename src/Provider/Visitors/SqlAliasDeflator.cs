using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	///  remove redundant/trivial aliases
	/// </summary>
	internal class SqlAliasDeflator : SqlVisitor
	{
		#region Member Declarations
		private Dictionary<SqlAlias, SqlAlias> _removedMap;
		#endregion

		internal SqlAliasDeflator()
		{
			_removedMap = new Dictionary<SqlAlias, SqlAlias>();
		}

		internal override SqlExpression VisitAliasRef(SqlAliasRef aref)
		{
			SqlAlias alias = aref.Alias;
			SqlAlias value;
			if(_removedMap.TryGetValue(alias, out value))
			{
				throw Error.InvalidReferenceToRemovedAliasDuringDeflation();
			}
			return aref;
		}

		internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
		{
			if(cref.Column.Alias != null && _removedMap.ContainsKey(cref.Column.Alias))
			{
				SqlColumnRef c = cref.Column.Expression as SqlColumnRef;
				if(c != null)
				{
					//The following code checks for cases where there are differences between the type returned
					//by a ColumnRef and the column that refers to it.  This situation can occur when conversions
					//are optimized out of the SQL node tree.  As mentioned in the SetClrType comments this is not 
					//an operation that can have adverse effects and should only be used in limited cases, such as
					//this one.
					if(c.ClrType != cref.ClrType)
					{
						c.SetClrType(cref.ClrType);
						return this.VisitColumnRef(c);
					}
				}
				return c;
			}
			return cref;
		}

		internal override SqlSource VisitSource(SqlSource node)
		{
			node = (SqlSource)this.Visit(node);
			SqlAlias alias = node as SqlAlias;
			if(alias != null)
			{
				SqlSelect sel = alias.Node as SqlSelect;
				if(sel != null && this.IsTrivialSelect(sel))
				{
					_removedMap[alias] = alias;
					node = sel.From;
				}
			}
			return node;
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			base.VisitJoin(@join);
			switch(@join.JoinType)
			{
				case SqlJoinType.Cross:
				case SqlJoinType.Inner:
					// reducing either side would effect cardinality of results
					break;
				case SqlJoinType.LeftOuter:
				case SqlJoinType.CrossApply:
				case SqlJoinType.OuterApply:
					// may reduce to left if no references to the right
					if(this.HasEmptySource(@join.Right))
					{
						SqlAlias a = (SqlAlias)@join.Right;
						_removedMap[a] = a;
						return @join.Left;
					}
					break;
			}
			return @join;
		}

		private bool IsTrivialSelect(SqlSelect select)
		{
			if(@select.OrderBy.Count != 0 ||
			   @select.GroupBy.Count != 0 ||
			   @select.Having != null ||
			   @select.Top != null ||
			   @select.IsDistinct ||
			   @select.Where != null)
				return false;
			return this.HasTrivialSource(@select.From) && this.HasTrivialProjection(@select);
		}

		private bool HasTrivialSource(SqlSource node)
		{
			SqlJoin join = node as SqlJoin;
			if(@join != null)
			{
				return this.HasTrivialSource(@join.Left) &&
					   this.HasTrivialSource(@join.Right);
			}
			return node is SqlAlias;
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private bool HasTrivialProjection(SqlSelect select)
		{
			foreach(SqlColumn c in @select.Row.Columns)
			{
				if(c.Expression != null && c.Expression.NodeType != SqlNodeType.ColumnRef)
				{
					return false;
				}
			}
			return true;
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private bool HasEmptySource(SqlSource node)
		{
			SqlAlias alias = node as SqlAlias;
			if(alias == null) return false;
			SqlSelect sel = alias.Node as SqlSelect;
			if(sel == null) return false;
			return sel.Row.Columns.Count == 0 &&
				   sel.From == null &&
				   sel.Where == null &&
				   sel.GroupBy.Count == 0 &&
				   sel.Having == null &&
				   sel.OrderBy.Count == 0;
		}
	}
}