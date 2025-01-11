using System.Threading;
using App.Repository.Storage;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetLatestVersion.Handlers
{
    public sealed class LocalVersionHandler : BaseHandler<GetLatestVersionContext>
    {
        private readonly LocalSaveStorage _storage;

        public LocalVersionHandler(LocalSaveStorage storage)
        {
            _storage = storage;
        }

        protected override async UniTask Process(GetLatestVersionContext context, CancellationToken token)
        {
            context.LocalVersion = await _storage.GetLatestVersionAsync(token);
        }
    }
}