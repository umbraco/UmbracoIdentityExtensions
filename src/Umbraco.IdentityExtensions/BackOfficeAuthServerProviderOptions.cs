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
            CorsPolicy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true
            };
        }

        public CorsPolicy CorsPolicy { get; set; }
    }
}
