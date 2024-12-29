using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EitherMonad;

namespace App.Repository.Storage
{
    public interface ISaveStorage
    {
        UniTask<Result<Unit, string>> SaveStateAsync(int version, string state, CancellationToken token = default);
        UniTask<Result<string, string>> LoadStateAsync(int version, CancellationToken token = default);
        UniTask<Result<int,string>> GetLatestVersionAsync(CancellationToken token);
    }
}