using System;
using App.SaveLoad;
using EitherMonad;
using Sirenix.Utilities;

namespace Game.Gameplay
{
    public sealed class ControlsPresenter : IControlsPresenter
    {
        private readonly GameSaveLoader saveLoader;

        public ControlsPresenter(GameSaveLoader saveLoader)
        {
            this.saveLoader = saveLoader;
        }

        public void Save(Action<Result<int, string>> callback)
        {
            saveLoader.Save(callback: callback).Forget();
        }

        public void Load(string versionText, Action<Result<int, string>> callback)
        {
            if (versionText.IsNullOrWhitespace())
            {
                saveLoader.Load(callback: callback).Forget();
            }
            else if (int.TryParse(versionText, out var version))
            {
                saveLoader.Load(version, callback: callback).Forget();
            }
        }
    }
}