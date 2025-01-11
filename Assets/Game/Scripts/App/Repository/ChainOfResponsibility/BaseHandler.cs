using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility
{
    public abstract class BaseHandler<TContext> : IHandler<TContext> where TContext : IContext
    {
        private IHandler<TContext> _nextHandler;

        public IHandler<TContext> SetNext(IHandler<TContext> next)
        {
            _nextHandler = next;
            return next;
        }

        public async UniTask Handle(TContext context, CancellationToken token = default)
        {
            await Process(context, token);

            if (_nextHandler != null && !context.Result.IsError)
            {
                await _nextHandler.Handle(context, token);
            }
        }

        protected void Error(string error, TContext context)
        {
            context.SetError($"[{GetType().Name}] " + error);
        }
        
        protected abstract UniTask Process(TContext context, CancellationToken token);
    }
}