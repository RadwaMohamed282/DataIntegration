using System.Net.Http.Headers;

namespace DeliveryIntegration.Services.HttpRequest
{
    public interface IHttpRequestService
    {

        Task<HttpResponseMessage> SendHttpRequest(HttpMethod method, string url,
           Dictionary<string, string> headers = null,
           HttpContent content = null,
           AuthenticationHeaderValue authHeader = null);
    }
}