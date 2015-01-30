using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Composes a subquery into a linked association.
	/// </summary>
	internal class RelationComposer : ExpressionVisitor {
		ParameterExpression parameter;
		MetaAssociation association;
		Expression otherSouce;
		Expression parameterReplacement;
		internal RelationComposer(ParameterExpression parameter, MetaAssociation association, Expression otherSouce, Expression parameterReplacement) {
			if (parameter==null)
				throw Error.ArgumentNull("parameter");
			if (association == null)
				throw Error.ArgumentNull("association");
			if (otherSouce == null)
				throw Error.ArgumentNull("otherSouce");
			if (parameterReplacement==null)
				throw Error.ArgumentNull("parameterReplacement");
			this.parameter = parameter;
			this.association = association;
			this.otherSouce = otherSouce;
			this.parameterReplacement = parameterReplacement;
		}
		internal override Expression VisitParameter(ParameterExpression p) {
			if (p == parameter) {
				return this.parameterReplacement;
			}
			return base.VisitParameter(p);
		}

		private static Expression[] GetKeyValues(Expression expr, ReadOnlyCollection<MetaDataMember> keys) {
			List<Expression> values = new List<Expression>();
			foreach(MetaDataMember key in keys){
				values.Add(Expression.PropertyOrField(expr, key.Name));
			}
			return values.ToArray();
		}

		internal override Expression VisitMemberAccess(MemberExpression m) {
			if (MetaPosition.AreSameMember(m.Member, this.association.ThisMember.Member)) {
				Expression[] keyValues = GetKeyValues(this.Visit(m.Expression), this.association.ThisKey);
				return Translator.WhereClauseFromSourceAndKeys(this.otherSouce, this.association.OtherKey.ToArray(), keyValues);
			}
			Expression exp = this.Visit(m.Expression);
			if (exp != m.Expression) {
				if (exp.Type != m.Expression.Type && m.Member.Name == "Count" && TypeSystem.IsSequenceType(exp.Type)) {
					return Expression.Call(typeof(Enumerable), "Count", new Type[] {TypeSystem.GetElementType(exp.Type)}, exp);
				}
				return Expression.MakeMemberAccess(exp, m.Member);
			}
			return m;
		}

	}
}