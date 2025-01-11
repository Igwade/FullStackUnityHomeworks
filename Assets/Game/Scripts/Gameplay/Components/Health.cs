using Modules.ComponentSerialization.Runtime.Attributes;
using UnityEngine;

namespace Game.Scripts.Gameplay.Components
{
    //Can be extended
    [SaveComponent]
    public sealed class Health : MonoBehaviour
    {
        ///Variable
        [Save]
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;
    }
}