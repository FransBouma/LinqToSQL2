using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

namespace System.Data.Linq {
    using System.Data.Linq.Mapping;
    using System.Data.Linq.Provider;

    internal abstract class IdentityManager {
        internal abstract object InsertLookup(MetaType type, object instance);
        internal abstract bool RemoveLike(MetaType type, object instance);
        internal abstract object Find(MetaType type, object[] keyValues);
        internal abstract object FindLike(MetaType type, object instance);

        internal static IdentityManager CreateIdentityManager(bool asReadOnly) {
            if (asReadOnly) {
                return new ReadOnlyIdentityManager();
            }
            else {
                return new StandardIdentityManager();
            }
        }
	}

	/// <summary>
	/// This is the noop implementation used when object tracking is disabled.
	/// </summary>
	internal class ReadOnlyIdentityManager : IdentityManager
	{
		internal ReadOnlyIdentityManager() { }
		internal override object InsertLookup(MetaType type, object instance) { return instance; }
		internal override bool RemoveLike(MetaType type, object instance) { return false; }
		internal override object Find(MetaType type, object[] keyValues) { return null; }
		internal override object FindLike(MetaType type, object instance) { return null; }
	}
}
