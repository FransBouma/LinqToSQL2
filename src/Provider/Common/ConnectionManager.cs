using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq.Provider.Interfaces;
using System.Linq.Expressions;
using System.IO;
using System.Reflection;
using System.Text;
using System.Transactions;

namespace System.Data.Linq.Provider.Common {
    using System.Data.Linq;

	internal class ConnectionManager : IConnectionManager {
        private IProvider provider;
        private DbConnection connection;
        private bool autoClose;
        private bool disposeConnection;  // should we dispose this connection when the context is disposed?
        private DbTransaction transaction;
        private Transaction systemTransaction;
        private List<IConnectionUser> users;
        private int maxUsers;

        internal ConnectionManager(IProvider provider, DbConnection con, int maxUsers, bool disposeConnection) {
            this.provider = provider;
            this.connection = con;
            this.maxUsers = maxUsers;
            this.users = new List<IConnectionUser>(maxUsers);
            this.disposeConnection = disposeConnection;
        }


        public DbConnection UseConnection(IConnectionUser user) {
            if (user == null) {
                throw Error.ArgumentNull("user");
            }
            if (this.connection.State == ConnectionState.Closed) {
                this.connection.Open();
                this.autoClose = true;
                this.AddInfoMessageHandler();
                if (System.Transactions.Transaction.Current != null) {
                    System.Transactions.Transaction.Current.TransactionCompleted += this.OnTransactionCompleted;
                }
            }
            if (this.transaction == null && System.Transactions.Transaction.Current != null &&
                        System.Transactions.Transaction.Current != systemTransaction) {
                this.ClearConnection();
                systemTransaction = System.Transactions.Transaction.Current;
                this.connection.EnlistTransaction(System.Transactions.Transaction.Current);
            }

            if (this.users.Count == this.maxUsers) {
                this.BootUser(this.users[0]);
            }
            this.users.Add(user);
            return this.connection;
        }

		/// <summary>
		/// Removes the specified connection using object from this connection.
		/// </summary>
		/// <param name="user"></param>
        private void BootUser(IConnectionUser user) {
            bool saveAutoClose = this.autoClose;
            this.autoClose = false;
            int index = this.users.IndexOf(user);
            if (index >= 0) {
                this.users.RemoveAt(index);
            }
            user.CompleteUse();
            this.autoClose = saveAutoClose;
        }

        internal DbConnection Connection {
            get { return this.connection; }
        }

        internal int MaxUsers {
            get { return this.maxUsers; }
        }

        internal void DisposeConnection() {
            // only close this guy if we opened it in the first place
            if (this.autoClose) {
                this.CloseConnection();
            }

            // If we created the connection, we need to dispose it even if the user explicitly
            // opened it using the Connection property on the context.
            if (this.connection != null && this.disposeConnection) {
                this.connection.Dispose();
                this.connection = null;
            }
        }

        internal void ClearConnection() {
            while (this.users.Count > 0) {
                this.BootUser(this.users[0]);
            }
        }

        internal bool AutoClose {
            get { return this.autoClose; }
            set { this.autoClose = value; }
        }

        internal DbTransaction Transaction {
            get { return this.transaction; }
            set {
                if (value != this.transaction) {
                    if (value != null) {
                        if (this.connection != value.Connection) {
                            throw Error.TransactionDoesNotMatchConnection();
                        }
                    }
                    this.transaction = value;
                }
            }
        }

        public void ReleaseConnection(IConnectionUser user) {
            if (user == null) {
                throw Error.ArgumentNull("user");
            }
            int index = this.users.IndexOf(user);
            if (index >= 0) {
                this.users.RemoveAt(index);
            }
            if (this.users.Count == 0 && this.autoClose && this.transaction == null && System.Transactions.Transaction.Current == null) {
                this.CloseConnection();
            }
        }

        protected virtual void CloseConnection() {
            if (this.connection != null && this.connection.State != ConnectionState.Closed) {
                this.connection.Close();
            }
            this.autoClose = false;
        }

		protected virtual void AddInfoMessageHandler()
		{
			// nop
		}


        private void OnTransactionCompleted(object sender, System.Transactions.TransactionEventArgs args) {
            if (this.users.Count == 0 && this.autoClose) {
                this.CloseConnection();
            }
		}

		#region Property Declarations
		protected IProvider Provider
		{
			get { return provider; }
		}
		#endregion
	}
}
