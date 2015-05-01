using System;
using System.Web.Cors;


namespace Umbraco.IdentityExtensions
{
    public sealed class BackOfficeAuthServerProviderOptions
    {
        /// <summary>
        /// Default options allows all request, CORS does not limit anything
        /// </summary>
        /// <param name="corsPolicy"></param>
        public BackOfficeAuthServerProviderOptions(CorsPolicy corsPolicy = null)
        {
            if (corsPolicy == null)
            {
                corsPolicy = new CorsPolicy()
                {
                    AllowAnyHeader = true,
                    AllowAnyMethod = true,
                    AllowAnyOrigin = true                    
                };
            }

            CorsPolicy = corsPolicy;
        }

        public CorsPolicy CorsPolicy { get; private set; }
    }
}
