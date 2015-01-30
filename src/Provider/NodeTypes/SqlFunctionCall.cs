using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlFunctionCall : SqlSimpleTypeExpression {
		private string name;
		private List<SqlExpression> arguments;

		internal SqlFunctionCall(Type clrType, ProviderType sqlType, string name, IEnumerable <SqlExpression > args , Expression source)
			: this(SqlNodeType.FunctionCall, clrType , sqlType, name, args, source) {
			}

		internal SqlFunctionCall(SqlNodeType nodeType, Type clrType, ProviderType sqlType, string name , IEnumerable <SqlExpression> args , Expression source)
			: base(nodeType, clrType, sqlType, source) {
			this.name = name;
			this.arguments = new List<SqlExpression>(args);
			}

		internal string Name {
			get { return this.name; }
		}

		internal List<SqlExpression> Arguments {
			get { return this.arguments; }
		}
	}
}