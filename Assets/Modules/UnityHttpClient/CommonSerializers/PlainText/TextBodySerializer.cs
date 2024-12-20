using System.Text;
using Modules.UnityHttpClient.Interfaces;

namespace Modules.UnityHttpClient.CommonSerializers.PlainText
{
    public class TextBodySerializer : IBodySerializer
    {
        public string ContentType => Content.PlainText;

        public byte[] Serialize<T>(T data)
        {
            var text = data?.ToString() ?? string.Empty;
            return Encoding.UTF8.GetBytes(text);
        }
    }
}