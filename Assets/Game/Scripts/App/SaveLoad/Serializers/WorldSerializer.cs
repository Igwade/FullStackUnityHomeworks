using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Modules.ComponentSerialization;
using Modules.Entities;
using SampleGame.Common;
using UnityEngine;

namespace App.SaveLoad.Serializers
{
    [Serializable]
    public class EntityData
    {
        public int id;
        public string entityName;
        public string entityType;
        public SerializedVector3 position;
        public SerializedVector3 rotation;
        public Dictionary<string, string> components;
    }

    [Serializable]
    public class EntityWorldData
    {
        public List<EntityData> entities;
    }

    [UsedImplicitly]
    public class WorldSerializer : GameSerializer<EntityWorld, EntityWorldData>
    {
        private readonly EntityCatalog catalog;

        public WorldSerializer(EntityCatalog catalog)
        {
            this.catalog = catalog;
        }

        protected override EntityWorldData Serialize(EntityWorld world)
        {
            var entities = world.GetAll();
            var entitiesData = entities.Select(SerializeEntity).ToList();

            return new EntityWorldData
            {
                entities = entitiesData
            };
        }

        protected override void Deserialize(EntityWorld world, EntityWorldData data)
        {
            world.DestroyAll();
            var entities = SpawnEntities(world, data);

            foreach (var (entity, entityData) in entities)
            {
                DeserializeComponents(entity, entityData.components);
            }
        }

        private EntityData SerializeEntity(Entity entity)
        {
            var components = entity.GetComponents<MonoBehaviour>();
            var componentsData = new Dictionary<string, string>();

            foreach (var component in components)
            {
                SerializeComponent(component, componentsData);
            }

            return new EntityData
            {
                id = entity.Id,
                entityName = entity.Name,
                entityType = entity.Type.ToString(),
                position = entity.transform.position,
                rotation = entity.transform.rotation,
                components = componentsData.Count > 0 ? componentsData : null
            };
        }

        private bool TrySpawnEntity(EntityWorld world, EntityData entityData, out Entity entity)
        {
            entity = null;

            if (!catalog.FindConfig(entityData.entityName, out var config))
                return false;

            entity = world.Spawn(config, entityData.position, entityData.rotation, entityData.id);
            return true;
        }

        private IEnumerable<(Entity entity, EntityData data)> SpawnEntities(EntityWorld world, EntityWorldData data)
        {
            var entities = new List<(Entity entity, EntityData data)>();

            foreach (var entityData in data.entities)
            {
                if (TrySpawnEntity(world, entityData, out var entity))
                {
                    entities.Add((entity, entityData));
                }
            }

            return entities;
        }

        private void DeserializeComponents(Entity entity, Dictionary<string, string> componentsData)
        {
            if (componentsData == null)
                return;

            foreach (var component in entity.GetComponents<MonoBehaviour>())
            {
                DeserializeComponent(component, componentsData);
            }
        }

        private void SerializeComponent(MonoBehaviour component, Dictionary<string, string> saveState)
        {
            var type = component.GetType().Name;
            var serializer = ComponentSerializersRegistry.GetFor(component);
            
            if (serializer != null)
            {
                var dto = serializer.Serialize(component);
                var json = JsonUtility.ToJson(dto);
                saveState[type] = json;
            }
        }

        private void DeserializeComponent(MonoBehaviour component, Dictionary<string, string> saveState)
        {
            var type = component.GetType().Name;
            var serializer = ComponentSerializersRegistry.GetFor(component);
            
            if (serializer != null && saveState.TryGetValue(type, out var json))
            {
                var dto = JsonUtility.FromJson(json, serializer.DtoType);
                serializer.Deserialize(component, dto);
            }
        }
    }
}