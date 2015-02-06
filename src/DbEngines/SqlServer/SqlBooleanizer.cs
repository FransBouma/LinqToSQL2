using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.DbEngines.SqlServer {

    /// <summary>
    /// Locate cases in which there is a 'Bit' but a 'Predicate' is expected or vice-versa.
    /// Transform these expressions into expressions of the expected type.
    /// </summary>
    internal class SqlBooleanizer {
	    /// <summary>
        /// Rationalize boolean expressions for the given node.
        /// </summary>
        internal static SqlNode Rationalize(SqlNode node, TypeSystemProvider typeProvider, MetaModel model) {
            return new Booleanizer(new SqlFactory(typeProvider, model)).Visit(node);
        }
    }
}
