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
using Umbraco.Web.Security;
using Microsoft.Owin.Security.MicrosoftAccount;

namespace $rootnamespace$
{
    public static class UmbracoMicrosoftAuthExtensions
    {
        ///  <summary>
        ///  Configure microsoft account sign-in
        ///  </summary>
        ///  <param name="app"></param>
        ///  <param name="clientId"></param>
        ///  <param name="clientSecret"></param>
        /// <param name="caption"></param>
        /// <param name="style"></param>
        /// <param name="icon"></param>
        /// <remarks>
        /// 
        ///  Microsoft account documentation for ASP.Net Identity can be found:
        ///  
        ///  http://www.asp.net/web-api/overview/security/external-authentication-services#MICROSOFT
        ///  
        ///  Microsoft apps can be created here:
        ///  
        ///  https://go.microsoft.com/fwlink/?linkid=2083908
        ///
        ///  NOTE: Apps are now based on Azure AD in the Azure Portal where you can assign a client secret to your app
        ///  
        ///  </remarks>
        public static void ConfigureBackOfficeMicrosoftAuth(this IAppBuilder app, string clientId, string clientSecret,
            string caption = "Microsoft", string style = "btn-microsoft", string icon = "fa-windows")
        {
            var msOptions = new MicrosoftAccountAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                SignInAsAuthenticationType = Constants.Security.BackOfficeExternalAuthenticationType,
                CallbackPath = new PathString("/umbraco-microsoft-signin")
            };
            msOptions.ForUmbracoBackOffice(style, icon);
            msOptions.Caption = caption;
            app.UseMicrosoftAccountAuthentication(msOptions);
        }

    }
        
}
