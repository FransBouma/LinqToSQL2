using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlAliasRef : SqlExpression {
		private SqlAlias alias;

		internal SqlAliasRef(SqlAlias alias)
			: base(SqlNodeType.AliasRef, GetClrType(alias.Node), alias.SourceExpression) {
			if (alias == null)
				throw Error.ArgumentNull("alias");
			this.alias = alias;
			}

		internal SqlAlias Alias {
			get { return this.alias; }
		}

		internal override ProviderType SqlType {
			get { return GetSqlType(this.alias.Node); }
		}

		private static Type GetClrType(SqlNode node) {
			SqlTableValuedFunctionCall tvf = node as SqlTableValuedFunctionCall;
			if (tvf != null)
				return tvf.RowType.Type;
			SqlExpression exp = node as SqlExpression;
			if (exp != null) {
				if (TypeSystem.IsSequenceType(exp.ClrType))
					return TypeSystem.GetElementType(exp.ClrType);
				return exp.ClrType;
			}
			SqlSelect sel = node as SqlSelect;
			if (sel != null)
				return sel.Selection.ClrType;
			SqlTable tab = node as SqlTable;
			if (tab != null)
				return tab.RowType.Type;
			SqlUnion su = node as SqlUnion;
			if (su != null)
				return su.GetClrType();
			throw Error.UnexpectedNode(node.NodeType);
		}

		private static ProviderType GetSqlType(SqlNode node) {
			SqlExpression exp = node as SqlExpression;
			if (exp != null)
				return exp.SqlType;
			SqlSelect sel = node as SqlSelect;
			if (sel != null)
				return sel.Selection.SqlType;
			SqlTable tab = node as SqlTable;
			if (tab != null)
				return tab.SqlRowType;
			SqlUnion su = node as SqlUnion;
			if (su != null)
				return su.GetSqlType();
			throw Error.UnexpectedNode(node.NodeType);
		}
	}
}