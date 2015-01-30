namespace System.Data.Linq.Provider.Interfaces
{
	/// <summary>
	/// Interface for objects which use a connection at a given time T, e.g. an active data reader.
	/// </summary>
	internal interface IConnectionUser
	{
		void CompleteUse();
	}
}