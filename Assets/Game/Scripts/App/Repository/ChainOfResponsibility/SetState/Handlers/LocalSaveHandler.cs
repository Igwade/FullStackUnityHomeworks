using System.Threading;
using App.Repository.Storage;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.SetState.Handlers
{
    public sealed class LocalSaveHandler : BaseHandler<SetStateContext>
    {
        private readonly LocalSaveStorage _storage;

        public LocalSaveHandler(LocalSaveStorage storage)
        {
            _storage = storage;
        }

        protected override async UniTask Process(SetStateContext context, CancellationToken token)
        {
            var result = await _storage.SaveStateAsync(context.Version, context.JsonPayload, token);
            context.LocalSaveResult = result;
        }
    }
}