using System;
using JetBrains.Annotations;
using SampleGame.Common;
using SampleGame.Gameplay;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class DestinationPointData
    {
        public SerializedVector3 value;
    }

    [UsedImplicitly]
    public class DestinationPointSerializer : ComponentSerializer<DestinationPoint, DestinationPointData>
    {
        public override DestinationPointData Serialize(DestinationPoint component) =>
            new()
            {
                value = component.Value
            };

        public override void Deserialize(DestinationPoint component, DestinationPointData data)
        {
            component.Value = data.value;
        }
    }
}