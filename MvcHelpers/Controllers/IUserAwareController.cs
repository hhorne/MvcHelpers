using System.Web.Mvc;
using MvcHelpers.Attributes;

namespace MvcHelpers.Controllers
{	
	public interface IUserAwareController<T> where T : class
	{
		T CurrentUser { get; }
	}
}
