using System.Web.Mvc;
using MvcHelpers.Controllers;

namespace MvcHelpers.Tests.Fakes
{
    public class FakeUserAwareController<T> : Controller, IUserAwareController<T> where T : class
    {
        public virtual T CurrentUser { get; set; } // Marking this property virtual so it can be overridden by Moq
    }
}