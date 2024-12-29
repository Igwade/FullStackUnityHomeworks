using Cysharp.Threading.Tasks;
using Modules.Ecryption;

namespace App.Repository.Middleware.Middlewares
{
    public sealed class AesCipherMiddleware : IRepositoryMiddleware
    {
        private readonly string password;
        private readonly byte[] salt;
        
        public AesCipherMiddleware(string password, byte[] salt)
        {
            this.password = password;
            this.salt = salt;
        }
        
        public UniTask<string> HandleSaveString(string data)
        {
            var encrypted = AesEncryptor.Encrypt(data, password, salt);
            return UniTask.FromResult(encrypted);
        }

        public UniTask<string> HandleLoadString(string data)
        {
            var decrypted = AesEncryptor.Decrypt(data, password, salt);
            return UniTask.FromResult(decrypted);
        }
    }
}