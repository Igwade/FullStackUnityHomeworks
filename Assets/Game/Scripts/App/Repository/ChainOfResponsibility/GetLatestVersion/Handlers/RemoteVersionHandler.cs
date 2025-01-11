using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetLatestVersion.Handlers
{
    public sealed class RemoteVersionHandler : BaseHandler<GetLatestVersionContext>
    {
        private readonly GameClient _gameClient;

        public RemoteVersionHandler(GameClient gameClient)
        {
            _gameClient = gameClient;
        }

        protected override async UniTask Process(GetLatestVersionContext context, CancellationToken token)
        {
            context.RemoteVersion = await _gameClient.GetLatestVersion();
        }
    }
}