using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.BindingLists;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Result type for single rowset returning stored procedures.
	/// </summary>
	internal class SingleResult<T> : ISingleResult<T>, IDisposable, IListSource
	{
		private IEnumerable<T> enumerable;
		private ExecuteResult executeResult;
		private DataContext context;
		private IBindingList cachedList;

		internal SingleResult(IEnumerable<T> enumerable, ExecuteResult executeResult, DataContext context)
		{
			Diagnostics.Debug.Assert(enumerable != null);
			Diagnostics.Debug.Assert(executeResult != null);
			this.enumerable = enumerable;
			this.executeResult = executeResult;
			this.context = context;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return enumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public object ReturnValue
		{
			get
			{
#warning [FB] SQL SERVER SPECIFIC. NEEDS REFACTORING.
				return executeResult.GetParameterValue("@RETURN_VALUE");
			}
		}

		public void Dispose()
		{
			// Technically, calling GC.SuppressFinalize is not required because the class does not
			// have a finalizer, but it does no harm, protects against the case where a finalizer is added
			// in the future, and prevents an FxCop warning.
			GC.SuppressFinalize(this);
			this.executeResult.Dispose();
		}

		IList IListSource.GetList()
		{
			if(this.cachedList == null)
			{
				this.cachedList = BindingList.Create<T>(this.context, this);
			}
			return this.cachedList;
		}

		bool IListSource.ContainsListCollection
		{
			get { return false; }
		}
	}
}