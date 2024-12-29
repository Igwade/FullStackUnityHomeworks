using System;
using System.Collections.Generic;
using System.Linq;
using App.SaveLoad.Entities.ComponentSerializers;
using JetBrains.Annotations;
using Modules.Entities;
using SampleGame.Common;

namespace App.SaveLoad.Entities
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

    [UsedImplicitly]
    public sealed class EntitySerializationHelper
    {
        private readonly EntityWorld world;
        private readonly EntityCatalog catalog;

        private readonly Dictionary<Type, IComponentSerializer> _serializers;

        public EntitySerializationHelper(EntityWorld world, EntityCatalog catalog, IEnumerable<IComponentSerializer> serializers)
        {
            this.world = world;
            this.catalog = catalog;

            _serializers = serializers.ToDictionary(
                serializer => serializer.ComponentType,
                serializer => serializer
            );
        }

        public EntityData SerializeEntity(Entity entity)
        {
            var components = entity.GetComponents<ISerializableComponent>();
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
        
        public bool TrySpawnEntity(EntityData entityData, out Entity entity)
        {
            entity = null;
            
            if (!catalog.FindConfig(entityData.entityName, out var config))
                return false;

            entity = world.Spawn(config, entityData.position, entityData.rotation, entityData.id);
            return true;
        }
        
        public void DeserializeComponents(Entity entity, Dictionary<string, string> componentsData)
        {
            foreach (var component in entity.GetComponents<ISerializableComponent>())
            {
                DeserializeComponent(component, componentsData);
            }
        }
        
        private IComponentSerializer GetSerializer(ISerializableComponent component)
        {
            if (_serializers.TryGetValue(component.GetType(), out var serializer))
                return serializer;

            throw new InvalidOperationException($"Serializer for component type {component.GetType()} not found.");
        }

        private void SerializeComponent(ISerializableComponent component, Dictionary<string, string> saveState) 
            => GetSerializer(component).Serialize(component, saveState);

        private void DeserializeComponent(ISerializableComponent component, Dictionary<string, string> saveState) 
            => GetSerializer(component).Deserialize(component, saveState);
    }
}