using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlVariable : SqlSimpleTypeExpression {
		private string name;

		internal SqlVariable(Type clrType, ProviderType sqlType, string name, Expression sourceExpression)
			: base(SqlNodeType.Variable, clrType, sqlType, sourceExpression) {
			if (name == null)
				throw Error.ArgumentNull("name");
			this.name = name;
			}

		internal string Name {
			get { return this.name; }
		}
	}
}