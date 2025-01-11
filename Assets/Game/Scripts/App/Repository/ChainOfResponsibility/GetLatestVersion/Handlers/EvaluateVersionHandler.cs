using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility.GetLatestVersion.Handlers
{
    public sealed class EvaluateVersionHandler : BaseHandler<GetLatestVersionContext>
    {
        protected override UniTask Process(GetLatestVersionContext context, CancellationToken token)
        {
            if (context.LocalVersion.IsError && context.RemoteVersion.IsError)
            {
                Error(
                    $"No valid save data found.\n" +
                    $"Local Error: {context.LocalVersion.Error}\n" +
                    $"Remote Error: {context.RemoteVersion.Error}", context);
                
                return UniTask.CompletedTask;
            }

            if (context.RemoteVersion.IsSuccess && context.LocalVersion.IsError)
            {
                context.Result = context.RemoteVersion;
                return UniTask.CompletedTask;
            }

            if (context.LocalVersion.IsSuccess && context.RemoteVersion.IsError)
            {
                context.Result = context.LocalVersion;
                return UniTask.CompletedTask;
            }

            var maxVer = Math.Max(context.LocalVersion.Success, context.RemoteVersion.Success);
            context.Result = maxVer;

            return UniTask.CompletedTask;
        }
    }
}