using SaveLoadEntitiesExtension;
using UnityEngine;
using System.Collections.Generic;
using SaveLoad;
namespace GeneratedSaveLoad {
public static class TargetObjectSerialization {
    private class TargetObjectData {
        public string Value;
    }
    public static string Save_TargetObject(IComponentAdapter c, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.TargetObject;
        var data = new TargetObjectData();
        data.Value = serializer.Serialize(comp.Value);
        return serializer.Serialize(data);
    }
    public static void Load_TargetObject(IComponentAdapter c, string json, ISerializer serializer, ISaveLoadContext context) {
        var comp = c.GetRawComponent() as SampleGame.Gameplay.TargetObject;
        var data = serializer.Deserialize<TargetObjectData>(json);
        comp.Value = serializer.Deserialize<Modules.Entities.Entity>(data.Value);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Register() {
        SaveableComponentRegistry.Register("TargetObject", Save_TargetObject, Load_TargetObject);
    }
}
}
