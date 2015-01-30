using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/// <summary>
	/// A case statement that must be evaluated on the client. For example, a case statement
	/// that contains values of LINK, Element, or Multi-set are not directly handleable by 
	/// SQL.
	/// 
	/// CASE inputExpression 
	/// WHEN whenExpression THEN resultExpression 
	/// [ ...n ] 
	/// END 
	/// </summary>
	internal class SqlClientCase : SqlExpression {
		private SqlExpression expression;
		private List<SqlClientWhen> whens = new List<SqlClientWhen>();

		internal SqlClientCase(Type clrType, SqlExpression expr, IEnumerable<SqlClientWhen> whens, Expression sourceExpression)
			: base(SqlNodeType.ClientCase, clrType, sourceExpression) {
			this.Expression = expr;
			if (whens == null)
				throw Error.ArgumentNull("whens");
			this.whens.AddRange(whens);
			if (this.whens.Count == 0)
				throw Error.ArgumentOutOfRange("whens");
			}

		internal SqlExpression Expression {
			get { return this.expression; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.expression != null && this.expression.ClrType != value.ClrType)
					throw Error.ArgumentWrongType("value", this.expression.ClrType, value.ClrType);
				this.expression = value;
			}
		}

		internal List<SqlClientWhen> Whens {
			get { return this.whens; }
		}

		internal override ProviderType SqlType {
			get { return this.whens[0].Value.SqlType; }
		}
	}
}