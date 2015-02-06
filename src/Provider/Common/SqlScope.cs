using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlScope
	{
		#region Member Declarations
		private SqlNode source;
		private SqlScope containing;
		#endregion

		internal SqlScope(SqlNode source, SqlScope containing)
		{
			this.source = source;
			this.containing = containing;
		}
		internal SqlNode Source
		{
			get { return this.source; }
		}
		internal SqlScope ContainingScope
		{
			get { return this.containing; }
		}
	}
}