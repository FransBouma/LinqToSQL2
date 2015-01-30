using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Expressions
{
	internal sealed class KnownExpression : InternalExpression {
		SqlNode node;
		internal KnownExpression(SqlNode node, Type type)
			: base(InternalExpressionType.Known, type) {
			this.node = node;
			}
		internal SqlNode Node {
			get { return this.node; }
		}
	}
}