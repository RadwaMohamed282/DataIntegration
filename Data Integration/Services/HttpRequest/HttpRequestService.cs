using System.Net.Http.Headers;

namespace DeliveryIntegration.Services.HttpRequest
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly HttpClient _httpClient;

        public HttpRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> SendHttpRequest(HttpMethod method, string url,
         Dictionary<string, string> headers = null,
         HttpContent content = null,
         AuthenticationHeaderValue authHeader = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            request.Content = content;

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (authHeader != null)
            {
                request.Headers.Authorization = authHeader;
            }

            return await _httpClient.SendAsync(request);
        }

    }
}
