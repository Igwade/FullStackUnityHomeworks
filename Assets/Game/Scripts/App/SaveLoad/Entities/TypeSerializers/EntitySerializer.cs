using System;
using Modules.ComponentSerialization.Runtime;
using Modules.Entities;

namespace App.SaveLoad.Entities.TypeSerializers
{
    [Serializable]
    public class EntityDto
    {
        public int id;
    }

    public class EntitySerializer : ITypeSerializer<Entity, EntityDto>
    {
        private readonly EntityWorld _world;

        public EntitySerializer(EntityWorld world)
        {
            _world = world;
        }

        public EntityDto Serialize(Entity entity) =>
            new()
            {
                id = entity?.Id ?? -1
            };

        public Entity Deserialize(EntityDto dto)
        {
            if (_world.TryGet(dto.id, out var entity))
            {
                return entity;
            }

            return null;
        }
    }
}