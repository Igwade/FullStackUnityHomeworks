using SaveLoadEntitiesExtension;
using UnityEngine;
using System.Collections.Generic;
using SaveLoad;

namespace GeneratedSaveLoad
{
    public static class ProductionOrder_Serialization
    {
        private class ProductionOrderData
        {
            public string Queue;
        }

        public static string Save_ProductionOrder(IComponent c, ISerializer serializer, ISaveLoadContext context)
        {
            var comp = c.GetRawComponent() as SampleGame.Gameplay.ProductionOrder;
            var data = new ProductionOrderData();

            data.Queue = serializer.Serialize(comp.Queue);

            return serializer.Serialize(data);
        }

        public static void Load_ProductionOrder(IComponent c, string json, ISerializer serializer, ISaveLoadContext context)
        {
            var comp = c.GetRawComponent() as SampleGame.Gameplay.ProductionOrder;
            var data = serializer.Deserialize<ProductionOrderData>(json);

            comp.Queue = serializer.Deserialize<System.Collections.Generic.IReadOnlyList<Modules.Entities.EntityConfig>>(data.Queue);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Register()
        {
            SaveableComponentRegistry.Register("ProductionOrder", Save_ProductionOrder, Load_ProductionOrder);
        }
    }
}
