#MvcHelpers

This is a collection of code-bits that I think are testable and resusable across MVC projects. It is meant to provide a basic set of functionality and services to a new web project.

[![Build status](https://ci.appveyor.com/api/projects/status/7kdahyddwp6srdiy)](https://ci.appveyor.com/project/hhorne/mvchelpers)

##Data

###IUnitOfWork & UnitOfWork&lt;TContext&gt;

A derivation of the UnitOfWork / Repository pattern. **IUnitOfWork** is an interface for working against and saving objects. The **UnitOfWork** implementation is meant for working with EntityFramework Code First and takes a type argument of a **DbContext**. All methods on the **IUnitOfWork** interface take type arguments which allows for the flexibility to work against any domain model in the DbContext passed into **UnitOfWork** with a consistent and familiar set of methods.

##Email

###BrandedEmailSender

Email sender with brands (aka "themes") and template using Razor templating. BrandedMailSender stores the root directory for brands and messages in the **EmailPath** property with has a default value of **"~/Emails"**. The Brands folder is set in the **BrandedTemplateFolder** property with a default value of **"BrandTemplates"**. The message templates folder is set in **BrandedMessageFolder** property with a default value of **"Messages"**.

Reads SMTP settings from the following **app.config/web.config appSettings**

* **MailUsername**
* **MailPassword**
* **MailHost**
* **MailPort**

##Services

###ApplicationCacheService

**ApplicaitonCacheService** acts as a wrapper around functionality found in **System.Web.Caching** and is meant for Application level data caching. The **IApplicationCacheService** interface is provided for IoC and Unit Testing.

###SessionCacheService

**SessionCacheService** acts as a wrapper around the  **HttpContext.Current.Session** object and is meant for user-session level data caching. The **ISessionCacheService** interface is provided for IoC and Unit Testing.

###AppSettingsReader

Enables easy and flexible way of reading values from **appSettings** using dynamics.

Given an app.Config/web.config containing the following:

	<appSettings>
		<add key="ProxyHost" value="localhost:8080" />
	</appSettings>

This code will result in the console output "localhost:8080":

	var appSettings = new AppSettingsReader();
	var mailHost = appSettings.ProxyHost;
	Console.WriteLine(mailHost);

###FormsAuthenticationService

**FormsAuthenticationService** acts as a wrapper around static **FormsAuthentication** methods found in **System.Web.Security**. The **IFormsAuthenticationService** interface is provided for IoC and **FormsAuthenticationFake** for use in Unit Testing since the wrapper provides no meaningful functionality beyond calling **FormsAuthentication** methods.

###Sha256CryptoService

An implementation of Sha256 encryption for use in hashing and verifying user passwords.