using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.SetState.Handlers
{
    public sealed class RemoteSaveHandler : BaseHandler<SetStateContext>
    {
        private readonly GameClient _gameClient;

        public RemoteSaveHandler(GameClient gameClient)
        {
            _gameClient = gameClient;
        }

        protected override async UniTask Process(SetStateContext context, CancellationToken token)
        {
            var result = await _gameClient.Save(context.Version, context.JsonPayload);
            context.RemoteSaveResult = result;
        }
    }
}