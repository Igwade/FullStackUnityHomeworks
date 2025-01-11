using System.Collections.Generic;
using System.Threading;
using App.Repository.ChainOfResponsibility;
using App.Repository.ChainOfResponsibility.SetState;
using App.Repository.ChainOfResponsibility.GetState;
using App.Repository.ChainOfResponsibility.GetLatestVersion;
using Cysharp.Threading.Tasks;
using EitherMonad;
using JetBrains.Annotations;
using SampleGame.App;
using Unit = EitherMonad.Unit;

namespace App.Repository
{
    [UsedImplicitly]
    public sealed class GameRepository : IGameRepository
    {
        private readonly IHandler<SetStateContext> _setStateChain;
        private readonly IHandler<GetStateContext> _getStateChain;
        private readonly IHandler<GetLatestVersionContext> _getLatestVersionChain;

        public GameRepository(
            IHandler<SetStateContext> setStateChain,
            IHandler<GetStateContext> getStateChain,
            IHandler<GetLatestVersionContext> getLatestVersionChain)
        {
            _setStateChain = setStateChain;
            _getStateChain = getStateChain;
            _getLatestVersionChain = getLatestVersionChain;
        }

        public async UniTask<Result<Unit, string>> SetState(int version, Dictionary<string, string> gameState, CancellationToken token = default)
        {
            var context = new SetStateContext(version, gameState);
            await _setStateChain.Handle(context, token);
            return context.Result;
        }

        public async UniTask<Result<Dictionary<string, string>, string>> GetState(int version, CancellationToken token = default)
        {
            var context = new GetStateContext(version);
            await _getStateChain.Handle(context, token);
            return context.Result;
        }

        public async UniTask<Result<int, string>> GetLatestVersion(CancellationToken token = default)
        {
            var context = new GetLatestVersionContext();
            await _getLatestVersionChain.Handle(context, token);
            return context.Result;
        }
    }
}