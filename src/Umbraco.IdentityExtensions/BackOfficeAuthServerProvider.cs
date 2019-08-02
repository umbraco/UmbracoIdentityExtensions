using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Cors;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Umbraco.Web.Security;

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

        /// <summary>
        /// Called at the final stage of a successful Token endpoint request. An application may implement this call in order to do any final 
        ///             modification of the claims being used to issue access or refresh tokens. This call may also be used in order to add additional 
        ///             response parameters to the Token endpoint's json response body.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>        
        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            ProcessCors(context);

            return base.ValidateTokenRequest(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<BackOfficeUserManager>();
            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = await userManager.ClaimsIdentityFactory.CreateAsync(userManager, user, context.Options.AuthenticationType);

            context.Validated(identity);
            
        }

        private void ProcessCors(OAuthValidateTokenRequestContext context)
        {
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

            if (corsRequest.IsPreflight)
            {
                try
                {
                    // Make sure Access-Control-Request-Method is valid.
                    var test = new HttpMethod(corsRequest.AccessControlRequestMethod);
                }
                catch (ArgumentException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.SetError("Access Control Request Method Cannot Be Null Or Empty");
                    return;
                }
                catch (FormatException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.SetError("Invalid Access Control Request Method");
                    return;
                }

                var result = engine.EvaluatePolicy(corsRequest, _options.CorsPolicy);

                if (!result.IsValid)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.SetError(string.Join(" | ", result.ErrorMessages));
                    return;                    
                }

                WriteCorsHeaders(result, context);
            }
            else
            {
                var result = engine.EvaluatePolicy(corsRequest, _options.CorsPolicy);

                if (result.IsValid)
                {
                    WriteCorsHeaders(result, context);                    
                }
            }
        }

        private void WriteCorsHeaders(CorsResult result, OAuthValidateTokenRequestContext context)
        {
            var headers = result.ToResponseHeaders();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    context.Response.Headers.Append(header.Key, header.Value);
                }
            }
        }
    }
}
