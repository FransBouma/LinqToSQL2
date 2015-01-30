namespace System.Data.Linq.Provider.Interfaces
{
	internal interface IObjectReaderSession : IConnectionUser, IDisposable {
		bool IsBuffered { get; }
		void Buffer();
	}
}