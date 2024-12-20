using Modules.UnityHttpClient.Interfaces;
using Newtonsoft.Json;

namespace Modules.UnityHttpClient.CommonSerializers.Json
{
    public class JsonResponseMapper<TResponse> : IHttpResponseMapper<TResponse>
    {
        public TResponse Map(string response)
        {
            return JsonConvert.DeserializeObject<TResponse>(response);
        }
    }

}