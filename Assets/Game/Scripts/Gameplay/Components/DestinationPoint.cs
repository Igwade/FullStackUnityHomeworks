using App.SaveLoad.Entities;
using App.SaveLoad.Entities.ComponentSerializers;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class DestinationPoint : MonoBehaviour, ISerializableComponent<DestinationPointSerializer>
    {
        ///Variable
        [field: SerializeField]
        public Vector3 Value { get; set; }
    }
}