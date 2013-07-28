using MvcHelpers.Services;

namespace MvcHelpers.Tests.Fakes
{
	public class FakeFormsAuthenticationService : IFormsAuthenticationService
	{
		public void SignIn(string userName, bool createCookie) { }
		public void SignOut() { }
	}
}