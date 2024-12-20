using Cysharp.Threading.Tasks;
using EitherMonad;
using Game.Scripts.SaveLoad.HttpClient;
using JetBrains.Annotations;
using SaveLoad;

namespace Game.Scripts.SaveLoad.Repositories
{
    [UsedImplicitly]
    public class WebRepository : IRepository
    {
        private readonly SaveLoadHttpClient _client;
        
        public WebRepository(SaveLoadHttpClient client)
        {
            _client = client;
        }

        public async UniTask<Result<int, string>> Save(int version, string content)
        {
            var result = await _client.Save(version, content);

            if (result.IsError)
            {
                return "Save failed: " + result.Error;
            }

            return version;
        }

        public async UniTask<Result<string, string>> Load(int version) 
            => await _client.Load(version);

        public async UniTask<Result<int, string>> GetLatestVersion() 
            => await _client.GetLatestVersion();
    }
}