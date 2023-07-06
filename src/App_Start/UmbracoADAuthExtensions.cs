using System;
using System.Globalization;
using Owin;
using Umbraco.Core;
using Umbraco.Web.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace $rootnamespace$
{
    public static class UmbracoADAuthExtensions
    {

        ///  <summary>
        ///  Configure ActiveDirectory sign-in
        ///  </summary>
        ///  <param name="app"></param>
        ///  <param name="tenant">
        ///  Your tenant ID i.e. YOURDIRECTORYNAME.onmicrosoft.com OR this could be the GUID of your tenant ID
        ///  </param>
        ///  <param name="clientId">
        ///  Also known as the Application Id in the azure portal
        ///  </param>
        ///  <param name="postLoginRedirectUri">
        ///  The URL that will be redirected to after login is successful, example: http://mydomain.com/umbraco/;
        ///  </param>
        ///  <param name="issuerId">
        /// 
        ///  This is the "Issuer Id" for you Azure AD application. This is a GUID value of your tenant ID.
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
        public static void ConfigureBackOfficeAzureActiveDirectoryAuthentication(this IAppBuilder app, 
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
                Authority = authority,
                RedirectUri = postLoginRedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthorizationCodeReceived = async n =>
                    {
                        if (clientSecret == null)
                        {
                            return;
                        }

#pragma warning disable IDE0063 // Use simple 'using' statement
                        using (HttpMessageHandler handler = new HttpClientHandler())
                        {
                            using (HttpMessageInvoker invoker = new HttpMessageInvoker(handler))
                            {
                                var tokenClient = new TokenClient(invoker, new TokenClientOptions()
                                {
                                    ClientId = clientId,
                                    ClientSecret = clientSecret,
                                    Address = $"{authority}/oauth2/token"
                                });

                                var tokenResponse = await tokenClient.RequestAuthorizationCodeTokenAsync(n.Code, postLoginRedirectUri);

                                if (tokenResponse.IsError)
                                {
                                    throw new Exception(tokenResponse.Error);
                                }

                                return;
                            }
                        }
#pragma warning restore IDE0063 // Use simple 'using' statement
                    }
                }
            };
            if (clientSecret != null)  
                adOptions.ClientSecret=clientSecret;

            adOptions.ForUmbracoBackOffice(style, icon);            
            adOptions.Caption = caption;
            //Need to set the auth type as the issuer path
            adOptions.AuthenticationType = string.Format(
                CultureInfo.InvariantCulture,
                "https://sts.windows.net/{0}/",
                issuerId);
            app.UseOpenIdConnectAuthentication(adOptions);            
        }

        [Obsolete("ConfigureBackOfficeAzureActiveDirectoryAuth has been deprecated , Please use ConfigureBackOfficeAzureActiveDirectoryAuthentication with clientSecret instead!")]
        public static void ConfigureBackOfficeAzureActiveDirectoryAuth(this IAppBuilder app, 
            string tenant, string clientId, string postLoginRedirectUri, Guid issuerId,
            string caption = "Active Directory", string style = "btn-microsoft", string icon = "fa-windows")
        {         
            ConfigureBackOfficeAzureActiveDirectoryAuthentication(app,tenant,clientId,null,
            postLoginRedirectUri,issuerId,
            caption,style,icon);
        }       

    }


    
}
