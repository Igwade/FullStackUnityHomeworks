using System;
using JetBrains.Annotations;
using Modules.Entities;
using SampleGame.Gameplay;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class TargetObjectData
    {
        public int entityId;
    }

    [UsedImplicitly]
    public class TargetObjectSerializer: ComponentSerializer<TargetObject, EntityWorld, TargetObjectData>
    {
        public override TargetObjectData Serialize(TargetObject component, EntityWorld world) =>
            new()
            {
                entityId = component.Value?.Id ?? -1
            };

        public override void Deserialize(TargetObject component, EntityWorld world, TargetObjectData data)
        {
            world.TryGet(data.entityId, out var entity);
            component.Value = entity;
        }
    }
}