using Microsoft.Owin;
using Owin;
using Umbraco.Core.Security;
using Umbraco.Web;
using Umbraco.Web.Security;
using $rootnamespace$;

//To use this startup class, change the appSetting value in the web.config called 
// "owin:appStartup" to be "UmbracoCustomOwinStartup"

[assembly: OwinStartup("UmbracoCustomOwinStartup", typeof(UmbracoCustomOwinStartup))]

namespace $rootnamespace$
{
    /// <summary>
    /// A custom way to configure OWIN for Umbraco
    /// </summary>
    /// <remarks>
    /// <para>
    /// *** EXPERT *** 
    /// </para>
    /// </para>
    /// The startup type is specified in appSettings under owin:appStartup - change it to "UmbracoCustomOwinStartup" to use this class.
    /// This startup class would allows you to customize all aspects of the Umbraco OWIN startup/configuration pipeline.
    /// This example shows the common overloaded methods but there are others if you need to entirely change the OWIN startup/configuration pipeline.
    /// </para>
    /// </remarks>
    public class UmbracoCustomOwinStartup : UmbracoDefaultOwinStartup
    {
        /// <summary>
        /// Configures the <see cref="BackOfficeUserManager"/> for Umbraco
        /// </summary>
        /// <param name="app"></param>
        protected override void ConfigureUmbracoUserManager(IAppBuilder app)
        {
            // There are several overloads of this method that allow you to customize the BackOfficeUserManager or even custom BackOfficeUserStore.
            app.ConfigureUserManagerForUmbracoBackOffice(
                Services,
                Mapper,
                UmbracoSettings.Content,
                GlobalSettings,
                //The Umbraco membership provider needs to be specified in order to maintain backwards compatibility with the 
                // user password formats. The membership provider is not used for authentication, if you require custom logic
                // to validate the username/password against an external data source you can create create a custom UserManager
                // and override CheckPasswordAsync
                global::Umbraco.Core.Security.MembershipProviderExtensions.GetUsersMembershipProvider().AsUmbracoMembershipProvider());
        }

        /// <summary>
        /// Configures the back office authentication for Umbraco
        /// </summary>
        /// <param name="app"></param>
        protected override void ConfigureUmbracoAuthentication(IAppBuilder app)
        {
            app
                .UseUmbracoBackOfficeCookieAuthentication(UmbracoContextAccessor, RuntimeState, Services.UserService, GlobalSettings, UmbracoSettings.Security, PipelineStage.Authenticate)
                .UseUmbracoBackOfficeExternalCookieAuthentication(UmbracoContextAccessor, RuntimeState, GlobalSettings, PipelineStage.Authenticate)
                .UseUmbracoPreviewAuthentication(UmbracoContextAccessor, RuntimeState, GlobalSettings, UmbracoSettings.Security, PipelineStage.Authorize);

            /* 
             * Configure external logins for the back office:
             * 
             * Depending on the authentication sources you would like to enable, you will need to install 
             * certain Nuget packages. 
             * 
             * For Google auth:					Install-Package UmbracoCms.IdentityExtensions.Google
             * For Facebook auth:					Install-Package UmbracoCms.IdentityExtensions.Facebook
             * For Azure ActiveDirectory auth:		Install-Package UmbracoCms.IdentityExtensions.AzureActiveDirectory
             * 
             * There are many more providers such as Twitter, Yahoo, ActiveDirectory, etc... most information can
             * be found here: http://www.asp.net/web-api/overview/security/external-authentication-services
             * 
             * For sample code on using external providers with the Umbraco back office, install one of the 
             * packages listed above to review it's code samples 
             *  
             */

            /*
             * To configure a simple auth token server for the back office:
             *             
             * By default the CORS policy is to allow all requests
             * 
             *      app.UseUmbracoBackOfficeTokenAuth(new BackOfficeAuthServerProviderOptions());
             *      
             * If you want to have a custom CORS policy for the token server you can provide
             * a custom CORS policy, example: 
             * 
             *      app.UseUmbracoBackOfficeTokenAuth(
             *          new BackOfficeAuthServerProviderOptions()
             *              {
             *             		//Modify the CorsPolicy as required
             *                  CorsPolicy = new CorsPolicy()
             *                  {
             *                      AllowAnyHeader = true,
             *                      AllowAnyMethod = true,
             *                      Origins = { "http://mywebsite.com" }                
             *                  }
             *              });
             */
        }

    }
}
