using SaveLoadEntitiesExtension;
using UnityEngine;
using System.Collections.Generic;
using SaveLoad;
namespace GeneratedSaveLoad {
public static class DestinationPointSerialization {
    private class DestinationPointData {
        public string Value;
    }
    public static string Save_DestinationPoint(IComponentAdapter c, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.DestinationPoint;
        var data = new DestinationPointData();
        data.Value = serializer.Serialize(comp.Value);
        return serializer.Serialize(data);
    }
    public static void Load_DestinationPoint(IComponentAdapter c, string json, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.DestinationPoint;
        var data = serializer.Deserialize<DestinationPointData>(json);
        comp.Value = serializer.Deserialize<UnityEngine.Vector3>(data.Value);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Register() {
        SaveableComponentRegistry.Register("DestinationPoint", Save_DestinationPoint, Load_DestinationPoint);
    }
}
}
