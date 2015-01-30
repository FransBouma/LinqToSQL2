using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Data.Linq;

namespace System.Data.Linq.Provider.Interfaces
{

    internal interface IObjectReader : IEnumerator, IDisposable {
        IObjectReaderSession Session { get; }
    }
}
