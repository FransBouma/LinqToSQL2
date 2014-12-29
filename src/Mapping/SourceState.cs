using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Data.Linq.BindingLists;

namespace System.Data.Linq {
    internal static class SourceState<T> {
        internal static readonly IEnumerable<T> Loaded = (IEnumerable<T>)new T[] { };
        internal static readonly IEnumerable<T> Assigned = (IEnumerable<T>)new T[] { };
    }
}
