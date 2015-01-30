using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/*
	* Searched CASE function:
	* CASE
	* WHEN BooleanExpression THEN resultExpression
	* [ ...n ]
	* [
	* ELSE elseResultExpression
	* ]
	* END
	*/
	internal class SqlSearchedCase : SqlExpression
	{
		private List<SqlWhen> whens;
		private SqlExpression @else;

		internal SqlSearchedCase(Type clrType, IEnumerable<SqlWhen> whens, SqlExpression @else, Expression sourceExpression)
			: base(SqlNodeType.SearchedCase, clrType, sourceExpression)
		{
			if(whens == null)
				throw Error.ArgumentNull("whens");
			this.whens = new List<SqlWhen>(whens);
			if(this.whens.Count == 0)
				throw Error.ArgumentOutOfRange("whens");
			this.Else = @else;
		}

		internal List<SqlWhen> Whens
		{
			get { return this.whens; }
		}

		internal SqlExpression Else
		{
			get { return this.@else; }
			set
			{
				if(value == null)
					throw Error.ArgumentNull("value");
				if(this.@else != null && !this.@else.ClrType.IsAssignableFrom(value.ClrType))
					throw Error.ArgumentWrongType("value", this.@else.ClrType, value.ClrType);
				this.@else = value;
			}
		}

		internal override ProviderType SqlType
		{
			get { return this.whens[0].Value.SqlType; }
		}
	}
}