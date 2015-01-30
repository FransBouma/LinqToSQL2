using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlColumn : SqlExpression {
		private SqlAlias alias;
		private string name;
		private int ordinal;
		private MetaDataMember member;
		private SqlExpression expression;
		private ProviderType sqlType;

		internal SqlColumn(Type clrType, ProviderType sqlType, string name, MetaDataMember member, SqlExpression expr, Expression sourceExpression)
			: base(SqlNodeType.Column, clrType, sourceExpression) {
			if (typeof(Type).IsAssignableFrom(clrType))
				throw Error.ArgumentWrongValue("clrType");
			this.Name = name;
			this.member = member;
			this.Expression = expr;
			this.Ordinal = -1;
			if (sqlType == null)
				throw Error.ArgumentNull("sqlType");
			this.sqlType = sqlType;
			System.Diagnostics.Debug.Assert(sqlType.CanBeColumn);
			}

		internal SqlColumn(string name, SqlExpression expr)
			: this(expr.ClrType, expr.SqlType, name, null, expr, expr.SourceExpression) {
			System.Diagnostics.Debug.Assert(expr != null);
			}

		internal SqlAlias Alias {
			get { return this.alias; }
			set { this.alias = value; }
		}

		internal string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		internal int Ordinal {
			get { return this.ordinal; }
			set { this.ordinal = value; }
		}

		internal MetaDataMember MetaMember {
			get { return this.member; }
		}

		/// <summary>
		/// Set the column's Expression. This can change the type of the column.
		/// </summary>
		internal SqlExpression Expression {
			get {
				return this.expression;
			}
			set {
				if (value != null) {
					if (!this.ClrType.IsAssignableFrom(value.ClrType))
						throw Error.ArgumentWrongType("value", this.ClrType, value.ClrType);
					SqlColumnRef cref = value as SqlColumnRef;
					if (cref != null && cref.Column == this)
						throw Error.ColumnCannotReferToItself();
				}
				this.expression = value;
			}
		}

		internal override ProviderType SqlType {
			get {
				if (this.expression != null)
					return this.expression.SqlType;
				return this.sqlType;
			}
		}
	}
}