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
using Microsoft.Owin.Security.Facebook;

namespace $rootnamespace$
{
    public static class UmbracoFacebookAuthExtensions
    {
        ///  <summary>
        ///  Configure facebook sign-in
        ///  </summary>
        ///  <param name="app"></param>
        ///  <param name="appId"></param>
        ///  <param name="appSecret"></param>
        /// <param name="caption"></param>
        /// <param name="style"></param>
        /// <param name="icon"></param>
        /// <remarks>
        ///  
        ///  Nuget installation:
        ///      Microsoft.Owin.Security.Facebook
        /// 
        ///  Facebook account documentation for ASP.Net Identity can be found:
        ///  
        ///  http://www.asp.net/web-api/overview/security/external-authentication-services#FACEBOOK
        ///  
        ///  Facebook apps can be created here:
        /// 
        ///  https://developers.facebook.com/
        ///  
        ///  </remarks>
        public static void ConfigureBackOfficeFacebookAuth(this IAppBuilder app, string appId, string appSecret,
            string caption = "Facebook", string style = "btn-facebook", string icon = "fa-facebook")
        {
            var fbOptions = new FacebookAuthenticationOptions
            {
                AppId = appId,
                AppSecret = appSecret,
                //In order to allow using different facebook providers on the front-end vs the back office,
                // these settings are very important to make them distinguished from one another.
                SignInAsAuthenticationType = Constants.Security.BackOfficeExternalAuthenticationType,
                //  By default this is '/signin-facebook', you will need to change that default value in your
                //  Facebook developer settings for your app in the Advanced settings under "Client OAuth Login"
                //  in the "Valid OAuth redirect URIs", specify the full URL, for example: http://mysite.com/umbraco-facebook-signin
                CallbackPath = new PathString("/umbraco-facebook-signin")
            };
            fbOptions.ForUmbracoBackOffice(style, icon);
            fbOptions.Caption = caption;
            app.UseFacebookAuthentication(fbOptions);
        }

    }
        
}