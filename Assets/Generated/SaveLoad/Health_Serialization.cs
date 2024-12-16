using SaveLoadEntitiesExtension;
using UnityEngine;
using System.Collections.Generic;
using SaveLoad;
namespace GeneratedSaveLoad {
public static class HealthSerialization {
    private class HealthData {
        public string Current;
    }
    public static string Save_Health(IComponentAdapter c, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.Health;
        var data = new HealthData();
        data.Current = serializer.Serialize(comp.Current);
        return serializer.Serialize(data);
    }
    public static void Load_Health(IComponentAdapter c, string json, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.Health;
        var data = serializer.Deserialize<HealthData>(json);
        comp.Current = serializer.Deserialize<System.Int32>(data.Current);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Register() {
        SaveableComponentRegistry.Register("Health", Save_Health, Load_Health);
    }
}
}
