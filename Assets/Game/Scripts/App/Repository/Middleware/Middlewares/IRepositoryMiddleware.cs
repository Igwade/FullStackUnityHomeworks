using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace App.Repository.Middleware
{
    public interface IRepositoryMiddleware
    {
        UniTask<Dictionary<string, string>> HandleBeforeSerialize(Dictionary<string, string> gameState) => UniTask.FromResult(gameState);
        UniTask<string> HandleSaveString(string data) => UniTask.FromResult(data);
        UniTask<string> HandleLoadString(string data) => UniTask.FromResult(data);
        UniTask<Dictionary<string, string>> HandleAfterDeserialize(Dictionary<string, string> gameState) => UniTask.FromResult(gameState);
    }
}