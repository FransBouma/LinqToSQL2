using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// finds location of expression definition and re-projects that value all the
	/// way to the outermost projection
	/// </summary>
	internal class SqlBubbler : SqlVisitor
	{
		#region Member Declarations
		private SqlColumn match;
		private SqlColumn found;
		#endregion

		internal SqlColumn BubbleUp(SqlColumn col, SqlNode source)
		{
			this.match = this.GetOriginatingColumn(col);
			this.found = null;
			this.Visit(source);
			return this.found;
		}

		internal SqlColumn GetOriginatingColumn(SqlColumn col)
		{
			SqlColumnRef cref = col.Expression as SqlColumnRef;
			if(cref != null)
			{
				return this.GetOriginatingColumn(cref.Column);
			}
			return col;
		}

		internal override SqlRow VisitRow(SqlRow row)
		{
			foreach(SqlColumn c in row.Columns)
			{
				if(this.RefersToColumn(c, this.match))
				{
					if(this.found != null)
					{
						throw Error.ColumnIsDefinedInMultiplePlaces(GetColumnName(this.match));
					}
					this.found = c;
					break;
				}
			}
			return row;
		}

		internal override SqlTable VisitTable(SqlTable tab)
		{
			foreach(SqlColumn c in tab.Columns)
			{
				if(c == this.match)
				{
					if(this.found != null)
						throw Error.ColumnIsDefinedInMultiplePlaces(GetColumnName(this.match));
					this.found = c;
					break;
				}
			}
			return tab;
		}

		internal override SqlSource VisitJoin(SqlJoin join)
		{
			switch(join.JoinType)
			{
				case SqlJoinType.CrossApply:
				case SqlJoinType.OuterApply:
				{
					this.Visit(join.Left);
					if(this.found == null)
					{
						this.Visit(join.Right);
					}
					break;
				}
				default:
				{
					this.Visit(join.Left);
					this.Visit(join.Right);
					break;
				}
			}
			return join;
		}

		internal override SqlExpression VisitTableValuedFunctionCall(SqlTableValuedFunctionCall fc)
		{
			foreach(SqlColumn c in fc.Columns)
			{
				if(c == this.match)
				{
					if(this.found != null)
						throw Error.ColumnIsDefinedInMultiplePlaces(GetColumnName(this.match));
					this.found = c;
					break;
				}
			}
			return fc;
		}

		private string GetColumnName(SqlColumn c)
		{
#if DEBUG
			return c.Text;
#else
            return c.Name;
#endif
		}

		private void ForceLocal(SqlRow row, string name)
		{
			bool isLocal = false;
			// check to see if it already exists locally
			foreach(SqlColumn c in row.Columns)
			{
				if(this.RefersToColumn(c, this.found))
				{
					this.found = c;
					isLocal = true;
					break;
				}
			}
			if(!isLocal)
			{
				// need to put this in the local projection list to bubble it up
				SqlColumn c = new SqlColumn(found.ClrType, found.SqlType, name, this.found.MetaMember, new SqlColumnRef(this.found), row.SourceExpression);
				row.Columns.Add(c);
				this.found = c;
			}
		}

		private bool IsFoundInGroup(SqlSelect select)
		{
			// does the column happen to be listed in the group-by clause?
			foreach(SqlExpression exp in select.GroupBy)
			{
				if(this.RefersToColumn(exp, this.found) || this.RefersToColumn(exp, this.match))
				{
					return true;
				}
			}
			return false;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			// look in this projection
			this.Visit(select.Row);

			if(this.found == null)
			{
				// look in upstream projections
				this.Visit(select.From);

				// bubble it up
				if(this.found != null)
				{
					if(select.IsDistinct && !match.IsConstantColumn)
					{
						throw Error.ColumnIsNotAccessibleThroughDistinct(GetColumnName(this.match));
					}
					if(select.GroupBy.Count == 0 || this.IsFoundInGroup(select))
					{
						this.ForceLocal(select.Row, this.found.Name);
					}
					else
					{
						// found it, but its hidden behind the group-by
						throw Error.ColumnIsNotAccessibleThroughGroupBy(GetColumnName(this.match));
					}
				}
			}

			return select;
		}
	}
}