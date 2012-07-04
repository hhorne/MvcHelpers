using MvcHelpers.Services;

namespace MvcHelpers.Controllers
{
	public interface ICachingController
	{
		IApplicationCacheService ApplicationCache { get; }
		ISessionCacheService UserSession { get; }
	}
}
