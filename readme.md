#MvcHelpers

This is a collection of code-bits that I think are testable and resusable across MVC projects. It is meant to provide a basic set of functionality and services to a new web project.

##Attributes

###UserEnabledAttribute (Inherits from ActionFilterAttribute)

If a controller implements **IUserAwareController&lt;T&gt;** you can decorate it with **UserEnabledAttribute** in order to automatically pass the current users information to views. This is achieved by accessing the calling controller during **OnResultExecuting** and converting the object representing the current using into an object representing the current users details and storing it in **ViewBag.UserDetails**.

**Example Usage** 

	[UserEnabled(typeof(Foo.Models.User), typeof(Foo.ViewModels.UserDetails))]
	public class HomeController : IUserAwareController<Foo.Models.User> ...

The first constructor argument is the domain model for your users and the second is the view model for how you want to represent your current user in your views. If the calling controller implements **ICachingService** it will cache the UserDetails in the SessionCache. 

##Controllers

###ICachingController
This simply enforces a contract that the controller will provide a mechanism for Application level and Session level caching. Mainly meant to work in conjunction with **IApplicationCacheService** and **ISessionCacheService**.

###IUserAwareController&lt;T&gt;

**IUserAwareController** exposes the currently logged in user through a public property where T is the domain model for your users. A controller must implement this in order to use a **UserEnabledAttribute** otherwise it will throw an exception.

##Data

###IUnitOfWork & UnitOfWork&lt;C&gt;

A derivation of the UnitOfWork / Repository pattern. **IUnitOfWork** is an interface for working against and saving objects. The **UnitOfWork** implementation is meant for working with EntityFramework Code First and takes a type argument of a **DbContext**. All methods on the **IUnitOfWork** interface take type arguments which allows for the flexibility to work against any domain model in the DbContext passed into **UnitOfWork** with a consistent and familiar set of methods.

##Services

###ICacheService

Provides a contract for a Get method in derived caching services. The Get method takes a string key value and a callback method to retrieve a value to be cached if one is not present for the given key.

###ApplicationCacheService

**ApplicaitonCacheService** acts as a wrapper around functionality found in **System.Web.Caching** and is meant for Application level data caching. The **IApplicationCacheService** interface is provided for IoC and Unit Testing.

###SessionCacheService

**SessionCacheService** acts as a wrapper around the  **HttpContext.Current.Session** object and is meant for user-session level data caching. The **ISessionCacheService** interface is provided for IoC and Unit Testing.

###FormsAuthenticationService

**FormsAuthenticationService** acts as a wrapper around static **FormsAuthentication** methods found in **System.Web.Security**. The **IFormsAuthenticationService** interface is provided for IoC and **FormsAuthenticationFake** for use in Unit Testing since the wrapper provides no meaningful functionality beyond calling **FormsAuthentication** methods.

###Sha256CryptoService

An implementation of Sha256 encryption for use in hashing and verifying user passwords.
