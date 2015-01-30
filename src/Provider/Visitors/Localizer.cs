using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Visitors
{
	internal class Localizer : ExpressionVisitor {
		Dictionary<Expression, bool> locals;

		internal Localizer(Dictionary<Expression, bool> locals) {
			this.locals = locals;
		}

		internal Expression Localize(Expression expression) {
			return this.Visit(expression);
		}

		internal override Expression Visit(Expression exp) {
			if (exp == null) {
				return null;
			}
			if (this.locals.ContainsKey(exp)) {
				return MakeLocal(exp);
			}
			if (exp.NodeType == (ExpressionType)InternalExpressionType.Known) {
				return exp;
			}
			return base.Visit(exp);
		}

		private static Expression MakeLocal(Expression e) {
			if (e.NodeType == ExpressionType.Constant) {
				return e;
			}
			if (e.NodeType == ExpressionType.Convert || e.NodeType == ExpressionType.ConvertChecked) {
				UnaryExpression ue = (UnaryExpression)e;
				if (ue.Type == typeof(object)) {
					Expression local = MakeLocal(ue.Operand);
					return (e.NodeType == ExpressionType.Convert) ? Expression.Convert(local, e.Type) : Expression.ConvertChecked(local, e.Type);
				}
				// convert a const null
				if (ue.Operand.NodeType == ExpressionType.Constant) {
					ConstantExpression c = (ConstantExpression)ue.Operand;
					if (c.Value == null) {
						return Expression.Constant(null, ue.Type);
					}
				}
			}
			return Expression.Invoke(Expression.Constant(Expression.Lambda(e).Compile()));
		}
	}
}