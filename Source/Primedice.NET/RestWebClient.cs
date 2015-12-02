using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KriPod.Primedice
{
    class RestWebClient
    {
        private static readonly HttpClient HttpClient;

        internal bool IsAuthorized => AuthToken != null;

        private string AuthToken { get; }

        static RestWebClient()
        {
            // Initialize the static HttpClient's handler, and add support for compression if possible
            var httpClientHandler = new HttpClientHandler();
            if (httpClientHandler.SupportsAutomaticDecompression) {
                httpClientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }

            // Initialize the static HttpClient using the handler specified above
            HttpClient = new HttpClient(httpClientHandler);
            HttpClient.DefaultRequestHeaders.ExpectContinue = false;
        }

        public RestWebClient(string authToken = null)
        {
            // Set the authentication token of the instance
            if (!string.IsNullOrEmpty(authToken)) {
                AuthToken = authToken;
            }
        }

        public Task<T> Get<T>(string command)
        {
            return RequestObject<T>(HttpMethod.Get, command);
        }

        public Task<T> Post<T>(string command, Dictionary<string, string> requestContent)
        {
            return RequestObject<T>(HttpMethod.Post, command, requestContent);
        }

        private async Task<T> RequestObject<T>(HttpMethod requestMethod, string command, Dictionary<string, string> requestContent = null)
        {
            // Query the server for a response
            var responseString = await SendRequest(requestMethod, command, requestContent);

            try {
                // Try to decode the JSON response
                return JsonConvert.DeserializeObject<T>(responseString);

            } catch {
                // The server throws exceptions as raw strings instead of JSON
                throw new PrimediceException(responseString);
            }
        }

        private async Task<string> SendRequest(HttpMethod requestMethod, string requestRelativeUrl, Dictionary<string, string> requestContent = null)
        {
            // Send an authenticated request if possible
            if (IsAuthorized) {
                requestRelativeUrl += "?access_token=" + AuthToken;
            }

            // Create the request message
            using (var requestMessage = new HttpRequestMessage(requestMethod, new Uri(Utils.ApiUrlBase + requestRelativeUrl))) {
                // Fill the request's content if needed
                if (requestContent != null) {
                    requestMessage.Content = new FormUrlEncodedContent(requestContent);
                }

                // Wait for the response, and then return it as a string
                var response = await HttpClient.SendAsync(requestMessage);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
