using System.Web.Mvc;
using MvcHelpers.Controllers;
using MvcHelpers.Services;

namespace MvcHelpers.Tests.Fakes
{
    public class FakeFullyEnabledController<T> : Controller, ICachingController, IUserAwareController<T> where T : class
    {
        public virtual T CurrentUser { get; set; }
        public virtual IApplicationCacheService ApplicationCache { get; set; }
        public virtual ISessionCacheService UserSession { get; set; }
    }
}