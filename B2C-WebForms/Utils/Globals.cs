using System.Configuration;

namespace B2C_WebForms.Utils
{
    public static class Globals
    {
        // App config settings
        public static string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        public static string TenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        public static string AadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        public static string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        // B2C policy identifiers
        public static string SignInPolicy = ConfigurationManager.AppSettings["ida:SignInPolicy"];
        public static string DefaultPolicy = SignInPolicy;

        // API
        public static string ApiScope = ConfigurationManager.AppSettings["api:Scope"];
        public static string[] Scopes = new string[] { "openid", ApiScope };
        public static string ServiceUrl = ConfigurationManager.AppSettings["api:Url"];
        public static string SubscriptionKey = ConfigurationManager.AppSettings["api:SubscriptionKey"];

        // Authorities
        public static string B2CAuthority = string.Format(AadInstance, Tenant, DefaultPolicy);
        public static string WellKnownMetadata = $"{AadInstance}/v2.0/.well-known/openid-configuration";

    }
}