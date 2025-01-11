using System.Threading;
using App.Repository.Storage;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetState.Handlers
{
    public sealed class LocalLoadHandler : BaseHandler<GetStateContext>
    {
        private readonly LocalSaveStorage _storage;

        public LocalLoadHandler(LocalSaveStorage storage)
        {
            _storage = storage;
        }

        protected override async UniTask Process(GetStateContext context, CancellationToken token)
        {
            context.LocalResult = await _storage.LoadStateAsync(context.Version, token);
        }
    }
}