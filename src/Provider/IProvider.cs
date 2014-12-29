using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Common;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider {

    /// <summary>
    /// A data provider implements this interface to hook into the LINQ to SQL framework.
    /// </summary>
    internal interface IProvider : IDisposable {
        /// <summary>
        /// Initializes the database provider with the data services object and connection.
        /// </summary>
        /// <param name="dataServices"></param>
        /// <param name="connection">A connection string, connection object or transaction object 
        /// used to seed the provider with database connection information.</param>
        void Initialize(IDataServices dataServices, object connection);

        /// <summary>
        /// The text writer used by the provider to output information such as query and commands
        /// being executed.
        /// </summary>
        TextWriter Log { get; set; }

        /// <summary>
        /// The connection object used by the provider when executing queries and commands.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// The transaction object used by the provider when executing queries and commands.
        /// </summary>
        DbTransaction Transaction { get; set; }

        /// <summary>
        /// The command timeout setting to use for command execution.
        /// </summary>
        int CommandTimeout { get; set; }

        /// <summary>
        /// Clears the connection of any current activity.
        /// </summary>
        void ClearConnection();

        /// <summary>
        /// Creates a new database instance (catalog or file) at the location specified by the connection
        /// using the metadata encoded within the entities or mapping file.
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// Deletes the database instance at the location specified by the connection.
        /// </summary>
        void DeleteDatabase();

        /// <summary>
        /// Returns true if the database specified by the connection object exists.
        /// </summary>
        /// <returns></returns>
        bool DatabaseExists();

        /// <summary>
        /// Executes the query specified as a LINQ expression tree.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A result object from which you can obtain the return value and output parameters.</returns>
        IExecuteResult Execute(Expression query);

        /// <summary>
        /// Compiles the query specified as a LINQ expression tree.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A compiled query instance.</returns>
        ICompiledQuery Compile(Expression query);

        /// <summary>
        /// Translates a DbDataReader into a sequence of objects (entity or projection) by mapping
        /// columns of the data reader to object members by name.
        /// </summary>
        /// <param name="elementType">The type of the resulting objects.</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        IEnumerable Translate(Type elementType, DbDataReader reader);

        /// <summary>
        /// Translates an IDataReader containing multiple result sets into sequences of objects
        /// (entity or projection) by mapping columns of the data reader to object members by name.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        IMultipleResults Translate(DbDataReader reader);

        /// <summary>
        /// Returns the query text in the database server's native query language
        /// that would need to be executed to perform the specified query.
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns></returns>
        string GetQueryText(Expression query);

        /// <summary>
        /// Return an IDbCommand object representing the translation of specified query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DbCommand GetCommand(Expression query);
    }
}