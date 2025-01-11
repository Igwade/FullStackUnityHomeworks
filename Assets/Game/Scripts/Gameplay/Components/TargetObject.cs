using Modules.ComponentSerialization.Runtime.Attributes;
using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class TargetObject : MonoBehaviour
    {
        ///Variable
        [Save]
        [field: SerializeField]
        public Entity Value { get; set; }
    }
}