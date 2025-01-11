using EitherMonad;

namespace App.Repository.ChainOfResponsibility.GetLatestVersion
{
    public sealed class GetLatestVersionContext: IContext
    {
        public Result<int, string> LocalVersion { get; set; }
        public Result<int, string> RemoteVersion { get; set; }
        public Result<int, string> Result { get; set; } = -1;
        public void SetError(string error) => Result = error;

        IResult IContext.Result => Result;
    }
}