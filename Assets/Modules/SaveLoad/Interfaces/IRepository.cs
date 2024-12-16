using Cysharp.Threading.Tasks;
using EitherMonad;

namespace SaveLoad
{
    public interface IRepository
    {
        UniTask<Result<int, string>> Save(int version, string data);
        UniTask<Result<string, string>> Load(int version);
        UniTask<Result<int, string>> GetLatestVersion();
    }
}