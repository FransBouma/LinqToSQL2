using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlIncludeScope : SqlNode {
		SqlNode child;
		internal SqlIncludeScope(SqlNode child, Expression sourceExpression) 
			: base(SqlNodeType.IncludeScope, sourceExpression) { 
			this.child = child;
			}
		internal SqlNode Child {
			get {return this.child;}
			set {this.child = value;}
		}
	}
}