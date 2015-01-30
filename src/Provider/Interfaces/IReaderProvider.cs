namespace System.Data.Linq.Provider.Interfaces
{
	internal interface IReaderProvider : IProvider {
		IDataServices Services { get; }
		IConnectionManager ConnectionManager { get; }
	}
}