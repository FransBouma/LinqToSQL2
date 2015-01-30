using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Maps expressions which refer solely to local elements (and not to any DB element, so can be inlined)
	/// </summary>
	internal class LocalMapper : ExpressionVisitor {
		bool isRemote;
		Dictionary<Expression, bool> locals;

		internal Dictionary<Expression, bool> MapLocals(Expression expression) {
			this.locals = new Dictionary<Expression, bool>();
			this.isRemote = false;
			this.Visit(expression);
			return this.locals;
		}

		internal override Expression Visit(Expression expression) {
			if (expression == null) {
				return null;
			}
			bool saveIsRemote = this.isRemote;
			switch (expression.NodeType) {
				case (ExpressionType)InternalExpressionType.Known:
					return expression;
				case (ExpressionType)ExpressionType.Constant:
					break;
				default:
					this.isRemote = false;
					base.Visit(expression);
					if (!this.isRemote
						&& expression.NodeType != ExpressionType.Lambda
						&& expression.NodeType != ExpressionType.Quote
						&& DependenceChecker.IsIndependent(expression)) {
							this.locals[expression] = true; // Not 'Add' because the same expression may exist in the tree twice. 
						}
					break;
			}
			if (typeof(ITable).IsAssignableFrom(expression.Type) ||
				typeof(DataContext).IsAssignableFrom(expression.Type)) {
					this.isRemote = true;
				}
			this.isRemote |= saveIsRemote;
			return expression;
		}
		internal override Expression VisitMemberAccess(MemberExpression m) {
			base.VisitMemberAccess(m);
			this.isRemote |= (m.Expression != null && typeof(ITable).IsAssignableFrom(m.Expression.Type));
			return m;
		}
		internal override Expression VisitMethodCall(MethodCallExpression m) {
			base.VisitMethodCall(m);
			this.isRemote |= m.Method.DeclaringType == typeof(DMLMethodPlaceholders)
							 || Attribute.IsDefined(m.Method, typeof(FunctionAttribute));
			return m;
		}
	}
}