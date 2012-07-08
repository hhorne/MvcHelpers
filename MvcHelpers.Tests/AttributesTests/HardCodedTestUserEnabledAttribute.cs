using System.Web.Mvc;
using AutoMapper;
using MvcHelpers.Controllers;

namespace MvcHelpers.Tests.AttributesTests
{
	public class HardCodedTestUserEnabledAttribute : ActionFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			var userController = filterContext.Controller as IUserAwareController<FakeUser>;

            if (userController == null || userController.CurrentUser == null)
                return;

			var userDetails = Mapper.Map<FakeUser, FakeUserDetails>(userController.CurrentUser);
			filterContext.Controller.ViewBag.UserDetails = userDetails;
		}
	}
}
