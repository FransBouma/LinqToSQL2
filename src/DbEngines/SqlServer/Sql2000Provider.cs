namespace System.Data.Linq.DbEngines.SqlServer
{
	public sealed class Sql2000Provider : SqlProvider
	{
		public Sql2000Provider()
			: base(SqlServerProviderMode.Sql2000)
		{
		}
	}
}