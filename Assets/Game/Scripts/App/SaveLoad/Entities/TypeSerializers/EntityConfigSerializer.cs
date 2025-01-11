using System;
using Modules.ComponentSerialization.Runtime;
using Modules.Entities;

namespace App.SaveLoad.Entities.TypeSerializers
{
    [Serializable]
    public class EntityConfigDto
    {
        public string name;
    }
    
    public class EntityConfigSerializer: ITypeSerializer<EntityConfig, EntityConfigDto>
    {
        private readonly EntityCatalog _catalog;
        
        public EntityConfigSerializer(EntityCatalog catalog)
        {
            _catalog = catalog;
        }
        
        public EntityConfigDto Serialize(EntityConfig config) =>
            new()
            {
                name = config.Name
            };

        public EntityConfig Deserialize(EntityConfigDto dto)
        {
            if (_catalog.FindConfig(dto.name, out var config))
            {
                return config;
            }
            
            throw new Exception($"Entity config with name {dto.name} not found");
        }
    }
}