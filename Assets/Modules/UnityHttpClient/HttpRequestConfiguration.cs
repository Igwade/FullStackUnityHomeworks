using Modules.UnityHttpClient.Interfaces;

namespace Modules.UnityHttpClient
{
    public class HttpRequestConfiguration<TResponse>
    {
        public HttpRequest HttpRequest { get; }
        public IHttpResponseMapper<TResponse> ResponseMapper { get; }
        public int RetryCount { get; }
        public int RetryDelayMilliseconds { get; }

        public HttpRequestConfiguration(
            HttpRequest httpRequest,
            IHttpResponseMapper<TResponse> responseMapper,
            int retryCount,
            int retryDelayMilliseconds)
        {
            HttpRequest = httpRequest;
            ResponseMapper = responseMapper;
            RetryCount = retryCount;
            RetryDelayMilliseconds = retryDelayMilliseconds;
        }
    }

}