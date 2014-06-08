using System;
using System.Web;
using System.Web.Caching;

namespace MvcHelpers.Services
{
	public interface IApplicationCacheService : ICacheService
	{
		T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies) where T : class;
		T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies, DateTime absoluteExpiration) where T : class;
		T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration) where T : class;
		T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback) where T : class;
	}

	public class ApplicationCacheService : IApplicationCacheService
	{
		protected Cache Cache
		{
			get { return HttpRuntime.Cache; }
		}

		public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
		{
			return Get(cacheID, getItemCallback, null, DateTime.UtcNow.AddSeconds(30), Cache.NoSlidingExpiration);
		}

		public T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies) where T : class
		{
			return Get(cacheID, getItemCallback, dependencies, DateTime.UtcNow.AddSeconds(30), Cache.NoSlidingExpiration);
		}

		public T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies, DateTime absoluteExpiration) where T : class
		{
			return Get(cacheID, getItemCallback, dependencies, absoluteExpiration, Cache.NoSlidingExpiration);
		}

		public T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration) where T : class
		{
			return Get(cacheID, getItemCallback, dependencies, absoluteExpiration, slidingExpiration, null);
		}

		public T Get<T>(string cacheID, Func<T> getItemCallback, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback) where T : class
		{
			T item = Cache.Get(cacheID) as T;
			if (item == null)
			{
				item = getItemCallback();
				Cache.Insert(cacheID, item, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
			}

			return item;
		}
	}
}