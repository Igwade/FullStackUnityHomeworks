using SaveLoadEntitiesExtension;
using UnityEngine;
using System.Collections.Generic;
using SaveLoad;
namespace GeneratedSaveLoad {
public static class ResourceBagSerialization {
    private class ResourceBagData {
        public string Type;
        public string Current;
    }
    public static string Save_ResourceBag(IComponentAdapter c, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.ResourceBag;
        var data = new ResourceBagData();
        data.Type = serializer.Serialize(comp.Type);
        data.Current = serializer.Serialize(comp.Current);
        return serializer.Serialize(data);
    }
    public static void Load_ResourceBag(IComponentAdapter c, string json, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.ResourceBag;
        var data = serializer.Deserialize<ResourceBagData>(json);
        comp.Type = serializer.Deserialize<SampleGame.Common.ResourceType>(data.Type);
        comp.Current = serializer.Deserialize<System.Int32>(data.Current);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Register() {
        SaveableComponentRegistry.Register("ResourceBag", Save_ResourceBag, Load_ResourceBag);
    }
}
}
