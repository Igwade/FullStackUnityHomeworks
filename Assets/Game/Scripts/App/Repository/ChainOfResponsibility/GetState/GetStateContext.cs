using System.Collections.Generic;
using EitherMonad;

namespace App.Repository.ChainOfResponsibility.GetState
{
    public sealed class GetStateContext: IContext
    {
        public int Version { get; }

        public Result<string, string> LocalResult { get; set; }
        public Result<string, string> RemoteResult { get; set; }

        public string LocalJson { get; set; }
        public string RemoteJson { get; set; }

        public Dictionary<string, string> DeserializedLocalState { get; set; }
        public Dictionary<string, string> DeserializedRemoteState { get; set; }
        
        public long LocalTimestamp { get; set; } = -2;
        public long RemoteTimestamp { get; set; } = -2;

        public Result<Dictionary<string, string>, string> Result { get; set; }

        IResult IContext.Result => Result;

        public void SetError(string error) => Result = error;

        public GetStateContext(int version)
        {
            Version = version;
            Result = new Dictionary<string, string>();
        }
    }
}