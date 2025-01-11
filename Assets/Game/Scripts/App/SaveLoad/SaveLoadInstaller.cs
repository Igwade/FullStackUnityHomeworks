using App.SaveLoad.Entities;
using App.SaveLoad.Entities.TypeSerializers;
using App.SaveLoad.Serializers;
using Modules.ComponentSerialization.Runtime;
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
            this.InstallComponentSerializers();
        }
        
        public void InstallComponentSerializers()
        {
            this.Container.BindInterfacesTo<WorldSerializer>().AsSingle();
            
            this.Container
                .BindInterfacesTo<EntityConfigSerializer>().AsCached()
                .OnInstantiated<EntityConfigSerializer>(RegisterSerializer)
                .NonLazy();
            
            this.Container
                .BindInterfacesTo<Vector3Serializer>().AsCached()
                .OnInstantiated<Vector3Serializer>(RegisterSerializer)
                .NonLazy();
            
            this.Container
                .BindInterfacesTo<EntitySerializer>().AsCached()
                .OnInstantiated<EntitySerializer>(RegisterSerializer)
                .NonLazy();
        }

        private void RegisterSerializer<TSource, TDto>(InjectContext _, ITypeSerializer<TSource, TDto> serializer)
        {
            TypeSerializers.Register(serializer);
        }
    }
}