namespace System.Data.Linq.DbEngines.SqlServer
{
	public sealed class Sql2005Provider : SqlProvider
	{
		public Sql2005Provider()
			: base(SqlServerProviderMode.Sql2005)
		{
		}
	}
}