namespace Modules.UnityHttpClient
{
    using System.Collections.Generic;

    public class HttpRequest
    {
        public string Method { get; }
        public string Url { get; }
        public Dictionary<string, string> Headers { get; }
        public byte[] Payload { get; }
        public string ContentType { get; }
        public int? Timeout { get; }

        public HttpRequest(string method, string url, Dictionary<string, string> headers, byte[] payload, string contentType, int? timeout)
        {
            Method = method;
            Url = url;
            Headers = headers;
            Payload = payload;
            ContentType = contentType;
            Timeout = timeout;
        }
    }

}