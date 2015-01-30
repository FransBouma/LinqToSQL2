using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace System.Data.Linq {
    using System.Data.Linq.Mapping;
    using Linq;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a single optimistic concurrency member conflict.
    /// </summary>
    public sealed class MemberChangeConflict {
        private ObjectChangeConflict conflict;
        private MetaDataMember metaMember;
        private object originalValue;
        private object databaseValue;
        private object currentValue;
        bool isResolved;

        internal MemberChangeConflict(ObjectChangeConflict conflict, MetaDataMember metaMember) {
            this.conflict = conflict;            
            this.metaMember = metaMember;
            this.originalValue = metaMember.StorageAccessor.GetBoxedValue(conflict.Original);
            this.databaseValue = metaMember.StorageAccessor.GetBoxedValue(conflict.Database);
            this.currentValue = metaMember.StorageAccessor.GetBoxedValue(conflict.TrackedObject.Current);
        }

        /// <summary>
        /// The previous client value.
        /// </summary>
        public object OriginalValue {
            get { return this.originalValue; }
        }

        /// <summary>
        /// The current database value.
        /// </summary>
        public object DatabaseValue {
            get { return this.databaseValue; }
        }

        /// <summary>
        /// The current client value.
        /// </summary>
        public object CurrentValue {
            get { return this.currentValue; }
        }

        /// <summary>
        /// MemberInfo for the member in conflict.
        /// </summary>
        public MemberInfo Member {
            get { return this.metaMember.Member; }
        }

        /// <summary>
        /// Updates the current value to the specified value.
        /// </summary>       
        public void Resolve(object value) {
            this.conflict.TrackedObject.RefreshMember(this.metaMember, RefreshMode.OverwriteCurrentValues, value);
            this.isResolved = true;
            this.conflict.OnMemberResolved();
        }

        /// <summary>
        /// Updates the current value using the specified strategy.
        /// </summary>        
        public void Resolve(RefreshMode refreshMode) {
            this.conflict.TrackedObject.RefreshMember(this.metaMember, refreshMode, this.databaseValue);
            this.isResolved = true;
            this.conflict.OnMemberResolved();
        }

        /// <summary>
        /// True if the value was modified by the client.
        /// </summary>
        public bool IsModified {
            get { return this.conflict.TrackedObject.HasChangedValue(this.metaMember); }
        }

        /// <summary>
        /// True if the member conflict has been resolved.
        /// </summary>
        public bool IsResolved {
            get { return this.isResolved; }
        }
    }
}
