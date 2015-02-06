using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlAliasesReferenced
	{
		#region Member Declarations
		private HashSet<SqlAlias> aliases;
		private bool referencesAny;
		private Visitor visitor;
		#endregion

		#region Private Classes
		private class Visitor : SqlVisitor
		{
			SqlAliasesReferenced parent;

			internal Visitor(SqlAliasesReferenced parent)
			{
				this.parent = parent;
			}
			internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
			{
				if(this.parent.aliases.Contains(cref.Column.Alias))
				{
					this.parent.referencesAny = true;
				}
				else if(cref.Column.Expression != null)
				{
					this.Visit(cref.Column.Expression);
				}
				return cref;
			}

			internal override SqlExpression VisitColumn(SqlColumn col)
			{
				if(col.Expression != null)
				{
					this.Visit(col.Expression);
				}
				return col;
			}
		}
		#endregion

		internal SqlAliasesReferenced(HashSet<SqlAlias> aliases)
		{
			this.aliases = aliases;
			this.visitor = new Visitor(this);
		}

		internal bool ReferencesAny(SqlExpression expression)
		{
			this.referencesAny = false;
			this.visitor.Visit(expression);
			return this.referencesAny;
		}
	}
}