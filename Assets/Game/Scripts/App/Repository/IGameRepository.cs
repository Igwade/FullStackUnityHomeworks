using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EitherMonad;

namespace SampleGame.App
{
    public interface IGameRepository
    {
        UniTask<Result<Unit, string>> SetState(int version, Dictionary<string, string> gameState, CancellationToken token = default);
        UniTask<Result<Dictionary<string, string>, string>> GetState(int version, CancellationToken token = default);
        UniTask<Result<int, string>> GetLatestVersion(CancellationToken token = default);
    }
}