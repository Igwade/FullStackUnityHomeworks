using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.SaveLoad;

namespace Game.Gameplay
{
    public sealed class ControlsPresenter : IControlsPresenter
    {
        private readonly GameSaveLoadService _gameSaveLoadService;

        public ControlsPresenter(GameSaveLoadService gameSaveLoadService)
        {
            this._gameSaveLoadService = gameSaveLoadService;
        }
        
        public void Save(Action<bool, int> callback)
        {
            _gameSaveLoadService.SaveGame(callback: callback).Forget();
        }

        public void Load(string versionText, Action<bool, int> callback)
        {
            int? version = null;

            if (int.TryParse(versionText, out var result))
            {
                version = result;
            }
            
            _gameSaveLoadService.LoadGame(version, callback).Forget();
        }
    }
}