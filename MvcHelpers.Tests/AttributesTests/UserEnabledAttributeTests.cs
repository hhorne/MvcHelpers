using System;
using System.Web.Mvc;
using MvcHelpers.Controllers;
using MvcHelpers.Services;
using Moq;
using MvcHelpers.Attributes;
using Xunit;

namespace MvcHelpers.Tests.AttributesTests
{
	public class UserEnabledAttributeTests
	{
		private readonly Mock<Controller> _BaseController = new Mock<Controller>();

		private readonly Mock<FakeFullyEnabledController<FakeUser>> _FullyEnabledController =
			new Mock<FakeFullyEnabledController<FakeUser>>();

		private readonly Mock<FakeUserAwareController<FakeUser>> _UserAwareController =
			new Mock<FakeUserAwareController<FakeUser>>();

		[Fact]
		public void Should_Throw_Exception_When_Calling_Controller_Doesnt_Implement_IUserAware()
		{
		    var exceptionMessage = "Calling Controller must implement IUserAwareController";
            var filterContext = new ResultExecutingContext { Controller = _BaseController.Object };
			var attribute = new UserEnabledAttribute(typeof (FakeUser), typeof (FakeUserDetails));

			var exception = Assert.Throws<Exception>(() => attribute.OnResultExecuting(filterContext));
            Assert.Equal(exceptionMessage, exception.Message);
		}

		[Fact]
		public void Should_Throw_Exception_When_UserModel_Differs_From_IUserAwareControllers_Type_Argument()
		{
            var exceptionMessage = "The User model type specified by the Controller and Attribute are not the same. And they should be.";
            var filterContext = new ResultExecutingContext();
			filterContext.Controller = new FakeUserAwareController<string>();
			var attribute = new UserEnabledAttribute(typeof (FakeUser), typeof (FakeUserDetails));

            var exception = Assert.Throws<Exception>(() => attribute.OnResultExecuting(filterContext));
            Assert.Equal(exceptionMessage, exception.Message);

		}

		[Fact]
		public void Should_Do_Nothing_When_CurrentUser_Is_Null()
		{
			var filterContext = new ResultExecutingContext { Controller = _UserAwareController.Object };
		    var attribute = new UserEnabledAttribute(typeof (FakeUser), typeof (FakeUserDetails));

			attribute.OnResultExecuting(filterContext);
			var details = filterContext.Controller.ViewBag.UserDetails;

			Assert.Null(details);
		}

		[Fact]
		public void Should_Set_ViewBag_Contents_When_Not_Implementing_ICachingController()
		{
			var filterContext = new ResultExecutingContext();
			filterContext.Controller = _UserAwareController.Object;
			_UserAwareController.SetupGet(c => c.CurrentUser)
				.Returns(new FakeUser
					{
						Name = "Test",
						RegistrationDate = DateTime.UtcNow
					});
			var attribute = new UserEnabledAttribute(typeof (FakeUser), typeof (FakeUserDetails));

			attribute.OnResultExecuting(filterContext);

            Assert.NotNull(filterContext.Controller.ViewBag.UserDetails);
            Assert.Equal("Test", filterContext.Controller.ViewBag.UserDetails.Name);
		}

		[Fact]
		public void Should_Set_ViewBag_Contents_When_Implementing_ICachingController()
		{
			var filterContext = new ResultExecutingContext();
			filterContext.Controller = _FullyEnabledController.Object;
			_FullyEnabledController.SetupGet(c => c.CurrentUser)
				.Returns(new FakeUser
					{
						Name = "Test",
						RegistrationDate = DateTime.MinValue
					});

			_FullyEnabledController.Setup(c => c.UserSession.Get(It.IsAny<string>(), It.IsAny<Func<dynamic>>()))
				.Returns(new FakeUserDetails
					{
						Name = "Test",
						RegistrationDate = DateTime.MinValue
					});
			var attribute = new UserEnabledAttribute(typeof (FakeUser), typeof (FakeUserDetails));

			attribute.OnResultExecuting(filterContext);

            Assert.NotNull(filterContext.Controller.ViewBag.UserDetails);
            Assert.Equal("Test", filterContext.Controller.ViewBag.UserDetails.Name); ;
		}
	}

	public class FakeUser
	{
		public string Name { get; set; }
		public DateTime RegistrationDate { get; set; }
	}

	public class FakeUserDetails
	{
		public string Name { get; set; }
		public DateTime RegistrationDate { get; set; }
	}

	public class FakeUserAwareController<T> : Controller, IUserAwareController<T> where T : class
	{
		public virtual T CurrentUser { get; set; } // Marking this property virtual so it can be overridden by Moq
	}

	public class FakeFullyEnabledController<T> : Controller, ICachingController, IUserAwareController<T> where T : class
	{
		public virtual T CurrentUser { get; set; }
		public virtual IApplicationCacheService ApplicationCache { get; set; }
		public virtual ISessionCacheService UserSession { get; set; }
	}
}