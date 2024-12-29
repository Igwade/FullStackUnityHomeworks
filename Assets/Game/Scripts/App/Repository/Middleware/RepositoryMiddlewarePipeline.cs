using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace App.Repository.Middleware
{
    [UsedImplicitly]
    public sealed class RepositoryMiddlewarePipeline
    {
        private readonly IReadOnlyList<IRepositoryMiddleware> middlewares;

        public RepositoryMiddlewarePipeline(IEnumerable<IRepositoryMiddleware> middlewares)
        {
            this.middlewares = middlewares.ToList();
        }
        
        public async UniTask<Dictionary<string, string>> OnBeforeSerialize(Dictionary<string, string> state)
        {
            var current = state;
            foreach (var middleware in middlewares)
            {
                current = await middleware.HandleBeforeSerialize(current);
            }
            return current;
        }
        
        public async UniTask<string> HandleSaveStringAsync(string data)
        {
            var current = data;
            foreach (var middleware in middlewares)
            {
                current = await middleware.HandleSaveString(current);
            }
            return current;
        }
        
        public async UniTask<string> HandleLoadStringAsync(string data)
        {
            var current = data;
            for (var i = middlewares.Count - 1; i >= 0; i--)
            {
                current = await middlewares[i].HandleLoadString(current);
            }
            return current;
        }
        
        public async UniTask<Dictionary<string, string>> OnAfterDeserialize(Dictionary<string, string> state)
        {
            var current = state;
            for (var i = middlewares.Count - 1; i >= 0; i--)
            {
                current = await middlewares[i].HandleAfterDeserialize(current);
            }
            return current;
        }
    }
}
