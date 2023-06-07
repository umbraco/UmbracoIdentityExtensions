using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Owin;
using Umbraco.Core;
using Umbraco.Web.Security.Identity;
using Microsoft.Owin.Security.OpenIdConnect;

namespace $rootnamespace$
{
    public static class UmbracoADAuthExtensions
    {

        ///  <summary>
        ///  Configure ActiveDirectory sign-in
        ///  </summary>
        ///  <param name="app"></param>
        ///  <param name="tenant">
        ///  Your tenant ID i.e. YOURDIRECTORYNAME.onmicrosoft.com
        ///  </param>
        ///  <param name="clientId">
        ///  Also known as the Application Id in the azure portal
        ///  </param>
        ///  <param name="postLoginRedirectUri">
        ///  The URL that will be redirected to after login is successful, example: http://mydomain.com/umbraco/;
        ///  </param>
        ///  <param name="issuerId">
        /// 
        ///  This is the "Issuer Id" for you Azure AD application. This a GUID value and can be found
        ///  in the Azure portal when viewing your configured application and clicking on 'View endpoints'
        ///  which will list all of the API endpoints. Each endpoint will contain a GUID value, this is
        ///  the Issuer Id which must be used for this value.        
        /// 
        ///  If this value is not set correctly then accounts won't be able to be detected 
        ///  for un-linking in the back office. 
        /// 
        ///  </param>
        /// <param name="caption"></param>
        /// <param name="style"></param>
        /// <param name="icon"></param>
        /// <remarks>
        ///  ActiveDirectory account documentation for ASP.Net Identity can be found:
        ///  https://github.com/AzureADSamples/WebApp-WebAPI-OpenIDConnect-DotNet
        ///  </remarks>
        public static void ConfigureBackOfficeAzureActiveDirectoryAuth(this IAppBuilder app, 
            string tenant, string clientId, string clientSecret, string postLoginRedirectUri, Guid issuerId,
            string caption = "Active Directory", string style = "btn-microsoft", string icon = "fa-windows")
        {         
            var authority = string.Format(
                CultureInfo.InvariantCulture, 
                "https://login.windows.net/{0}", 
                tenant);

            var adOptions = new OpenIdConnectAuthenticationOptions
            {
                SignInAsAuthenticationType = Constants.Security.BackOfficeExternalAuthenticationType,
                ClientId = clientId,
                ClientSecret=clientSecret,
                Authority = authority,
                RedirectUri = postLoginRedirectUri
            };

            adOptions.ForUmbracoBackOffice(style, icon);            
            adOptions.Caption = caption;
            //Need to set the auth tyep as the issuer path
            adOptions.AuthenticationType = string.Format(
                CultureInfo.InvariantCulture,
                "https://sts.windows.net/{0}/",
                issuerId);
            app.UseOpenIdConnectAuthentication(adOptions);            
        }

        [Obsolete("Usage of clientSecret is recommended!")]
        public static void ConfigureBackOfficeAzureActiveDirectoryAuth(this IAppBuilder app, 
            string tenant, string clientId,string clientSecret=null, string postLoginRedirectUri, Guid issuerId,
            string caption = "Active Directory", string style = "btn-microsoft", string icon = "fa-windows")
        {         
            ConfigureBackOfficeAzureActiveDirectoryAuth(app,tenant,clientId,clientSecret,
            postLoginRedirectUri,issuerId,
            caption,style,icon);
        }       

    }
    
}