namespace System.Data.Linq.Provider.Interfaces
{
	internal interface ICompiledSubQuery {
		IExecuteResult Execute(IProvider provider, object[] parentArgs, object[] userArgs);
	}
}