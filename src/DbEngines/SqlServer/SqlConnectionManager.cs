using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Provider.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal class SqlConnectionManager : ConnectionManager
	{

#warning REFACTORING CANDIDATE FOR #23
		#region Member Declarations
		private SqlInfoMessageEventHandler infoMessagehandler;
		#endregion


        internal SqlConnectionManager(IProvider provider, DbConnection con, int maxUsers, bool disposeConnection) 
			: base (provider, con, maxUsers, disposeConnection)
		{
            this.infoMessagehandler = new SqlInfoMessageEventHandler(this.OnInfoMessage);
        }


		protected override void CloseConnection()
		{
			base.CloseConnection();
            this.RemoveInfoMessageHandler();
		}

		
		protected override void AddInfoMessageHandler() {
            SqlConnection scon = this.Connection as SqlConnection;
            if (scon != null) {
                scon.InfoMessage += this.infoMessagehandler;
            }
        }


		private void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
		{
			if(this.Provider.Log != null)
			{
				this.Provider.Log.WriteLine(Strings.LogGeneralInfoMessage(args.Source, args.Message));
			}
		}
		

        private void RemoveInfoMessageHandler() {
            SqlConnection scon = this.Connection as SqlConnection;
            if (scon != null) {
                scon.InfoMessage -= this.infoMessagehandler;
            }
        }
	}
}
