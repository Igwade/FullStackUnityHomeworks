using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Modules.Entities;
using SampleGame.Gameplay;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class ProductionOrderData
    {
        public List<string> queue;
    }
    
    [UsedImplicitly]
    public class ProductionOrderSerializer: ComponentSerializer<ProductionOrder, EntityCatalog, ProductionOrderData>
    {
        public override ProductionOrderData Serialize(ProductionOrder component, EntityCatalog entityCatalog) =>
            new()
            {
                queue = component.Queue.Select(it => it.Name).ToList()
            };

        public override void Deserialize(ProductionOrder component, EntityCatalog entityCatalog, ProductionOrderData data)
        {
            var queue = new List<EntityConfig>();
            foreach (var entityName in data.queue)
            {
                if (entityCatalog.FindConfig(entityName, out var config))
                {
                    queue.Add(config);
                }
            }
            
            component.Queue = queue;
        }
    }
}