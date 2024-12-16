using SaveLoadEntitiesExtension.Attributes;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class DestinationPoint : MonoBehaviour
    {
        ///Variable
        [field: SerializeField, Saveable]
        public Vector3 Value { get; set; }
    }
}