using Game.Scripts.SaveLoad;
using Game.Scripts.SaveLoad.HttpClient;
using Modules.UnityHttpClient;
using UnityEngine;
using Zenject;

namespace SampleGame.App
{
    [CreateAssetMenu(
        fileName = "AppInstaller",
        menuName = "Zenject/New App Installer"
    )]
    public sealed class AppInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<GameFacade>().AsSingle();
            this.Container.Bind<UnityHttpClient>().AsSingle();
            this.Container.Bind<SaveLoadHttpClient>().AsSingle();
            this.Container.Bind<GameSaveLoadService>().AsSingle();
        }
    }
}