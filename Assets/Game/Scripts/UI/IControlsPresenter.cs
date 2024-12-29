using System;
using EitherMonad;

namespace Game.Gameplay
{
    //Don't modify
    public interface IControlsPresenter
    {
        void Save(Action<Result<int, string>> callback);
        void Load(string version, Action<Result<int, string>> callback);
    }
}