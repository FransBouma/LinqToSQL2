using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/*
	 * Simple CASE function:
	 * CASE inputExpression 
	 * WHEN whenExpression THEN resultExpression 
	 * [ ...n ] 
	 * [ 
	 * ELSE elseResultExpression 
	 * ] 
	 * END 
	 */
	internal class SqlSimpleCase : SqlExpression
	{
		private SqlExpression expression;
		private List<SqlWhen> whens = new List<SqlWhen>();

		internal SqlSimpleCase(Type clrType, SqlExpression expr, IEnumerable<SqlWhen> whens, Expression sourceExpression)
			: base(SqlNodeType.SimpleCase, clrType, sourceExpression)
		{
			this.Expression = expr;
			if(whens == null)
				throw Error.ArgumentNull("whens");
			this.whens.AddRange(whens);
			if(this.whens.Count == 0)
				throw Error.ArgumentOutOfRange("whens");
		}

		internal SqlExpression Expression
		{
			get { return this.expression; }
			set
			{
				if(value == null)
					throw Error.ArgumentNull("value");
				if(this.expression != null && this.expression.ClrType != value.ClrType)
					throw Error.ArgumentWrongType("value", this.expression.ClrType, value.ClrType);
				this.expression = value;
			}
		}

		internal List<SqlWhen> Whens
		{
			get { return this.whens; }
		}

		internal override ProviderType SqlType
		{
			get { return this.whens[0].Value.SqlType; }
		}
	}
}