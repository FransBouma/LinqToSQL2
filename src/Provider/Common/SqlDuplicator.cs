using System;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{

	internal class SqlDuplicator
	{
		DuplicatingVisitor superDuper;

		internal SqlDuplicator()
			: this(true)
		{
		}

		internal SqlDuplicator(bool ignoreExternalRefs)
		{
			this.superDuper = new DuplicatingVisitor(ignoreExternalRefs);
		}

		internal static SqlNode Copy(SqlNode node)
		{
			if(node == null)
				return null;
			switch(node.NodeType)
			{
				case SqlNodeType.ColumnRef:
				case SqlNodeType.Value:
				case SqlNodeType.Parameter:
				case SqlNodeType.Variable:
					return node;
				default:
					return new SqlDuplicator().Duplicate(node);
			}
		}

		internal SqlNode Duplicate(SqlNode node)
		{
			return this.superDuper.Visit(node);
		}
	}
}
