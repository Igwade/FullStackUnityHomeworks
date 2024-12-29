using System;
using JetBrains.Annotations;
using SampleGame.Common;
using SampleGame.Gameplay;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class ResourceBagData
    {
        public string type;
        public int current;
    }

    [UsedImplicitly]
    public class ResourceBagSerializer : ComponentSerializer<ResourceBag, ResourceBagData>
    {
        public override ResourceBagData Serialize(ResourceBag component) =>
            new()
            {
                type = component.Type.ToString(),
                current = component.Current
            };

        public override void Deserialize(ResourceBag component, ResourceBagData data)
        {
            if (Enum.TryParse<ResourceType>(data.type, out var resourceType))
            {
                component.Type = resourceType;
                component.Current = data.current;
            }
        }
    }
}