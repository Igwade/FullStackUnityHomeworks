using System;
using System.Collections.Generic;
using SaveLoad;
using SaveLoadEntitiesExtension.Dtos;

namespace SaveLoadEntitiesExtension
{
    public sealed class EntitiesDataProvider : IDataProvider
    {
        public string Key => saveKey;

        private readonly string saveKey;
        private readonly IWorld _iWorld;

        public EntitiesDataProvider(string saveKey, IWorld iWorld)
        {
            this.saveKey = saveKey;
            this._iWorld = iWorld ?? throw new ArgumentNullException(nameof(iWorld));
        }

        public string GetData(ISaveLoadContext context, ISerializer serializer)
        {
            var worldData = BuildWorldData(context, serializer);
            return serializer.Serialize(worldData);
        }

        public void SetData(string data, ISaveLoadContext context, ISerializer serializer)
        {
            var worldData = serializer.Deserialize<WorldData>(data);
            ApplyWorldData(context, worldData, serializer);
        }

        private WorldData BuildWorldData(ISaveLoadContext context, ISerializer serializer)
        {
            var entities = _iWorld.GetAllEntities();
            var entitiesData = new List<EntityData>();

            foreach (var e in entities)
            {
                var comps = e.GetComponents();
                var componentData = new Dictionary<string, string>();
                foreach (var c in comps)
                {
                    if (SaveableComponentRegistry.TryGet(c.GetComponentIdentifier(), out var entry))
                    {
                        var json = entry.save(c, serializer, context);
                        componentData[c.GetComponentIdentifier()] = json;
                    }
                }

                var (px, py, pz) = e.GetPosition();
                var (rx, ry, rz) = e.GetRotation();

                entitiesData.Add(new EntityData
                {
                    id = e.GetId(),
                    entityName = e.GetName(),
                    entityType = e.GetEntityType(),
                    px = px, py = py, pz = pz,
                    rx = rx, ry = ry, rz = rz,
                    Components = componentData
                });
            }

            var worldData = new WorldData { entities = entitiesData.ToArray() };
            return worldData;
        }

        private void ApplyWorldData(ISaveLoadContext context, WorldData worldData, ISerializer serializer)
        {
            _iWorld.DestroyAllEntities();

            var idToEntity = new Dictionary<int, IEntity>();
            foreach (var eData in worldData.entities)
            {
                var entity = _iWorld.SpawnEntity(
                    eData.entityName,
                    eData.px, eData.py, eData.pz,
                    eData.rx, eData.ry, eData.rz,
                    eData.id);
                idToEntity[eData.id] = entity;
            }

            foreach (var eData in worldData.entities)
            {
                var entity = idToEntity[eData.id];
                var comps = entity.GetComponents();
                foreach (var comp in comps)
                {
                    if (eData.Components.TryGetValue(comp.GetComponentIdentifier(), out var compJson))
                    {
                        if (SaveableComponentRegistry.TryGet(comp.GetComponentIdentifier(), out var entry))
                        {
                            entry.load(comp, compJson, serializer, context);
                        }
                    }
                }
            }
        }
    }
}