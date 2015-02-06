using System.Collections.Generic;
using System.Data.Linq.Provider.Interfaces;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Common
{
	internal class ObjectReaderFactoryCache
	{
		#region Member Declarations
		int maxCacheSize;
		LinkedList<CacheInfo> list;
		#endregion

		#region Private Classes
		private class CacheInfo
		{
			internal Type elementType;
			internal Type dataReaderType;
			internal object mapping;
			internal DataLoadOptions options;
			internal SqlExpression projection;
			internal IObjectReaderFactory factory;
			public CacheInfo(Type elementType, Type dataReaderType, object mapping, DataLoadOptions options, SqlExpression projection, IObjectReaderFactory factory)
			{
				this.elementType = elementType;
				this.dataReaderType = dataReaderType;
				this.options = options;
				this.mapping = mapping;
				this.projection = projection;
				this.factory = factory;
			}
		}
		#endregion

		internal ObjectReaderFactoryCache(int maxCacheSize)
		{
			this.maxCacheSize = maxCacheSize;
			this.list = new LinkedList<CacheInfo>();
		}

		internal IObjectReaderFactory GetFactory(Type elementType, Type dataReaderType, object mapping, DataLoadOptions options, SqlExpression projection)
		{
			for(LinkedListNode<CacheInfo> info = this.list.First; info != null; info = info.Next)
			{
				if(elementType == info.Value.elementType &&
				   dataReaderType == info.Value.dataReaderType &&
				   mapping == info.Value.mapping &&
				   DataLoadOptions.ShapesAreEquivalent(options, info.Value.options) &&
				   SqlProjectionComparer.AreSimilar(projection, info.Value.projection)
					)
				{
					// move matching item to head of list to reset its lifetime
					this.list.Remove(info);
					this.list.AddFirst(info);
					return info.Value.factory;
				}
			}
			return null;
		}

		internal void AddFactory(Type elementType, Type dataReaderType, object mapping, DataLoadOptions options, SqlExpression projection, IObjectReaderFactory factory)
		{
			this.list.AddFirst(new LinkedListNode<CacheInfo>(new CacheInfo(elementType, dataReaderType, mapping, options, projection, factory)));
			if(this.list.Count > this.maxCacheSize)
			{
				this.list.RemoveLast();
			}
		}
	}
}