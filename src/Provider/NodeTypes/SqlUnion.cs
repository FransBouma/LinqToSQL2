using System.Data.Linq.Provider.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlUnion : SqlNode {
		private SqlNode left;
		private SqlNode right;
		private bool all;

		internal SqlUnion(SqlNode left, SqlNode right, bool all)
			: base(SqlNodeType.Union, right.SourceExpression) {
			this.Left = left;
			this.Right = right;
			this.All = all;
			}

		internal SqlNode Left {
			get { return this.left; }
			set {
				Validate(value);
				this.left = value;
			}
		}

		internal SqlNode Right {
			get { return this.right; }
			set {
				Validate(value);
				this.right = value;
			}
		}

		internal bool All {
			get { return this.all; }
			set { this.all = value; }
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification="Unknown reason.")]
		private void Validate(SqlNode node) {
			if (node == null)
				throw Error.ArgumentNull("node");
			if (!(node is SqlExpression || node is SqlSelect || node is SqlUnion))
				throw Error.UnexpectedNode(node.NodeType);
		}

		internal Type GetClrType() {
			SqlExpression exp = this.Left as SqlExpression;
			if (exp != null)
				return exp.ClrType;
			SqlSelect sel = this.Left as SqlSelect;
			if (sel != null)
				return sel.Selection.ClrType;
			throw Error.CouldNotGetClrType();
		}

		internal ProviderType GetSqlType() {
			SqlExpression exp = this.Left as SqlExpression;
			if (exp != null)
				return exp.SqlType;
			SqlSelect sel = this.Left as SqlSelect;
			if (sel != null)
				return sel.Selection.SqlType;
			throw Error.CouldNotGetSqlType();
		}
	}
}