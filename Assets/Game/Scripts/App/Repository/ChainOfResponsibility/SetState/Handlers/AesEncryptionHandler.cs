using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Ecryption;

namespace App.Repository.ChainOfResponsibility.SetState.Handlers
{
    public sealed class AesEncryptionHandler : BaseHandler<SetStateContext>
    {
        private readonly string _password;
        private readonly byte[] _salt;

        public AesEncryptionHandler(string password, byte[] salt)
        {
            _password = password;
            _salt = salt;
        }

        protected override UniTask Process(SetStateContext context, CancellationToken token)
        {
            if (string.IsNullOrEmpty(context.JsonPayload))
                return UniTask.CompletedTask;

            var cipher = AesEncryptor.Encrypt(context.JsonPayload, _password, _salt);
            context.JsonPayload = cipher;
            return UniTask.CompletedTask;
        }
    }
}