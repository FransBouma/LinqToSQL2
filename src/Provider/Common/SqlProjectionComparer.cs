using System.Data.Linq.Provider.NodeTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlProjectionComparer
	{
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal static bool CanBeCompared(SqlExpression node)
		{
			if(node == null)
			{
				return true;
			}
			switch(node.NodeType)
			{
				case SqlNodeType.New:
				{
					SqlNew new1 = (SqlNew)node;
					for(int i = 0, n = new1.Args.Count; i < n; i++)
					{
						if(!CanBeCompared(new1.Args[i]))
						{
							return false;
						}
					}
					for(int i = 0, n = new1.Members.Count; i < n; i++)
					{
						if(!CanBeCompared(new1.Members[i].Expression))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.ColumnRef:
				case SqlNodeType.Value:
				case SqlNodeType.UserColumn:
					return true;
				case SqlNodeType.Link:
				{
					SqlLink l1 = (SqlLink)node;
					for(int i = 0, c = l1.KeyExpressions.Count; i < c; ++i)
					{
						if(!CanBeCompared(l1.KeyExpressions[i]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.OptionalValue:
					return CanBeCompared(((SqlOptionalValue)node).Value);
				case SqlNodeType.ValueOf:
				case SqlNodeType.OuterJoinedValue:
					return CanBeCompared(((SqlUnary)node).Operand);
				case SqlNodeType.Lift:
					return CanBeCompared(((SqlLift)node).Expression);
				case SqlNodeType.Grouping:
				{
					SqlGrouping g1 = (SqlGrouping)node;
					return CanBeCompared(g1.Key) && CanBeCompared(g1.Group);
				}
				case SqlNodeType.ClientArray:
				{
					if(node.SourceExpression.NodeType != ExpressionType.NewArrayInit &&
					   node.SourceExpression.NodeType != ExpressionType.NewArrayBounds)
					{
						return false;
					}
					SqlClientArray a1 = (SqlClientArray)node;
					for(int i = 0, n = a1.Expressions.Count; i < n; i++)
					{
						if(!CanBeCompared(a1.Expressions[i]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.ClientCase:
				{
					SqlClientCase c1 = (SqlClientCase)node;
					for(int i = 0, n = c1.Whens.Count; i < n; i++)
					{
						if(!CanBeCompared(c1.Whens[i].Match) ||
						   !CanBeCompared(c1.Whens[i].Value))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.SearchedCase:
				{
					SqlSearchedCase c1 = (SqlSearchedCase)node;
					for(int i = 0, n = c1.Whens.Count; i < n; i++)
					{
						if(!CanBeCompared(c1.Whens[i].Match) ||
						   !CanBeCompared(c1.Whens[i].Value))
						{
							return false;
						}
					}
					return CanBeCompared(c1.Else);
				}
				case SqlNodeType.TypeCase:
				{
					SqlTypeCase c1 = (SqlTypeCase)node;
					if(!CanBeCompared(c1.Discriminator))
					{
						return false;
					}
					for(int i = 0, c = c1.Whens.Count; i < c; ++i)
					{
						if(!CanBeCompared(c1.Whens[i].Match))
						{
							return false;
						}
						if(!CanBeCompared(c1.Whens[i].TypeBinding))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.DiscriminatedType:
					return CanBeCompared(((SqlDiscriminatedType)node).Discriminator);
				case SqlNodeType.JoinedCollection:
				{
					SqlJoinedCollection j1 = (SqlJoinedCollection)node;
					return CanBeCompared(j1.Count) && CanBeCompared(j1.Expression);
				}
				case SqlNodeType.Member:
					return CanBeCompared(((SqlMember)node).Expression);
				case SqlNodeType.MethodCall:
				{
					SqlMethodCall mc = (SqlMethodCall)node;
					if(mc.Object != null && !CanBeCompared(mc.Object))
					{
						return false;
					}
					for(int i = 0, n = mc.Arguments.Count; i < n; i++)
					{
						if(!CanBeCompared(mc.Arguments[0]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.ClientQuery:
					return true;
				case SqlNodeType.ClientParameter:
				default:
					return false;
			}
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal static bool AreSimilar(SqlExpression node1, SqlExpression node2)
		{
			if(node1 == node2)
			{
				return true;
			}
			if(node1 == null || node2 == null)
			{
				return false;
			}
			if(node1.NodeType != node2.NodeType ||
			   node1.ClrType != node2.ClrType ||
			   node1.SqlType != node2.SqlType)
			{
				return false;
			}
			switch(node1.NodeType)
			{
				case SqlNodeType.New:
				{
					SqlNew new1 = (SqlNew)node1;
					SqlNew new2 = (SqlNew)node2;
					if(new1.Args.Count != new2.Args.Count ||
					   new1.Members.Count != new2.Members.Count)
					{
						return false;
					}
					for(int i = 0, n = new1.Args.Count; i < n; i++)
					{
						if(!AreSimilar(new1.Args[i], new2.Args[i]))
						{
							return false;
						}
					}
					for(int i = 0, n = new1.Members.Count; i < n; i++)
					{
						if(!MetaPosition.AreSameMember(new1.Members[i].Member, new2.Members[i].Member) ||
						   !AreSimilar(new1.Members[i].Expression, new2.Members[i].Expression))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.ColumnRef:
				{
					SqlColumnRef cref1 = (SqlColumnRef)node1;
					SqlColumnRef cref2 = (SqlColumnRef)node2;
					return cref1.Column.Ordinal == cref2.Column.Ordinal;
				}
				case SqlNodeType.Link:
				{
					SqlLink l1 = (SqlLink)node1;
					SqlLink l2 = (SqlLink)node2;
					if(!MetaPosition.AreSameMember(l1.Member.Member, l2.Member.Member))
					{
						return false;
					}
					if(l1.KeyExpressions.Count != l2.KeyExpressions.Count)
					{
						return false;
					}
					for(int i = 0, c = l1.KeyExpressions.Count; i < c; ++i)
					{
						if(!AreSimilar(l1.KeyExpressions[i], l2.KeyExpressions[i]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.Value:
					return Object.Equals(((SqlValue)node1).Value, ((SqlValue)node2).Value);
				case SqlNodeType.OptionalValue:
				{
					SqlOptionalValue ov1 = (SqlOptionalValue)node1;
					SqlOptionalValue ov2 = (SqlOptionalValue)node2;
					return AreSimilar(ov1.Value, ov2.Value);
				}
				case SqlNodeType.ValueOf:
				case SqlNodeType.OuterJoinedValue:
					return AreSimilar(((SqlUnary)node1).Operand, ((SqlUnary)node2).Operand);
				case SqlNodeType.Lift:
					return AreSimilar(((SqlLift)node1).Expression, ((SqlLift)node2).Expression);
				case SqlNodeType.Grouping:
				{
					SqlGrouping g1 = (SqlGrouping)node1;
					SqlGrouping g2 = (SqlGrouping)node2;
					return AreSimilar(g1.Key, g2.Key) && AreSimilar(g1.Group, g2.Group);
				}
				case SqlNodeType.ClientArray:
				{
					SqlClientArray a1 = (SqlClientArray)node1;
					SqlClientArray a2 = (SqlClientArray)node2;
					if(a1.Expressions.Count != a2.Expressions.Count)
					{
						return false;
					}
					for(int i = 0, n = a1.Expressions.Count; i < n; i++)
					{
						if(!AreSimilar(a1.Expressions[i], a2.Expressions[i]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.UserColumn:
					return ((SqlUserColumn)node1).Name == ((SqlUserColumn)node2).Name;
				case SqlNodeType.ClientCase:
				{
					SqlClientCase c1 = (SqlClientCase)node1;
					SqlClientCase c2 = (SqlClientCase)node2;
					if(c1.Whens.Count != c2.Whens.Count)
					{
						return false;
					}
					for(int i = 0, n = c1.Whens.Count; i < n; i++)
					{
						if(!AreSimilar(c1.Whens[i].Match, c2.Whens[i].Match) ||
						   !AreSimilar(c1.Whens[i].Value, c2.Whens[i].Value))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.SearchedCase:
				{
					SqlSearchedCase c1 = (SqlSearchedCase)node1;
					SqlSearchedCase c2 = (SqlSearchedCase)node2;
					if(c1.Whens.Count != c2.Whens.Count)
					{
						return false;
					}
					for(int i = 0, n = c1.Whens.Count; i < n; i++)
					{
						if(!AreSimilar(c1.Whens[i].Match, c2.Whens[i].Match) ||
						   !AreSimilar(c1.Whens[i].Value, c2.Whens[i].Value))
							return false;
					}
					return AreSimilar(c1.Else, c2.Else);
				}
				case SqlNodeType.TypeCase:
				{
					SqlTypeCase c1 = (SqlTypeCase)node1;
					SqlTypeCase c2 = (SqlTypeCase)node2;
					if(!AreSimilar(c1.Discriminator, c2.Discriminator))
					{
						return false;
					}
					if(c1.Whens.Count != c2.Whens.Count)
					{
						return false;
					}
					for(int i = 0, c = c1.Whens.Count; i < c; ++i)
					{
						if(!AreSimilar(c1.Whens[i].Match, c2.Whens[i].Match))
						{
							return false;
						}
						if(!AreSimilar(c1.Whens[i].TypeBinding, c2.Whens[i].TypeBinding))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.DiscriminatedType:
				{
					SqlDiscriminatedType dt1 = (SqlDiscriminatedType)node1;
					SqlDiscriminatedType dt2 = (SqlDiscriminatedType)node2;
					return AreSimilar(dt1.Discriminator, dt2.Discriminator);
				}
				case SqlNodeType.JoinedCollection:
				{
					SqlJoinedCollection j1 = (SqlJoinedCollection)node1;
					SqlJoinedCollection j2 = (SqlJoinedCollection)node2;
					return AreSimilar(j1.Count, j2.Count) && AreSimilar(j1.Expression, j2.Expression);
				}
				case SqlNodeType.Member:
				{
					SqlMember m1 = (SqlMember)node1;
					SqlMember m2 = (SqlMember)node2;
					return m1.Member == m2.Member && AreSimilar(m1.Expression, m2.Expression);
				}
				case SqlNodeType.ClientQuery:
				{
					SqlClientQuery cq1 = (SqlClientQuery)node1;
					SqlClientQuery cq2 = (SqlClientQuery)node2;
					if(cq1.Arguments.Count != cq2.Arguments.Count)
					{
						return false;
					}
					for(int i = 0, n = cq1.Arguments.Count; i < n; i++)
					{
						if(!AreSimilar(cq1.Arguments[i], cq2.Arguments[i]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.MethodCall:
				{
					SqlMethodCall mc1 = (SqlMethodCall)node1;
					SqlMethodCall mc2 = (SqlMethodCall)node2;
					if(mc1.Method != mc2.Method || !AreSimilar(mc1.Object, mc2.Object))
					{
						return false;
					}
					if(mc1.Arguments.Count != mc2.Arguments.Count)
					{
						return false;
					}
					for(int i = 0, n = mc1.Arguments.Count; i < n; i++)
					{
						if(!AreSimilar(mc1.Arguments[i], mc2.Arguments[i]))
						{
							return false;
						}
					}
					return true;
				}
				case SqlNodeType.ClientParameter:
				default:
					return false;
			}
		}
	}
}