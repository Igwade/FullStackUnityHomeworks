using Modules.ComponentSerialization.Runtime.Attributes;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class Team : MonoBehaviour
    {
        ///Variable
        [Save]
        [field: SerializeField]
        public TeamType Type { get; set; }
    }
}