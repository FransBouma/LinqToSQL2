using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal abstract class SqlExpression : SqlNode {
		private Type clrType;
		internal SqlExpression(SqlNodeType nodeType, Type clrType, Expression sourceExpression)
			: base(nodeType, sourceExpression) {
			this.clrType = clrType;
			}

		internal Type ClrType {
			get { return this.clrType; }
		}

		// note: changing the CLR type of a node is potentially dangerous
		internal void SetClrType(Type type) {
			this.clrType = type;
		}

		internal abstract ProviderType SqlType { get; }

		/// <summary>
		/// Drill down looking for a constant root expression, returning true if found.
		/// </summary>           
		internal bool IsConstantColumn {
			get {
				if (this.NodeType == SqlNodeType.Column) {
					SqlColumn col = (SqlColumn)this;
					if (col.Expression != null) {
						return col.Expression.IsConstantColumn;
					}
				}
				else if (this.NodeType == SqlNodeType.ColumnRef) {
					return ((SqlColumnRef)this).Column.IsConstantColumn;
				}
				else if (this.NodeType == SqlNodeType.OptionalValue) {
					return ((SqlOptionalValue)this).Value.IsConstantColumn;
				}
				else if (this.NodeType == SqlNodeType.Value ||
						 this.NodeType == SqlNodeType.Parameter) {
							 return true;
						 }
				return false;
			}
		}
	}
}