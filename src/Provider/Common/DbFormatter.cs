using System;
using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Text;

namespace System.Data.Linq.Provider.Common {
    internal abstract class DbFormatter {
        internal abstract string Format(SqlNode node, bool isDebug);
        internal abstract string Format(SqlNode node);
    }
}
