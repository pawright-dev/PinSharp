using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PinSharp.Api;
using PinSharp.Extensions;

namespace PinSharp
{
    // TODO: Add Oauth exception classes and handling
    /// <summary>
    /// Static class used for getting an authorization URL and to get an access token from the code returned from Pinterest.
    /// </summary>
    public static class PinSharpAuthClient
    {
        private const string BaseUrl = "https://www.pinterest.com/";
        private const string ApiUrl = "https://api.pinterest.com/";

        /// <summary>
        /// <para>
        ///     Generates a login URL with the required parameters.
        ///     Users will need to visit this URL to authorize your app to use the API on their behalf.
        /// </para>
        /// <para>
        ///     If they accept they will be redirected to the <paramref name="redirectUri"/>
        ///     with two query string parameters - "state" and "code".
        /// </para>
        /// <para>
        ///     Call <see cref="BuildAuthorizationUrl(string, string, Scopes, string)"/> if you want to specify the state value yourself to be able to prevent spoofing.
        /// </para>
        /// <para>
        ///     "code" is used with <see cref="GetAccessTokenAsync"/> to
        ///     get an access token to use with <see cref="PinSharpClient"/>.
        /// </para>
        /// </summary>
        /// <param name="clientId">The Client ID (also known as App ID) of your app. See https://developers.pinterest.com/apps/</param>
        /// <param name="redirectUri">
        ///     The URL you want your user to be redirected to after authorizing your app.
        ///     The code needed for <see cref="GetAccessTokenAsync"/> will be added as query string parameter "code".
        /// </param>
        /// <param name="scopes">The scopes you want to request from the user.</param>
        /// <returns></returns>
        public static string BuildAuthorizationUrl(string clientId, string redirectUri, params Scopes[] scopes)
        {
            return BuildAuthorizationUrl(clientId, redirectUri, CreateRandomState(), scopes);
        }

        /// <summary>
        /// <para>
        ///     Generates a login URL with the required parameters.
        ///     Users will need to visit this URL to authorize your app to use the API on their behalf.
        /// </para>
        /// <para>
        ///     If they accept they will be redirected to the <paramref name="redirectUri"/>
        ///     with two query string parameters - "state" and "code".
        /// </para>
        /// <para>
        ///     "state" verifies that this comes from you.
        ///     "code" is used with <see cref="GetAccessTokenAsync"/> to
        ///     get an access token to use with <see cref="PinSharpClient"/>.
        /// </para>
        /// </summary>
        /// <param name="clientId">The Client ID (also known as App ID) of your app. See https://developers.pinterest.com/apps/</param>
        /// <param name="redirectUri">
        ///     The URL you want your user to be redirected to after authorizing your app.
        ///     The code needed for <see cref="GetAccessTokenAsync"/> will be added as query string parameter "code".
        /// </param>
        /// <param name="scopes">The scopes you want to request from the user.</param>
        /// <param name="state">A string that is added to <paramref name="redirectUri"/> as query string parameter "state". This is to prevent spoofing.</param>
        /// <returns></returns>
        public static string BuildAuthorizationUrl(string clientId, string redirectUri, string state, params Scopes[] scopes)
        {
            var scope = GetScope(scopes);

            return $"{BaseUrl}oauth/?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}&state={state}";
        }

        /// <summary>
        /// Gets an access token which you can then use with <see cref="PinSharpClient"/>.
        /// </summary>
        /// <param name="clientId">The Client ID (also known as App ID) of your app. See https://developers.pinterest.com/apps/</param>
        /// <param name="clientSecret">The Client secret (also known as App secret) of your app. See https://developers.pinterest.com/apps/</param>
        /// <param name="code">The code that was passed to your <c>redirectUri</c> as a query string parameter.</param>
        /// <param name="redirectUrl">The redirect URI needed for the pinterest API.</param>
        /// <returns>An access token for use with <see cref="PinSharpClient"/>.</returns>
        public static async Task<Models.IUser> GetAccessTokenAsync(string clientId, string clientSecret, string code, string redirectUrl)
        {
            var url = $"{ApiUrl}v5/oauth/token";

            var client = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret));
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var dict = new Dictionary<string, string>();
            dict.Add("code", code);
            dict.Add("grant_type", "authorization_code");
            dict.Add("redirect_uri", redirectUrl);
            var content = new FormUrlEncodedContent(dict);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadAsAsync<Models.User>().ConfigureAwait(false);
            return user;
        }

        /// <summary>
        /// Generates a random string that you can use to verify that
        /// the redirect back to your site or app wasn't spoofed.
        ///
        /// <para>
        ///     Pass this to <see cref="BuildAuthorizationUrl"/> to get the correct login URL.
        /// </para>
        /// </summary>
        /// <param name="length">The length of the random string.</param>
        /// <returns></returns>
        public static string CreateRandomState(int length = 10)
        {
            var data = new byte[length/2];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        private static string GetScope(params Scopes[] scopes)
        {
            var values = new List<string>();

            if (scopes.Contains(Scopes.AdsRead))
                values.Add("ads:read");

            if (scopes.Contains(Scopes.BoardsRead))
                values.Add("boards:read");

            if (scopes.Contains(Scopes.BoardsReadSecret))
                values.Add("boards:read_secret");

            if (scopes.Contains(Scopes.BoardsWrite))
                values.Add("boards:write");

            if (scopes.Contains(Scopes.BoardsWriteSecret))
                values.Add("boards:write_secret");

            if (scopes.Contains(Scopes.PinsRead))
                values.Add("pins:read");

            if (scopes.Contains(Scopes.PinsReadSecret))
                values.Add("pins:read_secret");

            if (scopes.Contains(Scopes.PinsWrite))
                values.Add("pins:write");

            if (scopes.Contains(Scopes.PinsWriteSecret))
                values.Add("pins:write_secret");

            if (scopes.Contains(Scopes.UserAccountsRead))
                values.Add("user_accounts:read");

            return string.Join(",", values);
        }
    }
}
