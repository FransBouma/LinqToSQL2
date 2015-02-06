using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Ensure that the subquery follows the rules for subqueries.
	/// </summary>
	internal class SubqueryValidator : Provider.Visitors.ExpressionVisitor
	{
		bool isTopLevel = true;
		internal override Expression VisitMethodCall(MethodCallExpression m)
		{
			bool was = isTopLevel;
			try
			{
				if(isTopLevel && !SubqueryRules.IsSupportedTopLevelMethod(m.Method))
					throw Error.SubqueryDoesNotSupportOperator(m.Method.Name);
				isTopLevel = false;
				return base.VisitMethodCall(m);
			}
			finally
			{
				isTopLevel = was;
			}
		}
	}
}