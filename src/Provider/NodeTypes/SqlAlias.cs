namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlAlias : SqlSource {
		private string name;
		private SqlNode node;

		internal SqlAlias(SqlNode node)
			: base(SqlNodeType.Alias, node.SourceExpression) {
			this.Node = node;
			}

		internal string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		internal SqlNode Node {
			get { return this.node; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (!(value is SqlExpression || value is SqlSelect || value is SqlTable || value is SqlUnion))
					throw Error.UnexpectedNode(value.NodeType);
				this.node = value;
			}
		}
	}
}