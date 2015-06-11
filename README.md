# Umbraco Identity Extensibility

An Umbraco add-on package that enables easy extensibility points for ASP.Net Identity integration with Umbraco.

## Installation

*Coming soon!*

This project contains a few packages:

#### UmbracoCms.IdentityExtensions

This is the core package which installs Identity extensisibiliy points. It includes a very simple OAuth token server which can be extended to suit you needs. It also contains documentation and codefiles to allow you to extend the authentication flow of the Umbraco back office.

    Install-Package UmbracoCms.IdentityExtensions

#### Third party extensions

All of these packages reference the above package: UmbracoCms.IdentityExtensions and are simple ways to enable third party auth extensions like Google and Facebook logins. Each package contains documentation and code files with documentation to show you how to integrate third party logins.

    Install-Package UmbracoCms.IdentityExtensions.Google
  
    Install-Package UmbracoCms.IdentityExtensions.Facebook
  
    Install-Package UmbracoCms.IdentityExtensions.Microsoft
  
    Install-Package UmbracoCms.IdentityExtensions.AzureActiveDirectory
  
  
