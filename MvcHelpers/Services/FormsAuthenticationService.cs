using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace MvcHelpers.Services
{
	public interface IFormsAuthenticationService
	{
		void SignIn(string userName, bool createCookie);
		void SignOut();
	}

	// Wrapper around static ASP.NET classes making them usable in MVC controllers 
	// while not making them difficult to test. A "Fake" implementation is included 
	// below for use in Unit Testing controllers that consume this service.
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
		public void SignIn(string email, bool createCookie)
        {
			FormsAuthentication.SetAuthCookie(email, createCookie);
			FormsAuthentication.RedirectFromLoginPage(email, createCookie);			
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    // For Testing
	public class FormsAuthenticationFake : IFormsAuthenticationService
	{
		public void SignIn(string userName, bool createCookie) { }
		public void SignOut() { }
	}
}
