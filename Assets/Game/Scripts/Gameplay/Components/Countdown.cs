using Modules.ComponentSerialization.Runtime.Attributes;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class Countdown : MonoBehaviour
    {
        ///Variable
        [Save]
        [field: SerializeField]
        public float Current { get; set; }

        ///Const
        [field: SerializeField]
        public float Duration { get; private set; }
    }
}