using System;

namespace MvcHelpers.Services
{
	public interface ICacheService
	{
		T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;
	}
}
