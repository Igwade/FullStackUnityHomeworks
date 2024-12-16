using Game.Scripts.SaveLoad;
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
            this.Container.Bind<GameSaveLoadService>().AsSingle();
        }
    }
}