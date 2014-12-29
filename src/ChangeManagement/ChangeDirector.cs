using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Security.Permissions;
using System.Security;

namespace System.Data.Linq {
    using System.Data.Linq.Mapping;
    using System.Data.Linq.Provider;
    using System.Diagnostics.CodeAnalysis;


    /// <summary>
    /// Controls how inserts, updates and deletes are performed.
    /// </summary>
    internal abstract class ChangeDirector {
        internal abstract int Insert(TrackedObject item);
        internal abstract int DynamicInsert(TrackedObject item);
        internal abstract void AppendInsertText(TrackedObject item, StringBuilder appendTo);

        internal abstract int Update(TrackedObject item);
        internal abstract int DynamicUpdate(TrackedObject item);
        internal abstract void AppendUpdateText(TrackedObject item, StringBuilder appendTo);

        internal abstract int Delete(TrackedObject item);
        internal abstract int DynamicDelete(TrackedObject item);
        internal abstract void AppendDeleteText(TrackedObject item, StringBuilder appendTo);

        internal abstract void RollbackAutoSync();
        internal abstract void ClearAutoSyncRollback();

        internal static ChangeDirector CreateChangeDirector(DataContext context) {
            return new StandardChangeDirector(context);
        }

    }
}
