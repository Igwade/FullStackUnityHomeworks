using System.Threading;
using Cysharp.Threading.Tasks;
using Unit = EitherMonad.Unit;

namespace App.Repository.ChainOfResponsibility.SetState.Handlers
{
    public sealed class EvaluateSaveHandler : BaseHandler<SetStateContext>
    {
        protected override UniTask Process(SetStateContext context, CancellationToken token)
        {
            var localError = context.LocalSaveResult.IsError ? context.LocalSaveResult.Error : null;
            var remoteError = context.RemoteSaveResult.IsError ? context.RemoteSaveResult.Error : null;

            if (context.LocalSaveResult.IsError && context.RemoteSaveResult.IsError)
            {
                Error($"Failed to save:\nLocal Error: {localError}\nRemote Error: {remoteError}", context);
            }
            else
            {
                context.Result = Unit.Default;
            }

            return UniTask.CompletedTask;
        }
    }
}