using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Repository.Middleware.Middlewares
{
    [UsedImplicitly]
    public sealed class LoggingMiddleware : IRepositoryMiddleware
    {
        public UniTask<Dictionary<string, string>> HandleBeforeSerialize(Dictionary<string, string> gameState)
        {
            Debug.Log($"[LoggingMiddleware] HandleBeforeSerialize -> State Count: {gameState?.Count}");
            return UniTask.FromResult(gameState);
        }

        public UniTask<string> HandleSaveString(string data)
        {
            Debug.Log($"[LoggingMiddleware] HandleSaveString -> Data Length: {data?.Length}");
            return UniTask.FromResult(data);
        }

        public UniTask<string> HandleLoadString(string data)
        {
            Debug.Log($"[LoggingMiddleware] HandleLoadString -> Data Length: {data?.Length}");
            return UniTask.FromResult(data);
        }

        public UniTask<Dictionary<string, string>> HandleAfterDeserialize(Dictionary<string, string> gameState)
        {
            Debug.Log($"[LoggingMiddleware] HandleAfterDeserialize -> State Count: {gameState?.Count}");
            return UniTask.FromResult(gameState);
        }
    }
}