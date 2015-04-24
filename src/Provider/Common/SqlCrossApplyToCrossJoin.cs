using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Turn CROSS APPLY into CROSS JOIN when the right side 
	/// of the apply doesn't reference anything on the left side.
	/// 
	/// Any query which has a CROSS APPLY which cannot be converted to
	/// a CROSS JOIN is annotated so that we can give a meaningful
	/// error message later for SQL2K.
	/// </summary>
	internal class SqlCrossApplyToCrossJoin
	{
	
		#region Private Classes
		private class Reducer : SqlVisitor
		{
			private Enum[] _providerModesWithIncompatibilities;

			internal SqlNodeAnnotations Annotations;
			
			internal Reducer(Enum[] providerModesWithIncompatibilities)
			{
				_providerModesWithIncompatibilities = providerModesWithIncompatibilities;
			}


			internal override SqlSource VisitJoin(SqlJoin join)
			{
				if(join.JoinType == SqlJoinType.CrossApply)
				{
					// Look down the left side to see what table aliases are produced.
					HashSet<SqlAlias> p = SqlGatherProducedAliases.Gather(join.Left);
					// Look down the right side to see what table aliases are consumed.
					HashSet<SqlAlias> c = SqlGatherConsumedAliases.Gather(join.Right);
					// Look at each consumed alias and see if they are mentioned in produced.
					if(p.Overlaps(c))
					{
						Annotations.Add(join, new CompatibilityAnnotation(Strings.SourceExpressionAnnotation(join.SourceExpression), _providerModesWithIncompatibilities));
						// Can't reduce because this consumed alias is produced on the left.
						return base.VisitJoin(join);
					}

					// Can turn this into a CROSS JOIN
					join.JoinType = SqlJoinType.Cross;
					return VisitJoin(join);
				}
				return base.VisitJoin(join);
			}
		}
		#endregion


		internal static SqlNode Reduce(SqlNode node, SqlNodeAnnotations annotations, Enum[] providerModesWithIncompatibilities)
		{
			Reducer r = new Reducer(providerModesWithIncompatibilities) {Annotations = annotations};
			return r.Visit(node);
		}

	}
}
