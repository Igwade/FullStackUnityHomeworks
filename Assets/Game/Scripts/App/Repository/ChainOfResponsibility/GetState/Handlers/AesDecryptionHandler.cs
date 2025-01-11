using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Ecryption;

namespace App.Repository.ChainOfResponsibility.GetState.Handlers
{
    public sealed class AesDecryptionHandler : BaseHandler<GetStateContext>
    {
        private readonly string _password;
        private readonly byte[] _salt;

        public AesDecryptionHandler(string password, byte[] salt)
        {
            _password = password;
            _salt = salt;
        }

        protected override UniTask Process(GetStateContext context, CancellationToken token)
        {
            var localJson = context.LocalJson;
            var remoteJson = context.RemoteJson;

            if (context.Result.IsError)
                return UniTask.CompletedTask;

            if (string.IsNullOrEmpty(localJson) && string.IsNullOrEmpty(remoteJson))
                return UniTask.CompletedTask;

            try
            {
                if (!string.IsNullOrEmpty(localJson))
                    context.LocalJson = AesEncryptor.Decrypt(localJson, _password, _salt);

                if (!string.IsNullOrEmpty(remoteJson))
                    context.RemoteJson = AesEncryptor.Decrypt(remoteJson, _password, _salt);
            }
            catch (Exception e)
            {
                context.Result = e.Message;
            }

            return UniTask.CompletedTask;
        }
    }
}