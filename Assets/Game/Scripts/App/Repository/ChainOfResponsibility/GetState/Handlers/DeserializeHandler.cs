using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Repository.ChainOfResponsibility.GetState.Handlers
{
    public sealed class DeserializeHandler : BaseHandler<GetStateContext>
    {
        protected override UniTask Process(GetStateContext context, CancellationToken token)
        {
            if (string.IsNullOrEmpty(context.LocalJson) && string.IsNullOrEmpty(context.RemoteJson))
            {
                Error("No valid save data found.", context);
                return UniTask.CompletedTask;
            }

            try
            {
                if (!string.IsNullOrEmpty(context.RemoteJson))
                    context.DeserializedRemoteState = JsonConvert.DeserializeObject<Dictionary<string, string>>(context.RemoteJson);

                if (!string.IsNullOrEmpty(context.LocalJson))
                    context.DeserializedLocalState = JsonConvert.DeserializeObject<Dictionary<string, string>>(context.LocalJson);
            }
            catch (Exception e)
            {
                Error(e.Message, context);
            }

            return UniTask.CompletedTask;
        }
    }
}