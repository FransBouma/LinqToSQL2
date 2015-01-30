using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlParameter : SqlSimpleTypeExpression {
		private string name;
		private System.Data.ParameterDirection direction;

		internal SqlParameter(Type clrType, ProviderType sqlType, string name, Expression sourceExpression)
			: base(SqlNodeType.Parameter, clrType, sqlType, sourceExpression) {
			if (name == null)
				throw Error.ArgumentNull("name");
			if (typeof(Type).IsAssignableFrom(clrType))
				throw Error.ArgumentWrongValue("clrType");
			this.name = name;
			this.direction = System.Data.ParameterDirection.Input;
			}

		internal string Name {
			get { return this.name; }
		}

		internal System.Data.ParameterDirection Direction {
			get { return this.direction; }
			set { this.direction = value; }
		}
	}
}