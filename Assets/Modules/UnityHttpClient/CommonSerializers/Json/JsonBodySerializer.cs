using System.Text;
using Modules.UnityHttpClient.Interfaces;
using Newtonsoft.Json;

namespace Modules.UnityHttpClient.CommonSerializers.Json
{
    public class JsonBodySerializer : IBodySerializer
    {
        public string ContentType => Content.Json;

        public byte[] Serialize<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}