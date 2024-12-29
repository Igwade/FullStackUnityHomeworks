using System;
using SampleGame.Gameplay;
using UnityEngine.Serialization;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class CountdownData
    {
        public float current;
    }
    
    public class CountdownSerializer: ComponentSerializer<Countdown, CountdownData>
    {
        public override CountdownData Serialize(Countdown component) =>
            new()
            {
                current = component.Current
            };

        public override void Deserialize(Countdown component, CountdownData data)
        {
            component.Current = data.current;
        }
    }
}