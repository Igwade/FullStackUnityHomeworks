using Modules.ComponentSerialization.Runtime.Attributes;
using UnityEngine;

namespace Gameplay.Components
{
    // Saved implicitly
    public class TestComponent : MonoBehaviour
    {
        [field: SerializeField]
        public int Value { get; set; }
    }
}