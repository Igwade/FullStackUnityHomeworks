using App.Repository.Middleware.Middlewares;
using App.SaveLoad.Entities;
using App.SaveLoad.Entities.ComponentSerializers;
using App.SaveLoad.Serializers;
using UnityEngine;
using Zenject;

namespace App.SaveLoad
{
    [CreateAssetMenu(
        fileName = "SaveLoadInstaller",
        menuName = "Zenject/App/New SaveLoadInstaller"
    )]
    public sealed class SaveLoadInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<GameSaveLoader>().AsSingle();
            this.Container.BindInterfacesAndSelfTo<EntitySerializationHelper>().AsSingle();

            this.InstallComponentSerializers();
        }
        
        public void InstallComponentSerializers()
        {
            this.Container.BindInterfacesTo<WorldSerializer>().AsSingle();
            this.Container.BindInterfacesTo<HealthSerializer>().AsSingle();
            this.Container.BindInterfacesTo<DestinationPointSerializer>().AsSingle();
            this.Container.BindInterfacesTo<ProductionOrderSerializer>().AsSingle();
            this.Container.BindInterfacesTo<ResourceBagSerializer>().AsSingle();
            this.Container.BindInterfacesTo<TargetObjectSerializer>().AsSingle();
            this.Container.BindInterfacesTo<TeamSerializer>().AsSingle();
        }
    }
}