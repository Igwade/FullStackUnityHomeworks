namespace Modules.UnityHttpClient
{
    public static class HttpMethod
    {
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "PUT";
        public const string Delete = "DELETE";
        public const string Head = "HEAD";
        public const string Options = "OPTIONS";
        public const string Patch = "PATCH";
    }

    public static class Content
    {
        public const string Json = "application/json";
        public const string PlainText = "text/plain";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
        public const string MultipartFormData = "multipart/form-data";
    }
}