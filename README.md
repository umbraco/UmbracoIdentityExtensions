# Umbraco Identity Extensibility

An Umbraco add-on package that enables easy extensibility points for ASP.Net Identity integration with the Umbraco backoffice including a simple OAuth token server and snippets on how to incorporate custom OAuth authentication providers for the back office.

_This package is for back office **users** and is not for front-end members. For front-end members ASP.NET Identity support see this community project: https://github.com/shazwazza/umbracoidentity_

## Installation

This project contains a few packages:

#### UmbracoCms.IdentityExtensions

This is the core package which installs Identity extensisibiliy points. It includes a very simple OAuth token server which can be extended to suit you needs. It also contains documentation and codefiles to allow you to extend the authentication flow of the Umbraco back office.

    Install-Package UmbracoCms.IdentityExtensions

#### External login providers

The below packages will help you get started with External login providers (OAuth) for the Umbraco back office.

_Each package contains documentation and code files with documentation to show you how to integrate third party logins_

    Install-Package UmbracoCms.IdentityExtensions.Google

_Readme: https://github.com/umbraco/UmbracoIdentityExtensions/blob/master/build/Google.Readme.txt_

    Install-Package UmbracoCms.IdentityExtensions.Facebook
    
_Readme: https://github.com/umbraco/UmbracoIdentityExtensions/blob/master/build/Facebook.Readme.txt_     

    Install-Package UmbracoCms.IdentityExtensions.AzureActiveDirectory
    
_Readme: https://github.com/umbraco/UmbracoIdentityExtensions/blob/master/build/ActiveDirectory.Readme.txt_
  
What about [Identity Server](https://github.com/IdentityServer)? Yes that will of course work too, [here's a great community blog post](https://yuriburger.net/2017/04/26/login-to-umbraco-backoffice-using-identityserver4/) about setting up that integration. 
