using Cysharp.Threading.Tasks;
using EitherMonad;

namespace Modules.UnityHttpClient.Interfaces
{
    public interface IHttpClient
    {
        UniTask<Result<TResponse, string>> SendRequestAsync<TResponse>(HttpRequestConfiguration<TResponse> config);
    }
}