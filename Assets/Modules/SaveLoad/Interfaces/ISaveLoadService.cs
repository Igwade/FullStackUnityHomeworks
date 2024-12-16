using Cysharp.Threading.Tasks;
using EitherMonad;

namespace SaveLoad
{
    public interface ISaveLoadService
    {
        void AddProvider(IDataProvider provider);
        UniTask<Result<int, string>> Save(int? version, ISaveLoadContext context);
        UniTask<Result<int, string>> Load(int? version, ISaveLoadContext context);
    }
}