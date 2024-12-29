using System;
using System.Collections.Generic;
using System.Linq;
using App.SaveLoad.Entities;
using JetBrains.Annotations;
using Modules.Entities;

namespace App.SaveLoad.Serializers
{
    [Serializable]
    public class EntityWorldData
    {
        public List<EntityData> entities;
    }
    
    [UsedImplicitly]
    public class WorldSerializer : GameSerializer<EntityWorld, EntitySerializationHelper, EntityWorldData>
    {
        protected override EntityWorldData Serialize(EntityWorld world, EntitySerializationHelper esh)
        {
            var entities = world.GetAll();
            var entitiesData = entities.Select(esh.SerializeEntity).ToList();

            return new EntityWorldData
            {
                entities = entitiesData
            };
        }
        
        protected override void Deserialize(EntityWorld world, EntitySerializationHelper esh, EntityWorldData data)
        {
            world.DestroyAll();
            var entities = SpawnEntities(esh, data);
            foreach (var e in entities)
            {
                esh.DeserializeComponents(e.entity, e.data.components);
            }
        }

        private IEnumerable<(Entity entity, EntityData data)> SpawnEntities(EntitySerializationHelper esh, EntityWorldData data)
        {
            var entities = new List<(Entity, EntityData)>();
            foreach (var entityData in data.entities)
            {
                if (esh.TrySpawnEntity(entityData, out var entity))
                {
                    entities.Add((entity, entityData));
                }
            }

            return entities;
        }
    }
}