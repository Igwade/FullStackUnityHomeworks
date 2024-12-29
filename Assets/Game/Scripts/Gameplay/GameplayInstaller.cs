using Modules.Entities;
using SampleGame.App;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    //Don't modify
    [CreateAssetMenu(
        fileName = "GameplayInstaller",
        menuName = "Zenject/New Gameplay Installer"
    )]
    public sealed class GameplayInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private EntityCatalog _catalog;
        
        public override void InstallBindings()
        {
            this.Container.Bind<EntityWorld>().FromComponentInHierarchy().AsSingle();
            this.Container.Bind<EntityCatalog>().FromInstance(_catalog).AsSingle();
        }
    }
}