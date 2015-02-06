using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{

	/// <summary>
	/// Resolves references to columns/expressions defined in other scopes
	/// </summary>
	internal class SqlResolver
	{
		Visitor visitor;

		#region Private Classes
		private class Visitor : SqlScopedVisitor
		{
			SqlBubbler bubbler;

			internal Visitor()
			{
				this.bubbler = new SqlBubbler();
			}

			internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
			{
				SqlColumnRef result = this.BubbleUp(cref);
				if(result == null)
				{
					throw Error.ColumnReferencedIsNotInScope(GetColumnName(cref.Column));
				}
				return result;
			}

			private SqlColumnRef BubbleUp(SqlColumnRef cref)
			{
				for(SqlScope s = this.CurrentScope; s != null; s = s.ContainingScope)
				{
					if(s.Source != null)
					{
						SqlColumn found = this.bubbler.BubbleUp(cref.Column, s.Source);
						if(found != null)
						{
							if(found != cref.Column)
								return new SqlColumnRef(found);
							return cref;
						}
					}
				}
				return null;
			}
		}
		#endregion

		internal SqlResolver()
		{
			this.visitor = new Visitor();
		}

		internal SqlNode Resolve(SqlNode node)
		{
			return this.visitor.Visit(node);
		}

		private static string GetColumnName(SqlColumn c)
		{
#if DEBUG
			return c.Text;
#else
            return c.Name;
#endif
		}
	}
}
