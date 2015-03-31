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
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace $rootnamespace$
{
    public static class UmbracoADAuthExtensions
    {

        ///  <summary>
        ///  Configure ActiveDirectory sign-in
        ///  </summary>
        ///  <param name="app"></param>
        ///  <param name="tenant"></param>
        ///  <param name="clientId"></param>
        ///  <param name="postLoginRedirectUri">
        ///  The URL that will be redirected to after login is successful, example: http://mydomain.com/umbraco/;
        ///  </param>
        ///  <param name="appKey"></param>
        ///  <param name="authType">
        ///  This by default is 'OpenIdConnect' but that doesn't match what ASP.Net Identity actually stores in the
        ///  loginProvider field in the database which looks something like this (for example): 
        ///      https://sts.windows.net/3bb0b4c5-364f-4394-ad36-0f29f95e5ggg/
        ///  and is based on your AD setup. This value needs to match in order for accounts to 
        ///  detected as linked/un-linked in the back office. 
        ///  </param>
        /// <param name="caption"></param>
        /// <param name="style"></param>
        /// <param name="icon"></param>
        /// <remarks>
        ///  
        ///  Nuget installation:
        ///      Install-Package Microsoft.Owin.Security.OpenIdConnect
        ///      Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory
        /// 
        ///  ActiveDirectory account documentation for ASP.Net Identity can be found:
        ///  
        ///  https://github.com/AzureADSamples/WebApp-WebAPI-OpenIDConnect-DotNet
        /// 
        ///  This configuration requires the NaiveSessionCache class below which will need to be un-commented
        /// 
        ///  </remarks>
        public static void ConfigureBackOfficeActiveDirectoryAuth(this IAppBuilder app, 
            string tenant, string clientId, string postLoginRedirectUri, string appKey,
            string authType,
            string caption = "Active Directory", string style = "btn-microsoft", string icon = "fa-windows")
        {         
            const string aadInstance = "https://login.windows.net/{0}";
            const string graphResourceId = "https://graph.windows.net";

            var authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
            var adOptions = new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = authType,
                SignInAsAuthenticationType = Constants.Security.BackOfficeExternalAuthenticationType,
                ClientId = clientId,
                Authority = authority,
                PostLogoutRedirectUri = postLoginRedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                    AuthorizationCodeReceived = (context) =>
                    {
                        var credential = new ClientCredential(clientId, appKey);
                        var userObjectId = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                        var authContext = new AuthenticationContext(authority, new NaiveSessionCache(userObjectId));
                        var result = authContext.AcquireTokenByAuthorizationCode(
                            context.Code,
                            //NOTE: This URL needs to match EXACTLY the same path that is configured in the AD configuration. 
                            new Uri(
                                HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                HttpContext.Current.Request.RawUrl.EnsureStartsWith('/').EnsureEndsWith('/')),
                            credential,
                            graphResourceId);

                        return Task.FromResult(0);
                    }

                }

            };
            adOptions.ForUmbracoBackOffice(style, icon);
            adOptions.Caption = caption;
            app.UseOpenIdConnectAuthentication(adOptions);
        }    

    }
    
    /// <summary>
    /// A Session cache token storage which is required to initialize the AD Identity provider on startup
    /// </summary>
    /// <remarks>    
    /// Based on the examples from the AD samples: 
    /// https://github.com/AzureADSamples/WebApp-WebAPI-OpenIDConnect-DotNet/blob/master/TodoListWebApp/Utils/NaiveSessionCache.cs
    /// 
    /// There are some newer examples of different token storage including persistent storage here:
    /// https://github.com/OfficeDev/O365-WebApp-SingleTenant/blob/master/O365-WebApp-SingleTenant/Models/ADALTokenCache.cs
    /// 
    /// The type of token storage will be dependent on your requirements but this should be fine for standard installations
    /// </remarks>
    public class NaiveSessionCache : TokenCache
    {
        private static readonly object FileLock = new object();
        readonly string _cacheId;
        public NaiveSessionCache(string userId)
        {
            _cacheId = userId + "_TokenCache";

            AfterAccess = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
            Load();
        }

        public void Load()
        {
            lock (FileLock)
            {
                Deserialize((byte[])HttpContext.Current.Session[_cacheId]);
            }
        }

        public void Persist()
        {
            lock (FileLock)
            {
                // reflect changes in the persistent store
                HttpContext.Current.Session[_cacheId] = Serialize();
                // once the write operation took place, restore the HasStateChanged bit to false
                HasStateChanged = false;
            }
        }

        // Empties the persistent store.
        public override void Clear()
        {
            base.Clear();
            HttpContext.Current.Session.Remove(_cacheId);
        }

        public override void DeleteItem(TokenCacheItem item)
        {
            base.DeleteItem(item);
            Persist();
        }

        // Triggered right before ADAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load();
        }

        // Triggered right after ADAL accessed the cache.
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (HasStateChanged)
            {
                Persist();
            }
        }
    }
    
}