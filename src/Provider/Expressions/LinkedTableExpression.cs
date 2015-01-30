using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Expressions
{
	internal sealed class LinkedTableExpression : InternalExpression {
		private SqlLink link; 
		private ITable table; 
		internal LinkedTableExpression(SqlLink link, ITable table, Type type) 
			: base(InternalExpressionType.LinkedTable, type) {
			this.link = link;
			this.table = table;
			}
		internal SqlLink Link {
			get {return this.link;}
		}
		internal ITable Table {
			get {return this.table;}
		}
	}
}