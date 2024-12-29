using Cysharp.Threading.Tasks;
using EitherMonad;
using JetBrains.Annotations;
using Modules.UnityHttpClient;

namespace App.Repository
{
    [UsedImplicitly]
    public class GameClient
    {
        private string SaveURL(int version) => $"{_baseUrl}/save?version={version}";
        private string LoadURL(int version) => $"{_baseUrl}/load?version={version}";
        private string LatestVersionURL => $"{_baseUrl}/latest-version";

        private readonly UnityHttpClient _httpClient;
        private readonly string _baseUrl;

        public GameClient(UnityHttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public UniTask<Result<Unit, string>> Save(int version, string content) =>
            _httpClient.SendRequestAsync(new HttpRequestBuilder<string, Unit>()
                .SetMethod(HttpMethod.Put)
                .SetUrl(SaveURL(version))
                .SetRequestData(content)
                .UsePlainText()
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