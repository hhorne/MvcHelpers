using System;
using System.Web;
using System.Web.SessionState;

namespace MvcHelpers.Services
{
	public interface ISessionCacheService : ICacheService
	{
		void Abandon();
		void Clear();
		void Remove(string cacheID);
	}

	public class SessionCacheService : ISessionCacheService
	{
		private HttpSessionState Session
		{
			get { return HttpContext.Current.Session; }
		}

		public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
		{
			T item = Session[cacheID] as T;
			if (item == null)
			{
				item = getItemCallback();
				Session.Add(cacheID, item);
			}

			return item;
		}

		// Destroys the session and the Session_OnEnd event is triggered.
		public void Abandon()
		{
			Session.Abandon();
		}

		// Removes all content. The session with the same key is still alive.
		public void Clear()
		{
			Session.Clear();
		}

		public void Remove(string cacheID)
		{
			Session.Remove(cacheID);
		}
	}
}