using SaveLoadEntitiesExtension;
using UnityEngine;
using System.Collections.Generic;
using SaveLoad;

namespace GeneratedSaveLoad
{
    public static class Countdown_Serialization
    {
        private class CountdownData
        {
            public string Current;
        }

        public static string Save_Countdown(IComponent c, ISerializer serializer, ISaveLoadContext context)
        {
            var comp = c.GetRawComponent() as SampleGame.Gameplay.Countdown;
            var data = new CountdownData();

            data.Current = serializer.Serialize(comp.Current);

            return serializer.Serialize(data);
        }

        public static void Load_Countdown(IComponent c, string json, ISerializer serializer, ISaveLoadContext context)
        {
            var comp = c.GetRawComponent() as SampleGame.Gameplay.Countdown;
            var data = serializer.Deserialize<CountdownData>(json);

            comp.Current = serializer.Deserialize<System.Single>(data.Current);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Register()
        {
            SaveableComponentRegistry.Register("Countdown", Save_Countdown, Load_Countdown);
        }
    }
}
