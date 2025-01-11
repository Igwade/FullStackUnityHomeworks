using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.SetState.Handlers
{
    public sealed class AddTimestampHandler : BaseHandler<SetStateContext>
    {
        private const string SaveTimeKey = "SaveTime";
        private static readonly DateTime OriginTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected override UniTask Process(SetStateContext context, CancellationToken token)
        {
            var timeSeconds = (DateTime.UtcNow - OriginTime).TotalSeconds.ToString("F0");
            context.GameState[SaveTimeKey] = timeSeconds;
            return UniTask.CompletedTask;
        }
    }
}