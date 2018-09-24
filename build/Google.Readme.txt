  _    _ __  __ ____  _____           _____ ____  
 | |  | |  \/  |  _ \|  __ \    /\   / ____/ __ \ 
 | |  | | \  / | |_) | |__) |  /  \ | |   | |  | |
 | |  | | |\/| |  _ <|  _  /  / /\ \| |   | |  | |
 | |__| | |  | | |_) | | \ \ / ____ | |___| |__| |
  \____/|_|  |_|____/|_|  \_/_/    \_\_____\____/ 
                                                   
----------------------------------------------------

Umbraco extensibility code has been installed for integrating Google 
OAuth with ASP.Net Identity for Umbraco back office users
			
DOCUMENTATION, NOTES AND CODE have been installed to this file: 
"~/App_Start/UmbracoGoogleAuthExtensions.cs"
Please read for information on integrating Google OAuth providers 
for ASP.Net Identity for the Umbraco back office. For Google OAuth to work
you must enable the Google+ API in the Google developer console for your
site/project: https://console.developers.google.com/apis/api/plus.googleapis.com

The usage of this extension method in your OWIN startup class will look like this:

	app.ConfigureBackOfficeGoogleAuth(
		"YOUR_CLIENT_ID", 
		"YOUR_CLIENT_SECRET");


Files have been installed into your App_Start folder if you have a Web Application project 
or into App_Code if you have a Website project. 

All of these files include lots of code comments, documentation & notes to assist with extending
the ASP.Net Identity implementaion for back office users in Umbraco. For all 3rd party 
ASP.Net providers, their dependencies will need to be manually installed. See comments in the
following files for full details:

* UmbracoStandardOwinStartup.cs 	Includes code snippets to enable 3rd party ASP.Net Identity 
									providers to work with the Umbraco back office.
									To enable the 'UmbracoStandardOwinStartup', update the web.config
									appSetting "owin:appStartup" to be: "UmbracoStandardOwinStartup"

* UmbracoCustomOwinStartup			Includes code snippets to customize the Umbraco ASP.Net 
									Identity implementation for back office users as well as 
									snippets to enable 3rd party ASP.Net Identity providers to work.
									To enable the 'UmbracoCustomOwinStartup', update the web.config
									appSetting "owin:appStartup" to be: "UmbracoCustomOwinStartup"
