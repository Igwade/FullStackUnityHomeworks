using Modules.ComponentSerialization.Runtime.Attributes;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class DestinationPoint : MonoBehaviour
    {
        ///Variable
        [Save]
        [field: SerializeField]
        public Vector3 Value { get; set; }
    }
}