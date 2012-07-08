using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using MvcHelpers.Controllers;
using MvcHelpers.Services;

namespace MvcHelpers.Attributes
{
	public class UserEnabledAttribute : ActionFilterAttribute
	{
	    private readonly ITypeMapService _TypeMapService;
	    private readonly Type _UserModel;
		private readonly Type _UserDetails;
        
		public UserEnabledAttribute(Type userModel, Type userDetails, ITypeMapService typeService)
		{
		    _TypeMapService = typeService;
		    _UserModel = userModel;
			_UserDetails = userDetails;

            if (_TypeMapService.TypeMapExists(_UserModel, _UserDetails) == false)
            {
                _TypeMapService.CreateMap(userModel, userDetails);
            }
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			var controller = filterContext.Controller;
			var userInterface = controller.GetType().GetInterfaces().SingleOrDefault(i => i.Name.Contains("IUserAwareController"));
			
			if (userInterface == null)
			{
				throw new Exception("Calling Controller must implement IUserAwareController");
			}

			if (userInterface.GenericTypeArguments.Contains(_UserModel) == false)
			{
				throw new Exception("The User model type specified by the Controller and Attribute are not the same. And they should be.");
			}

			dynamic userController = controller;
			
			if (userController.CurrentUser != null)
			{				
				var cachingController = controller as ICachingController;
			
				if (cachingController == null) // If caching services aren't available, just make the request/conversion each time.
				{
                    controller.ViewBag.UserDetails = GetUserDetails(userController.CurrentUser);
				}
				else
				{
                    controller.ViewBag.UserDetails = cachingController.UserSession.Get("UserBadge", () => GetUserDetails(userController.CurrentUser));
				}
			}

			base.OnResultExecuting(filterContext);
		}

        private dynamic GetUserDetails<T>(T user)
        {
            return _TypeMapService.Map(user, _UserModel, _UserDetails);
        }
	}
}