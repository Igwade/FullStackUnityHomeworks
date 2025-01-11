using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetState.Handlers
{
    public sealed class CompareTimestampsHandler : BaseHandler<GetStateContext>
    {
        private const string SaveTimeKey = "SaveTime";

        protected override UniTask Process(GetStateContext context, CancellationToken token)
        {
            if (context.Result.IsError) 
                return UniTask.CompletedTask;
            
            var localState = context.DeserializedLocalState;
            var remoteState = context.DeserializedRemoteState;
            
            if (localState != null)
            {
                context.LocalTimestamp = ExtractTimestamp(localState);
            }
            
            if (remoteState != null)
            {
                context.RemoteTimestamp = ExtractTimestamp(remoteState);
            }
            
            if (localState == null && remoteState == null)
            {
                Error("No valid state data found. Maybe need decrypt/deserialize", context);
                return UniTask.CompletedTask;
            }
            
            if (localState != null && remoteState != null)
            {
                context.Result = 
                    (context.LocalTimestamp >= context.RemoteTimestamp) 
                    ? localState 
                    : remoteState;
            }
            else
            {
                context.Result = localState ?? remoteState;
            }

            return UniTask.CompletedTask;
        }

        private long ExtractTimestamp(Dictionary<string, string> data)
        {
            try
            {
                if (data != null && data.TryGetValue(SaveTimeKey, out var time))
                {
                    if (long.TryParse(time, out var val))
                    {
                        return val;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return -1;
        }
    }
}
