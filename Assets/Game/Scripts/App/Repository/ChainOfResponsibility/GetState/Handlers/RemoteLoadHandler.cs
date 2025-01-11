using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetState.Handlers
{
    public sealed class RemoteLoadHandler : BaseHandler<GetStateContext>
    {
        private readonly GameClient _gameClient;

        public RemoteLoadHandler(GameClient gameClient)
        {
            _gameClient = gameClient;
        }

        protected override async UniTask Process(GetStateContext context, CancellationToken token)
        {
            context.RemoteResult = await _gameClient.Load(context.Version);
        }
    }
}