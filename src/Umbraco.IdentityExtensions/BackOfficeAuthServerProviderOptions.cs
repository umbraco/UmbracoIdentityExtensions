using System;
using System.Web.Cors;


namespace Umbraco.IdentityExtensions
{
    public sealed class BackOfficeAuthServerProviderOptions
    {
        /// <summary>
        /// Default options allows all request, CORS does not limit anything
        /// </summary>
        public BackOfficeAuthServerProviderOptions()
        {
            //These are the defaults that we know work but people can modify them
            // on startup if required.
            CorsPolicy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                SupportsCredentials = true
            };
        }

        public CorsPolicy CorsPolicy { get; set; }
    }
}
