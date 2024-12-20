using System;
using Modules.UnityHttpClient.Interfaces;

namespace Modules.UnityHttpClient.CommonSerializers.PlainText
{
    public class TextResponseMapper<TResponse> : IHttpResponseMapper<TResponse>
    {
        public TResponse Map(string response)
        {
            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)response;
            }

            try
            {
                var converted = Convert.ChangeType(response, typeof(TResponse));
                return (TResponse)converted;
            }
            catch
            {
                throw new InvalidOperationException($"Cannot convert plain text response to {typeof(TResponse).Name}. Provide a custom response mapper or use a different method.");
            }
        }
    }
}