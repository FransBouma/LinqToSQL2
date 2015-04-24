using System.Collections.ObjectModel;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Methods for checking whethe a query was compatible with the
	/// server it will be sent to.
	/// </summary>
	static internal class CompatibilityCheck
	{
		/// <summary>
		/// Private visitor class checks each node for compatibility annotations.
		/// </summary>
		private class Visitor : SqlVisitor
		{
			private Enum _providerMode;
			internal SqlNodeAnnotations annotations;

			internal Visitor(Enum providerMode)
			{
				if(providerMode == null)
				{
					throw Error.ArgumentNull("providerMode");
				}
				this._providerMode = providerMode;
			}

			/// <summary>
			/// The reasons why this query is not compatible.
			/// </summary>
			internal Collection<string> reasons = new Collection<string>();

			internal override SqlNode Visit(SqlNode node)
			{
				if(annotations.NodeIsAnnotated(node))
				{
					foreach(SqlNodeAnnotation annotation in annotations.Get(node))
					{
						CompatibilityAnnotation ssca = annotation as CompatibilityAnnotation;
						if(ssca != null && ssca.AppliesTo(_providerMode))
						{
							reasons.Add(annotation.Message);
						}
					}
				}
				return base.Visit(node);
			}
		}

		/// <summary>
		/// Checks whether the given node is supported on the given server.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="annotations">The annotations.</param>
		/// <param name="providerMode">The provider mode to check for.</param>
		internal static void ThrowIfUnsupported(SqlNode node, SqlNodeAnnotations annotations, Enum providerMode)
		{
			// Check to see whether there's at least one SqlServerCompatibilityAnnotation.
			if(annotations.HasAnnotationType(typeof(CompatibilityAnnotation)))
			{
				Visitor visitor = new Visitor(providerMode) {annotations = annotations};
				visitor.Visit(node);

				// If any messages were recorded, then throw an exception.
				if(visitor.reasons.Count > 0)
				{
					throw Error.ExpressionNotSupportedForSqlServerVersion(visitor.reasons);
				}
			}
		}
	}
}
