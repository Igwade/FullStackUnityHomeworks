using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetState.Handlers
{
    public sealed class EvaluateLoadHandler : BaseHandler<GetStateContext>
    {
        protected override UniTask Process(GetStateContext context, CancellationToken token)
        {
            if (context.Result.IsError)
                return UniTask.CompletedTask;
            
            if (context.LocalResult.IsError && context.RemoteResult.IsError)
            {
                Error("No valid save data found (both local & remote failed).", context);
                return UniTask.CompletedTask;
            }

            if (context.LocalResult.IsSuccess)
                context.LocalJson = context.LocalResult.Success;

            if (context.RemoteResult.IsSuccess)
                context.RemoteJson = context.RemoteResult.Success;
            
            return UniTask.CompletedTask;
        }
    }
}