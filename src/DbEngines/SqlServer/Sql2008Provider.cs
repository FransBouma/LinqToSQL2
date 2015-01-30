namespace System.Data.Linq.DbEngines.SqlServer
{
	public sealed class Sql2008Provider : SqlProvider
	{
		public Sql2008Provider()
			: base(SqlServerProviderMode.Sql2008)
		{
		}
	}
}