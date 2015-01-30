using System;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Data;
using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Provider.NodeTypes {


	[System.Diagnostics.DebuggerDisplay("text = {Text}, \r\nsource = {SourceExpression}")]
    internal abstract class SqlNode {
        private SqlNodeType nodeType;
        private Expression sourceExpression;

        internal SqlNode(SqlNodeType nodeType, Expression sourceExpression) {
            this.nodeType = nodeType;
            this.sourceExpression = sourceExpression;
        }

        internal Expression SourceExpression {
            get { return this.sourceExpression; }
        }

        internal void ClearSourceExpression() {
            this.sourceExpression = null;
        }

        internal SqlNodeType NodeType {
            get { return this.nodeType; }
        }

#if DEBUG
        private static DbFormatter formatter;
        internal static DbFormatter Formatter {
            get { return formatter; }
            set { formatter = value; }
        }

        internal string Text {
            get {
                if (Formatter == null)
                    return "SqlNode.Formatter is not assigned";
                return SqlNode.Formatter.Format(this, true);
            }
        }
#endif
    }


}
