using B2C_WebForms.Utils;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace B2C_WebForms
{
    public partial class Claims : System.Web.UI.Page
    {
        protected string FirstName { get; set; }
        protected string LastName { get; set; }

        protected string ApiResponse { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Extract claims about the user
            if (Request.IsAuthenticated)
            {
                FirstName = ClaimsPrincipal.Current.Identities.First().Name;
            }
        }

        protected async void buttonApiCall_Click(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                try
                {
                    // Retrieve the token with the specified scopes
                    AuthenticationResult result = await AcquireTokenForScopes(new string[] { Globals.ApiScope });

                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Globals.ServiceUrl);

                    // Add token to the Authorization header and make the request
                    request.Headers.Accept.Clear();
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Add("Ocp-Apim-Subscription-Key", $"{Globals.SubscriptionKey}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Handle the response
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            string responseString = await response.Content.ReadAsStringAsync();
                            ApiResponse = responseString;
                            break;
                        case HttpStatusCode.Unauthorized:
                            ApiResponse = "Please sign in again. " + response.ReasonPhrase;
                            break;
                        default:
                            ApiResponse = "Error. Status code = " + response.StatusCode + ": " + response.ReasonPhrase;
                            break;
                    }
                }
                catch (MsalUiRequiredException)
                {
                    /*
                        If the tokens have expired or become invalid for any reason, ask the user to sign in again.
                        Another cause of this exception is when you restart the app using InMemory cache.
                        It will get wiped out while the user will be authenticated still because of their cookies, requiring the TokenCache to be initialized again
                        through the sign in flow.
                    */
                    
                }
                catch (Exception ex)
                {
                    //log error
                }
            }
        }

        private async Task<AuthenticationResult> AcquireTokenForScopes(string[] scopes)
        {
            IConfidentialClientApplication cca = MsalAppBuilder.BuildConfidentialClientApplication();
            var account = await cca.GetAccountAsync(ClaimsPrincipal.Current.GetB2CMsalAccountIdentifier());
            return await cca.AcquireTokenSilent(scopes, account).ExecuteAsync().ConfigureAwait(false);
        }
    }
}