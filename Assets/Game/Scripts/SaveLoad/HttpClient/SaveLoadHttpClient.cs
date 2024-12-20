using Cysharp.Threading.Tasks;
using EitherMonad;
using JetBrains.Annotations;
using Modules.UnityHttpClient;

namespace Game.Scripts.SaveLoad.HttpClient
{
    [UsedImplicitly]
    public class SaveLoadHttpClient
    {
        private const string BaseURL = "http://localhost:5041";

        private string SaveURL(int version) => $"{BaseURL}/save?version={version}";
        private string LoadURL(int version) => $"{BaseURL}/load?version={version}";
        private string LatestVersionURL => $"{BaseURL}/latest-version";

        private readonly UnityHttpClient _httpClient;

        public SaveLoadHttpClient(UnityHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public UniTask<Result<string, string>> Save(int version, string content) =>
            _httpClient.SendRequestAsync(new HttpRequestBuilder<string, string>()
                .SetMethod(HttpMethod.Put)
                .SetUrl(SaveURL(version))
                .SetRequestData(content)
                .UsePlainText()
                .SetRawPayload(content, Content.Json)
                .Build());

        public UniTask<Result<string, string>> Load(int version) =>
            _httpClient.SendRequestAsync(new HttpRequestBuilder<Unit, string>()
                .SetMethod(HttpMethod.Get)
                .SetUrl(LoadURL(version))
                .UsePlainText()
                .Build());

        public UniTask<Result<int, string>> GetLatestVersion() =>
            _httpClient.SendRequestAsync(new HttpRequestBuilder<Unit, int>()
                .SetMethod(HttpMethod.Get)
                .SetUrl(LatestVersionURL)
                .UsePlainText()
                .Build());
    }
}