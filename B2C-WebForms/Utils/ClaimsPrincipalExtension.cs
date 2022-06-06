﻿using System.Security.Claims;
using Microsoft.Identity.Client;

namespace B2C_WebForms.Utils
{
    public static class ClaimsPrincipalExtension
    {
        public const string ObjectId = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

        /// <summary>
        /// Get the B2C Account identifier for an MSAL.NET account from a ClaimsPrincipal
        /// </summary>
        /// <param name="claimsPrincipal">Claims principal</param>
        /// <returns>A string corresponding to an account identifier as defined in <see cref="Microsoft.Identity.Client.AccountId.Identifier"/></returns>
        public static string GetB2CMsalAccountId(this ClaimsPrincipal claimsPrincipal)
        {
            string userObjectId = GetObjectId(claimsPrincipal);
            string tenantId = Globals.TenantId;

            if (!string.IsNullOrWhiteSpace(userObjectId) && !string.IsNullOrWhiteSpace(tenantId))
            {
                return $"{userObjectId}.{tenantId}";
            }

            return null;
        }

        public static string GetB2CMsalAccountIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            string userObjectId = GetObjectId(claimsPrincipal);
            string tenantId = Globals.TenantId;

            if (!string.IsNullOrWhiteSpace(userObjectId) && !string.IsNullOrWhiteSpace(tenantId))
            {
                return $"{userObjectId}-{Globals.SignInPolicy}.{tenantId}";
            }

            return null;
        }

        /// <summary>
        /// Get the unique object ID associated with the claimsPrincipal
        /// </summary>
        /// <param name="claimsPrincipal">Claims principal from which to retrieve the unique object id</param>
        /// <returns>Unique object ID of the identity, or <c>null</c> if it cannot be found</returns>
        public static string GetObjectId(this ClaimsPrincipal claimsPrincipal)
        {
            var objIdclaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (objIdclaim == null)
            {
                objIdclaim = claimsPrincipal.FindFirst("sub");
            }

            return objIdclaim != null ? objIdclaim.Value : string.Empty;
        }

        /// <summary>
        /// Builds a ClaimsPrincipal from an IAccount
        /// </summary>
        /// <param name="account">The IAccount instance.</param>
        /// <returns>A ClaimsPrincipal built from IAccount</returns>
        public static ClaimsPrincipal ToClaimsPrincipal(this IAccount account)
        {
            if (account != null)
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(ObjectId, account.HomeAccountId.ObjectId));
                identity.AddClaim(new Claim(TenantId, account.HomeAccountId.TenantId));
                identity.AddClaim(new Claim(ClaimTypes.Upn, account.Username));
                return new ClaimsPrincipal(identity);
            }

            return null;
        }
    }
}