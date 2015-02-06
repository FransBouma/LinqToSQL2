using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Data.Linq.DbEngines.SqlServer
{

	/// <summary>
	/// </summary>
	internal class SqlOuterApplyReducer
	{
		internal static SqlNode Reduce(SqlNode node, NodeFactory factory, SqlNodeAnnotations annotations)
		{
			Visitor r = new Visitor(factory, annotations);
			return r.Visit(node);
		}

		class Visitor : SqlVisitor
		{
			NodeFactory factory;
			SqlNodeAnnotations annotations;

			internal Visitor(NodeFactory factory, SqlNodeAnnotations annotations)
			{
				this.factory = factory;
				this.annotations = annotations;
			}

			internal override SqlSource VisitSource(SqlSource source)
			{
				source = base.VisitSource(source);

				SqlJoin join = source as SqlJoin;
				if(join != null)
				{
					if(join.JoinType == SqlJoinType.OuterApply)
					{
						// Reduce outer-apply into left-outer-join
						HashSet<SqlAlias> leftProducedAliases = SqlGatherProducedAliases.Gather(join.Left);
						HashSet<SqlExpression> liftedExpressions = new HashSet<SqlExpression>();

						if(SqlPredicateLifter.CanLift(join.Right, leftProducedAliases, liftedExpressions) &&
							SqlSelectionLifter.CanLift(join.Right, leftProducedAliases, liftedExpressions) &&
							!SqlAliasDependencyChecker.IsDependent(join.Right, leftProducedAliases, liftedExpressions))
						{

							SqlExpression liftedPredicate = SqlPredicateLifter.Lift(join.Right, leftProducedAliases);
							List<List<SqlColumn>> liftedSelections = SqlSelectionLifter.Lift(join.Right, leftProducedAliases, liftedExpressions);

							join.JoinType = SqlJoinType.LeftOuter;
							join.Condition = liftedPredicate;

							if(liftedSelections != null)
							{
								foreach(List<SqlColumn> selection in liftedSelections)
								{
									source = this.PushSourceDown(source, selection);
								}
							}
						}
						else
						{
							this.AnnotateSqlIncompatibility(join, SqlServerProviderMode.Sql2000);
						}
					}
					else if(join.JoinType == SqlJoinType.CrossApply)
					{
						// reduce cross apply with special nested left-outer-join's into a single left-outer-join
						//
						// SELECT x.*, y.*
						// FROM X
						// CROSS APPLY (
						//      SELECT y.*
						//       FROM (
						//          SELECT ?
						//       ) 
						//       LEFT OUTER JOIN (
						//          SELECT y.* FROM Y
						//       ) AS y
						//
						// ==>
						// 
						// SELECT x.*, y.*
						// FROM X
						// LEFT OUTER JOIN (
						//     SELECT y.* FROM Y
						// )

						SqlJoin leftOuter = this.GetLeftOuterWithUnreferencedSingletonOnLeft(join.Right);
						if(leftOuter != null)
						{
							HashSet<SqlAlias> leftProducedAliases = SqlGatherProducedAliases.Gather(join.Left);
							HashSet<SqlExpression> liftedExpressions = new HashSet<SqlExpression>();

							if(SqlPredicateLifter.CanLift(leftOuter.Right, leftProducedAliases, liftedExpressions) &&
								SqlSelectionLifter.CanLift(leftOuter.Right, leftProducedAliases, liftedExpressions) &&
								!SqlAliasDependencyChecker.IsDependent(leftOuter.Right, leftProducedAliases, liftedExpressions)
								)
							{

								SqlExpression liftedPredicate = SqlPredicateLifter.Lift(leftOuter.Right, leftProducedAliases);
								List<List<SqlColumn>> liftedSelections = SqlSelectionLifter.Lift(leftOuter.Right, leftProducedAliases, liftedExpressions);

								// add intermediate selections 
								this.GetSelectionsBeforeJoin(join.Right, liftedSelections);

								// push down all selections
								foreach(List<SqlColumn> selection in liftedSelections.Where(s => s.Count > 0))
								{
									source = this.PushSourceDown(source, selection);
								}

								join.JoinType = SqlJoinType.LeftOuter;
								join.Condition = this.factory.AndAccumulate(leftOuter.Condition, liftedPredicate);
								join.Right = leftOuter.Right;
							}
							else
							{
								this.AnnotateSqlIncompatibility(join, SqlServerProviderMode.Sql2000);
							}
						}
					}

					// re-balance join tree of left-outer-joins to expose LOJ w/ leftside unreferenced
					while(join.JoinType == SqlJoinType.LeftOuter)
					{
						// look for buried left-outer-joined-with-unreferenced singleton
						SqlJoin leftLeftOuter = this.GetLeftOuterWithUnreferencedSingletonOnLeft(join.Left);
						if(leftLeftOuter == null)
							break;

						List<List<SqlColumn>> liftedSelections = new List<List<SqlColumn>>();

						// add intermediate selections 
						this.GetSelectionsBeforeJoin(join.Left, liftedSelections);

						// push down all selections
						foreach(List<SqlColumn> selection in liftedSelections)
						{
							source = this.PushSourceDown(source, selection);
						}

						// bubble this one up on-top of this 'join'.
						SqlSource jRight = join.Right;
						SqlExpression jCondition = join.Condition;

						join.Left = leftLeftOuter.Left;
						join.Right = leftLeftOuter;
						join.Condition = leftLeftOuter.Condition;

						leftLeftOuter.Left = leftLeftOuter.Right;
						leftLeftOuter.Right = jRight;
						leftLeftOuter.Condition = jCondition;
					}
				}

				return source;
			}

			private void AnnotateSqlIncompatibility(SqlNode node, params SqlServerProviderMode[] providers)
			{
				this.annotations.Add(node, new SqlServerCompatibilityAnnotation(Strings.SourceExpressionAnnotation(node.SourceExpression), providers));
			}

			[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
			private SqlSource PushSourceDown(SqlSource sqlSource, List<SqlColumn> cols)
			{
				SqlSelect ns = new SqlSelect(new SqlNop(cols[0].ClrType, cols[0].SqlType, sqlSource.SourceExpression), sqlSource, sqlSource.SourceExpression);
				ns.Row.Columns.AddRange(cols);
				return new SqlAlias(ns);
			}

			private SqlJoin GetLeftOuterWithUnreferencedSingletonOnLeft(SqlSource source)
			{
				SqlAlias alias = source as SqlAlias;
				if(alias != null)
				{
					SqlSelect select = alias.Node as SqlSelect;
					if(select != null &&
						select.Where == null &&
						select.Top == null &&
						select.GroupBy.Count == 0 &&
						select.OrderBy.Count == 0)
					{
						return this.GetLeftOuterWithUnreferencedSingletonOnLeft(select.From);
					}
				}
				SqlJoin join = source as SqlJoin;
				if(join == null || join.JoinType != SqlJoinType.LeftOuter)
					return null;
				if(!this.IsSingletonSelect(join.Left))
					return null;
				HashSet<SqlAlias> p = SqlGatherProducedAliases.Gather(join.Left);
				HashSet<SqlAlias> c = SqlGatherConsumedAliases.Gather(join.Right);
				if(p.Overlaps(c))
				{
					return null;
				}
				return join;
			}

			private void GetSelectionsBeforeJoin(SqlSource source, List<List<SqlColumn>> selections)
			{
				SqlJoin join = source as SqlJoin;
				if(join != null)
					return;
				SqlAlias alias = source as SqlAlias;
				if(alias != null)
				{
					SqlSelect select = alias.Node as SqlSelect;
					if(select != null)
					{
						this.GetSelectionsBeforeJoin(select.From, selections);
						selections.Add(select.Row.Columns);
					}
				}
			}

			[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
			private bool IsSingletonSelect(SqlSource source)
			{
				SqlAlias alias = source as SqlAlias;
				if(alias == null)
					return false;
				SqlSelect select = alias.Node as SqlSelect;
				if(select == null)
					return false;
				if(select.From != null)
					return false;
				return true;
			}
		}
	}
}
