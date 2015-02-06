using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Threading;
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;
using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Mapping {

    internal class MappedMetaModel : MetaModel {
        ReaderWriterLock @lock = new ReaderWriterLock();
        MappingSource mappingSource;
        Type contextType;
        Type providerType;
        DatabaseMapping mapping;
        HashSet<Module> modules;
        Dictionary<string, Type> types;
        Dictionary<Type, MetaType> metaTypes;
        Dictionary<Type, MetaTable> metaTables;
        Dictionary<MetaPosition, MetaFunction> metaFunctions;
        bool fullyLoaded;

        [ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // mapping parameter contains various type references.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // FindType method call.
        internal MappedMetaModel(MappingSource mappingSource, Type contextType, DatabaseMapping mapping) {
            this.mappingSource = mappingSource;
            this.contextType = contextType;
            this.mapping = mapping;
            this.modules = new HashSet<Module>();
            this.modules.Add(this.contextType.Module);
            this.metaTypes = new Dictionary<Type, MetaType>();
            this.metaTables = new Dictionary<Type, MetaTable>();
            this.types = new Dictionary<string, Type>();
#warning [FB] REFACTOR SQL Server specific. Requires change to have its provider type injected instead of it tries to discover it on its own using sql server's specific namespace.
            // Provider type
            if (this.providerType == null && !String.IsNullOrEmpty(this.mapping.Provider)) {
                this.providerType = this.FindType(this.mapping.Provider, typeof(System.Data.Linq.DbEngines.SqlServer.SqlProvider).Namespace);
                if (this.providerType == null) {
                    throw Error.ProviderTypeNotFound(this.mapping.Provider);
                }
            }
            else if (this.providerType == null) {
                this.providerType = typeof(System.Data.Linq.DbEngines.SqlServer.SqlProvider);
            }
            this.Init();
        }
        #region Initialization
        private void Init() {
            if (!fullyLoaded) {
                // The fullyLoaded state is required so that tools like
                // CreateDatabase can get a full view of all tables.
                @lock.AcquireWriterLock(Timeout.Infinite);
                try {
                    if (!fullyLoaded) {
                        // Initialize static tables and functions.
                        this.InitStaticTables();
                        this.InitFunctions();
                        fullyLoaded = true;
                    }
                }
                finally {
                    @lock.ReleaseWriterLock();
                }
            }
        }

        [ResourceExposure(ResourceScope.None)] // Exposure is via external mapping file/attributes.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine, ResourceScope.Assembly | ResourceScope.Machine)] // FindType method call.
        private void InitStaticTables() {
            this.InitStaticTableTypes();
            foreach (TableMapping tableMapping in this.mapping.Tables) {
                Type rowType = this.FindType(tableMapping.RowType.Name);
                if (rowType == null) {
                    throw Error.CouldNotFindTypeFromMapping(tableMapping.RowType.Name);
                }
                Type rootType = this.GetRootType(rowType, tableMapping.RowType);
                MetaTable table = new MappedTable(this, tableMapping, rootType);
                foreach (MetaType mt in table.RowType.InheritanceTypes) {
                    this.metaTypes.Add(mt.Type, mt);
                    this.metaTables.Add(mt.Type, table);
                }
            }
        }

        private void InitStaticTableTypes() {
            for (Type type = this.contextType; type != typeof(DataContext); type = type.BaseType) {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (FieldInfo fi in fields) {
                    Type ft = fi.FieldType;
                    if (ft.IsGenericType && ft.GetGenericTypeDefinition() == typeof(Table<>)) {
                        Type rowType = ft.GetGenericArguments()[0];
                        if (!this.types.ContainsKey(rowType.Name)) {
                            this.types.Add(rowType.FullName, rowType);
                            if (!this.types.ContainsKey(rowType.Name)) {
                                this.types.Add(rowType.Name, rowType);
                            }
                            this.modules.Add(rowType.Module);
                        }
                    }
                }
                PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (PropertyInfo pi in props) {
                    Type pt = pi.PropertyType;
                    if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Table<>)) {
                        Type rowType = pt.GetGenericArguments()[0];
                        if (!this.types.ContainsKey(rowType.Name)) {
                            this.types.Add(rowType.FullName, rowType);
                            if (!this.types.ContainsKey(rowType.Name)) {
                                this.types.Add(rowType.Name, rowType);
                            }
                            this.modules.Add(rowType.Module);
                        }
                    }
                }
            }
        }

        [ResourceExposure(ResourceScope.None)] // mapping instance variable is set elsewhere.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine, ResourceScope.Assembly | ResourceScope.Machine)] // For GetMethods call.
        private void InitFunctions() {
            this.metaFunctions = new Dictionary<MetaPosition, MetaFunction>();
            if (this.contextType != typeof(DataContext)) {
                foreach (FunctionMapping fmap in this.mapping.Functions) {
                    MethodInfo method = this.GetMethod(fmap.MethodName);
                    if (method == null) {
                        throw Error.MethodCannotBeFound(fmap.MethodName);
                    }
                    MappedFunction func = new MappedFunction(this, fmap, method);
                    this.metaFunctions.Add(new MetaPosition(method), func);

                    // pre-set all known function result types into metaType map
                    foreach (MetaType rt in func.ResultRowTypes) {
                        foreach (MetaType it in rt.InheritanceTypes) {
                            if (!this.metaTypes.ContainsKey(it.Type)) {
                                this.metaTypes.Add(it.Type, it);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public override MappingSource MappingSource {
            get { return this.mappingSource; }
        }

        public override Type ContextType {
            get { return this.contextType; }
        }

        public override string DatabaseName {
            get { return this.mapping.DatabaseName; }
        }

        public override Type ProviderType {
            get { return this.providerType; }
        }

        public override IEnumerable<MetaTable> GetTables() {
            return this.metaTables.Values.Where(x => x != null).Distinct();
        }

        public override MetaTable GetTable(Type rowType) {
            if (rowType == null) {
                throw Error.ArgumentNull("rowType");
            }
            MetaTable table = null;
            this.metaTables.TryGetValue(rowType, out table);
            return table;
        }

        public override MetaType GetMetaType(Type type) {
            if (type == null) {
                throw Error.ArgumentNull("type");
            }
            MetaType mtype = null;
            @lock.AcquireReaderLock(Timeout.Infinite);
            try {
                if (this.metaTypes.TryGetValue(type, out mtype)) {
                    return mtype;
                }
            }
            finally {
                @lock.ReleaseReaderLock();
            }
            @lock.AcquireWriterLock(Timeout.Infinite);
            try {
                if (!this.metaTypes.TryGetValue(type, out mtype)) {
                    // not known, so must be unmapped type
                    mtype = new UnmappedType(this, type);
                    this.metaTypes.Add(type, mtype);
                }
            }
            finally {
                @lock.ReleaseWriterLock();
            }
            return mtype;
        }

        public override MetaFunction GetFunction(MethodInfo method) {
            if (method == null) {
                throw new ArgumentNullException("method");
            }
            MetaFunction func = null;
            this.metaFunctions.TryGetValue(new MetaPosition(method), out func);
            return func;
        }

        public override IEnumerable<MetaFunction> GetFunctions() {
            return this.metaFunctions.Values;
        }

        private Type GetRootType(Type type, TypeMapping rootMapping) {
            if (string.Compare(rootMapping.Name, type.Name, StringComparison.Ordinal) == 0
                || string.Compare(rootMapping.Name, type.FullName, StringComparison.Ordinal) == 0
                || string.Compare(rootMapping.Name, type.AssemblyQualifiedName, StringComparison.Ordinal) == 0)
                return type;
            if (type.BaseType != typeof(object)) {
                return this.GetRootType(type.BaseType, rootMapping);
            }
            throw Error.UnableToResolveRootForType(type);
        }

        [ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // name parameter will be found on a type.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // FindType method call.
        private MethodInfo GetMethod(string name) {
            string typeName, methodName;
            this.GetTypeAndMethod(name, out typeName, out methodName);
            Type type = this.FindType(typeName);
            if (type != null) {
                return type.GetMethod(methodName, BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
            }
            return null;
        }

        private void GetTypeAndMethod(string name, out string typeName, out string methodName) {
            int dotIndex = name.LastIndexOf(".", StringComparison.CurrentCulture);
            if (dotIndex > 0) {
                typeName = name.Substring(0, dotIndex);
                methodName = name.Substring(dotIndex + 1);
            }
            else {
                typeName = this.contextType.FullName;
                methodName = name;
            }
        }

        [ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // name parameter is a type name.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // FindType method call.
        internal Type FindType(string name) {
            return this.FindType(name, this.contextType.Namespace);
        }

        [ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // name parameter is a type name.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // SearchForType method call.
        internal Type FindType(string name, string defaultNamespace) {
            Type result = null;
            string name2 = null;
            @lock.AcquireReaderLock(Timeout.Infinite);
            try {
                if (this.types.TryGetValue(name, out result)) {
                    return result;
                }
                name2 = name.Contains(".") ? null : defaultNamespace + "." + name;
                if (name2 != null && this.types.TryGetValue(name2, out result)) {
                    return result;
                }
            }
            finally {
                @lock.ReleaseReaderLock();
            }
            // don't block anyone while we search for the correct type
            Type foundResult = this.SearchForType(name);

            if (foundResult == null && name2 != null) {
                foundResult = this.SearchForType(name2);
            }
            if (foundResult != null) {
                @lock.AcquireWriterLock(Timeout.Infinite);
                try {
                    if (this.types.TryGetValue(name, out result)) {
                        return result; 
                    }
                    this.types.Add(name, foundResult);
                    return foundResult;
                }
                finally {
                    @lock.ReleaseWriterLock();
                }
            }
            return null;
        }

        [ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // name parameter is a type name.
        [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // SearchForType method call.
        private Type SearchForType(string name) {
            // Try search for type using case sensitive.
            Type type = SearchForType(name, false);
            if (type != null) {
                return type;
            }

            // Try search for type using case in-sensitive.
            return SearchForType(name, true);
        }

       [ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // name parameter is a type name.
       [ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // Assembly.GetLoadedModules method call.
       private Type SearchForType(string name, bool ignoreCase) {
            // first try default system lookup
            Type type = Type.GetType(name, false, ignoreCase);
            if (type != null) {
                return type;
            }

            // try all known modules (modules that other statically known types were found in)
            foreach (Module module in this.modules) {
                type = module.GetType(name, false, ignoreCase);
                if (type != null) {
                    return type;
                }
            }

            // try all loaded modules (is there a better way?)
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (Module module in a.GetLoadedModules()) {
                    type = module.GetType(name, false, ignoreCase);
                    if (type != null) {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}