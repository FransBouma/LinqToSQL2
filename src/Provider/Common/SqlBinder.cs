using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;
using System.Linq;

namespace System.Data.Linq.Provider.Common
{
    /// <summary>
    /// Binds MemberAccess
    /// Prefetches deferrable expressions (SqlLink) if necessary
    /// Translates structured object comparision (EQ, NE) into memberwise comparison
    /// Translates shared expressions (SqlSharedExpression, SqlSharedExpressionRef)
    /// Optimizes out simple redundant operations : 
    ///     XXX OR TRUE ==> TRUE
    ///     XXX AND FALSE ==> FALSE
    ///     NON-NULL EQ NULL ==> FALSE
    ///     NON-NULL NEQ NULL ==> TRUE
    /// </summary>

    internal class SqlBinder {
        SqlColumnizer columnizer;
        MemberAccessBinder visitor;
        NodeFactory sql;
        Func<SqlNode, SqlNode> prebinder;

        bool optimizeLinkExpansions = true;
        bool simplifyCaseStatements = true;

        internal SqlBinder(Translator translator, NodeFactory sqlFactory, MetaModel model, DataLoadOptions shape, SqlColumnizer columnizer, bool canUseOuterApply) {
            this.sql = sqlFactory;
            this.columnizer = columnizer;
            this.visitor = new MemberAccessBinder(this, translator, this.columnizer, this.sql, model, shape, canUseOuterApply);
        }

        internal Func<SqlNode, SqlNode> PreBinder {
            get { return this.prebinder; }
            set { this.prebinder = value; }
        }

        internal SqlNode Prebind(SqlNode node) {
            if (this.prebinder != null) {
                node = this.prebinder(node);
            }
            return node;
        }


	    internal SqlNode Bind(SqlNode node) {
            node = Prebind(node);
            node = this.visitor.Visit(node);
            return node;
        }

        internal bool OptimizeLinkExpansions {
            get { return this.optimizeLinkExpansions; }
            set { this.optimizeLinkExpansions = value; }
        }

        internal bool SimplifyCaseStatements {
            get { return this.simplifyCaseStatements; }
            set { this.simplifyCaseStatements = value; }
        }
    }
}
