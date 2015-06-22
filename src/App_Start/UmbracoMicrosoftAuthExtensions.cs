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
        ///  Nuget installation:
        ///      Microsoft.Owin.Security.MicrosoftAccount
        /// 
        ///  Microsoft account documentation for ASP.Net Identity can be found:
        ///  
        ///  http://www.asp.net/web-api/overview/security/external-authentication-services#MICROSOFT
        ///  http://blogs.msdn.com/b/webdev/archive/2012/09/19/configuring-your-asp-net-application-for-microsoft-oauth-account.aspx
        ///  
        ///  Microsoft apps can be created here:
        ///  
        ///  http://go.microsoft.com/fwlink/?LinkID=144070
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
