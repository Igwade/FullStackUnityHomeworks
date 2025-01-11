using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Repository.ChainOfResponsibility.SetState.Handlers
{
    public sealed class SerializeHandler : BaseHandler<SetStateContext>
    {
        protected override UniTask Process(SetStateContext context, CancellationToken token)
        {
            context.JsonPayload = JsonConvert.SerializeObject(context.GameState);
            return UniTask.CompletedTask;
        }
    }
}