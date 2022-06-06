using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.Distributed;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace B2C_WebForms.Utils
{
    public static class MsalAppBuilder
    {
        /// <summary>
        /// Shared method to create an IConfidentialClientApplication from configuration and attach the application's token cache implementation
        /// </summary>
        /// <returns></returns>
        public static IConfidentialClientApplication BuildConfidentialClientApplication()
        {
            return BuildConfidentialClientApplication(ClaimsPrincipal.Current);
        }

        /// <summary>
        /// Shared method to create an IConfidentialClientApplication from configuration and attach the application's token cache implementation
        /// </summary>
        /// <param name="currentUser">The current ClaimsPrincipal</param>
        public static IConfidentialClientApplication BuildConfidentialClientApplication(ClaimsPrincipal currentUser)
        {
            IConfidentialClientApplication clientapp = ConfidentialClientApplicationBuilder.Create(Globals.ClientId)
                  .WithClientSecret(Globals.ClientSecret)
                  .WithRedirectUri(Globals.RedirectUri)
                  .WithB2CAuthority(Globals.B2CAuthority)
                  .Build();

            clientapp.AddDistributedTokenCache(services =>
            {
                // Do not use DistributedMemoryCache in production!
                // This is a memory cache which is not distributed and is not persisted.
                // It's useful for testing and samples, but in production use a durable distributed cache,
                // such as Redis.
                services.AddDistributedMemoryCache();

                // The setting below shows encryption which works on a single machine. 
                // In a distributed system, the encryption keys must be shared between all machines
                // For details see https://github.com/AzureAD/microsoft-identity-web/wiki/L1-Cache-in-Distributed-(L2)-Token-Cache#distributed-systems
                services.Configure<MsalDistributedTokenCacheAdapterOptions>(o =>
                {
                    o.Encrypt = true;
                });
            });

            return clientapp;
        }

        /// <summary>
        /// Common method to remove the cached tokens for the currently signed in user
        /// </summary>
        /// <returns></returns>
        public static async Task ClearUserTokenCache()
        {
            IConfidentialClientApplication clientapp = ConfidentialClientApplicationBuilder.Create(Globals.ClientId)
                .WithB2CAuthority(Globals.B2CAuthority)
                .WithClientSecret(Globals.ClientSecret)
                .WithRedirectUri(Globals.RedirectUri)
                .Build();

            // We only clear the user's tokens.
            MSALPerUserMemoryTokenCache userTokenCache = new MSALPerUserMemoryTokenCache(clientapp.UserTokenCache);

            //Remove the users from the MSAL's internal cache
            await clientapp.RemoveAsync(await clientapp.GetAccountAsync(ClaimsPrincipal.Current.GetB2CMsalAccountIdentifier()));

            userTokenCache.Clear();
        }
    }
}