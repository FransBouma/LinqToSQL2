using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Interfaces;
using System.Linq;

namespace System.Data.Linq.Provider.Common
{
	internal class MultipleResults : IMultipleResults, IDisposable
	{
		private IProvider provider;
		private IReaderProvider _readerProvider;
		private MetaFunction function;
		private IObjectReaderSession session;
		private bool isDisposed;
		private ExecuteResult executeResult;

		internal MultipleResults(IProvider provider, MetaFunction function, IObjectReaderSession session, ExecuteResult executeResult)
		{
			this.provider = provider;
			this.function = function;
			this.session = session;
			this.executeResult = executeResult;
			_readerProvider = provider as IReaderProvider;
		}

		public IEnumerable<T> GetResult<T>()
		{
			MetaType metaType = null;
			// Check the inheritance hierarchy of each mapped result row type
			// for the function.
			if(this.function != null)
			{
				foreach(MetaType mt in function.ResultRowTypes)
				{
					metaType = mt.InheritanceTypes.SingleOrDefault(it => it.Type == typeof(T));
					if(metaType != null)
					{
						break;
					}
				}
			}
			if(metaType == null)
			{
				if(_readerProvider == null)
				{
					throw Error.ArgumentTypeMismatch("provider");
				}
				metaType = _readerProvider.Services.Model.GetMetaType(typeof(T));
			}
			IObjectReaderFactory factory = this.provider.GetDefaultFactory(metaType);
			IObjectReader objReader = factory.GetNextResult(this.session, false);
			if(objReader == null)
			{
				this.Dispose();
				return null;
			}
			return new SingleResult<T>(new OneTimeEnumerable<T>((IEnumerator<T>)objReader), this.executeResult, _readerProvider.Services.Context);
		}

		public void Dispose()
		{
			if(!this.isDisposed)
			{
				// Technically, calling GC.SuppressFinalize is not required because the class does not
				// have a finalizer, but it does no harm, protects against the case where a finalizer is added
				// in the future, and prevents an FxCop warning.
				GC.SuppressFinalize(this);
				this.isDisposed = true;
				if(this.executeResult != null)
				{
					this.executeResult.Dispose();
				}
				else
				{
					this.session.Dispose();
				}
			}
		}

		public object ReturnValue
		{
			get
			{
				if(this.executeResult != null)
				{
					return executeResult.GetParameterValue("@RETURN_VALUE");
				}
				else
				{
					return null;
				}
			}
		}
	}
}