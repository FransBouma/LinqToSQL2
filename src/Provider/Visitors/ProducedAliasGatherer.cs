using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class ProducedAliasGatherer : SqlVisitor
	{
		private HashSet<SqlAlias> _produced = new HashSet<SqlAlias>();

		internal override SqlAlias VisitAlias(SqlAlias a)
		{
			_produced.Add(a);
			return base.VisitAlias(a);
		}

		internal HashSet<SqlAlias> Produced
		{
			get { return _produced; }
		}
	}
}