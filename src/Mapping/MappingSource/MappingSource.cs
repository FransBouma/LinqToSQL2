using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using LinqToSqlShared.Mapping;

namespace System.Data.Linq.Mapping {
    using System.Data.Linq.Provider;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a source for mapping information.
    /// </summary>
    public abstract class MappingSource {
        MetaModel primaryModel;
        ReaderWriterLock rwlock;
        Dictionary<Type, MetaModel> secondaryModels;

        /// <summary>
        /// Gets the MetaModel representing a DataContext and all it's 
        /// accessible tables, functions and entities.
        /// </summary>
        public MetaModel GetModel(Type dataContextType) {
            if (dataContextType == null) {
                throw Error.ArgumentNull("dataContextType");
            }
            MetaModel model = null;
            if (this.primaryModel == null) {
                model = this.CreateModel(dataContextType);
                Interlocked.CompareExchange<MetaModel>(ref this.primaryModel, model, null);
            }

            // if the primary one matches, use it!
            if (this.primaryModel.ContextType == dataContextType) {
                return this.primaryModel;
            }

            // the rest of this only happens if you are using the mapping source for
            // more than one context type

            // build a map if one is not already defined
            if (this.secondaryModels == null) {
                Interlocked.CompareExchange<Dictionary<Type, MetaModel>>(ref this.secondaryModels, new Dictionary<Type, MetaModel>(), null);
            }

            // if we haven't created a read/writer lock, make one now
            if (this.rwlock == null) {
                Interlocked.CompareExchange<ReaderWriterLock>(ref this.rwlock, new ReaderWriterLock(), null);
            }

            // lock the map and look inside
            MetaModel foundModel;
            this.rwlock.AcquireReaderLock(Timeout.Infinite);
            try {
                if (this.secondaryModels.TryGetValue(dataContextType, out foundModel)) {
                    return foundModel;
                }
            }
            finally {
                this.rwlock.ReleaseReaderLock();
            }
            // if it wasn't found, lock for write and try again
            this.rwlock.AcquireWriterLock(Timeout.Infinite);
            try {
                if (this.secondaryModels.TryGetValue(dataContextType, out foundModel)) {
                    return foundModel;
                }
                if (model == null) {
                    model = this.CreateModel(dataContextType);
                }
                this.secondaryModels.Add(dataContextType, model);
            }
            finally {
                this.rwlock.ReleaseWriterLock();
            }
            return model;
        }

        /// <summary>
        /// Creates a new instance of a MetaModel.  This method is called by GetModel().
        /// Override this method when defining a new type of MappingSource.
        /// </summary>
        /// <param name="dataContextType"></param>
        /// <returns></returns>
        protected abstract MetaModel CreateModel(Type dataContextType);
    }
}