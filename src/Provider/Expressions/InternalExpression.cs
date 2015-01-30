using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Expressions {

    // SQL Client extensions to ExpressionType

	abstract internal class InternalExpression : Expression {
#pragma warning disable 618 // Disable the 'obsolete' warning.
        internal InternalExpression(InternalExpressionType nt, Type type)
            : base ((ExpressionType)nt, type) {
        }
#pragma warning restore 618
        internal static KnownExpression Known(SqlExpression expr) {
            return new KnownExpression(expr, expr.ClrType);
        }
        internal static KnownExpression Known(SqlNode node, Type type) {
            return new KnownExpression(node, type);
        }
    }
}
