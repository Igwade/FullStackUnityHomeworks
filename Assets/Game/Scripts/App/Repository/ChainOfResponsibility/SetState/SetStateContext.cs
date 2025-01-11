using System.Collections.Generic;
using EitherMonad;
using Unit = EitherMonad.Unit;

namespace App.Repository.ChainOfResponsibility.SetState
{
    public sealed class SetStateContext: IContext
    {
        public int Version { get; }
        public Dictionary<string, string> GameState { get; }
        public string JsonPayload { get; set; }
        public Result<Unit, string> LocalSaveResult { get; set; }
        public Result<Unit, string> RemoteSaveResult { get; set; }
        public Result<Unit, string> Result { get; set; }
        public void SetError(string error) => Result = error;

        IResult IContext.Result => Result;

        public SetStateContext(int version, Dictionary<string, string> gameState)
        {
            Version = version;
            GameState = gameState;
            Result = Unit.Default;
        }
    }
}