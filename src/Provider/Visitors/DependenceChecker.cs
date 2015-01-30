using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Visitors
{
	internal class DependenceChecker : ExpressionVisitor {
		HashSet<ParameterExpression> inScope = new HashSet<ParameterExpression>();
		bool isIndependent = true;

		/// <summary>
		/// This method returns 'true' when the expression doesn't reference any parameters 
		/// from outside the scope of the expression.
		/// </summary>
		static public bool IsIndependent(Expression expression) {
			var v = new DependenceChecker();
			v.Visit(expression);
			return v.isIndependent;
		}
		internal override Expression VisitLambda(LambdaExpression lambda) {
			foreach (var p in lambda.Parameters) {
				this.inScope.Add(p);
			}
			return base.VisitLambda(lambda);
		}
		internal override Expression VisitParameter(ParameterExpression p) {
			this.isIndependent &= this.inScope.Contains(p);
			return p;
		}
	}
}