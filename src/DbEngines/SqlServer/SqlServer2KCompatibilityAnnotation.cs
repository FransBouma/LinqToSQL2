using System;
using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Text;

namespace System.Data.Linq.DbEngines.SqlServer {

    /// <summary>
    /// Annotation which indicates that the given node will cause a compatibility problem
    /// for the indicated set of providers.
    /// </summary>
    internal class SqlServerCompatibilityAnnotation : SqlNodeAnnotation {
		SqlServerProviderMode[] providers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The compatibility message.</param>
        /// <param name="providers">The set of providers this compatibility issue applies to.</param>
		internal SqlServerCompatibilityAnnotation(string message, params SqlServerProviderMode[] providers)
            : base(message) {
            this.providers = providers;
        }

        /// <summary>
        /// Returns true if this annotation applies to the specified provider.
        /// </summary>
		internal bool AppliesTo(SqlServerProviderMode provider)
		{
			foreach(SqlServerProviderMode p in providers)
			{
                if (p == provider) {
                    return true;
                }
            }
            return false;
        }
    }
}
