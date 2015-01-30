using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlBlock : SqlStatement {
		private List<SqlStatement> statements;

		internal SqlBlock(Expression sourceExpression)
			: base(SqlNodeType.Block, sourceExpression) {
			this.statements = new List<SqlStatement>();
			}

		internal List<SqlStatement> Statements {
			get { return this.statements; }
		}
	}
}