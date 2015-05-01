using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Umbraco.Core.Security;

namespace Umbraco.IdentityExtensions
{
    /// <summary>
    /// A simple OAuth server provider to verify back office users
    /// </summary>
    public class BackOfficeAuthServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly BackOfficeAuthServerProviderOptions _options;

        public BackOfficeAuthServerProvider(BackOfficeAuthServerProviderOptions options = null)
        {
            if (options == null)
                options = new BackOfficeAuthServerProviderOptions();
            _options = options;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<BackOfficeUserManager>();

            var accessControlRequestMethodHeaders = context.Request.Headers.GetCommaSeparatedValues(CorsConstants.AccessControlRequestMethod);
            var originHeaders = context.Request.Headers.GetCommaSeparatedValues(CorsConstants.Origin);
            var accessControlRequestHeaders = context.Request.Headers.GetCommaSeparatedValues(CorsConstants.AccessControlRequestMethod);
            var corsRequest = new CorsRequestContext
            {
                Host = context.Request.Host.Value,
                HttpMethod = context.Request.Method,
                Origin = originHeaders == null ? null : originHeaders.FirstOrDefault(),
                RequestUri = context.Request.Uri,
                AccessControlRequestMethod = accessControlRequestMethodHeaders == null ? null : accessControlRequestMethodHeaders.FirstOrDefault()
            };
            if (accessControlRequestHeaders != null)
            {
                foreach (var header in context.Request.Headers.GetCommaSeparatedValues(CorsConstants.AccessControlRequestMethod))
                {
                    corsRequest.AccessControlRequestHeaders.Add(header);
                }    
            }
            
            var engine = new CorsEngine();
            var result = engine.EvaluatePolicy(corsRequest, _options.CorsPolicy);

            if (result.IsValid)
            {
                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var identity = await userManager.ClaimsIdentityFactory.CreateAsync(userManager, user, context.Options.AuthenticationType);

                context.Validated(identity);
            }
            
        }
    }
}