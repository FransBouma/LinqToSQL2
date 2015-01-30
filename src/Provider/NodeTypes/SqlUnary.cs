using System.Data.Linq.Provider.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlUnary : SqlSimpleTypeExpression {
		private SqlExpression operand;
		private MethodInfo method;

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification="These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal SqlUnary(SqlNodeType nt, Type clrType, ProviderType sqlType, SqlExpression expr, Expression sourceExpression)
			: this(nt, clrType, sqlType, expr, null, sourceExpression) {
			}

		internal SqlUnary(SqlNodeType nt, Type clrType, ProviderType sqlType, SqlExpression expr, MethodInfo method, Expression sourceExpression)
			: base(nt, clrType, sqlType, sourceExpression) {
			switch (nt) {
				case SqlNodeType.Not:
				case SqlNodeType.Not2V:
				case SqlNodeType.Negate:
				case SqlNodeType.BitNot:
				case SqlNodeType.IsNull:
				case SqlNodeType.IsNotNull:
				case SqlNodeType.Count:
				case SqlNodeType.LongCount:
				case SqlNodeType.Max:
				case SqlNodeType.Min:
				case SqlNodeType.Sum:
				case SqlNodeType.Avg:
				case SqlNodeType.Stddev:
				case SqlNodeType.Convert:
				case SqlNodeType.ValueOf:
				case SqlNodeType.Treat:
				case SqlNodeType.OuterJoinedValue:
				case SqlNodeType.ClrLength:
					break;
				default:
					throw Error.UnexpectedNode(nt);
			}
			this.Operand = expr;
			this.method = method;
			}

		internal SqlExpression Operand {
			get { return this.operand; }
			set {
				if (value == null && (this.NodeType != SqlNodeType.Count && this.NodeType != SqlNodeType.LongCount))
					throw Error.ArgumentNull("value");
				this.operand = value;
			}
		}

		internal MethodInfo Method {
			get { return this.method; }
		}
	}
}