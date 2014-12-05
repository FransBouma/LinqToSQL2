using System;
using System.Data;
using System.Data.Common;

namespace System.Data.Linq.SqlClient {

    internal interface IConnectionManager {
        DbConnection UseConnection(IConnectionUser user);
        void ReleaseConnection(IConnectionUser user);
    }

	/// <summary>
	/// Interface for objects which use a connection at a given time T, e.g. an active data reader.
	/// </summary>
    internal interface IConnectionUser {
        void CompleteUse();
    }
}
