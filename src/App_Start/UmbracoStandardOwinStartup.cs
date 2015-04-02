using Microsoft.Owin;
using Owin;
using Umbraco.Core;
using Umbraco.Core.Security;
using Umbraco.Web.Security.Identity;
using $rootnamespace$;

//To use this startup class, change the appSetting value in the web.config called 
// "owin:appStartup" to be "StandardUmbracoOwinStartup"

[assembly: OwinStartup("StandardUmbracoOwinStartup", typeof(StandardUmbracoOwinStartup))]

namespace $rootnamespace$
{
    /// <summary>
    /// The standard way to configure OWIN for Umbraco
    /// </summary>
    /// <remarks>
    /// The startup type is specified in appSettings under owin:appStartup - change it to "StandardUmbracoStartup" to use this class
    /// </remarks>
    public class StandardUmbracoOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            //ensure the default options are configured
            base.Configuration(app);

            /* 
             * Configure external logins for the back office:
             * 
             * Depending on the authentication sources you would like to enable, you will need to install 
             * certain Nuget packages. 
             * 
             * For Google auth:             Install-Package UmbracoCms.Identity.Google
             * For Facebook auth:           Install-Package UmbracoCms.Identity.Facebook
             * For Microsoft auth:          Install-Package UmbracoCms.Identity.MicrosoftAccount
             * For ActiveDirectory auth:    Install-Package UmbracoCms.Identity.ActiveDirectory
             * 
             * There are many more providers such as Twitter, Yahoo, ActiveDirectory, etc... most information can
             * be found here: http://www.asp.net/web-api/overview/security/external-authentication-services
             * 
             * For sample code on using external providers with the Umbraco back office, install one of the 
             * packages listed above to review it's code samples 
             *  
             */

        }
    }
}
