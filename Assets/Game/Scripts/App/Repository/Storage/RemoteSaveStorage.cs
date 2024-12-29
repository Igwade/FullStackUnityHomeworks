using System.Threading;
using Cysharp.Threading.Tasks;
using EitherMonad;
using JetBrains.Annotations;

namespace App.Repository.Storage
{
    [UsedImplicitly]
    public sealed class RemoteSaveStorage : ISaveStorage
    {
        private readonly GameClient client;

        public RemoteSaveStorage(GameClient client)
        {
            this.client = client;
        }

        public async UniTask<Result<Unit, string>> SaveStateAsync(int version, string state, CancellationToken token = default) 
            => await client.Save(version, state);

        public async UniTask<Result<string, string>> LoadStateAsync(int version, CancellationToken token = default) 
            => await client.Load(version);

        public async UniTask<Result<int, string>> GetLatestVersionAsync(CancellationToken token = default) 
            => await client.GetLatestVersion();
    }
}