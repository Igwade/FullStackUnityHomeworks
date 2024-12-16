using SampleGame.Common;
using SaveLoadEntitiesExtension.Attributes;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class ResourceBag : MonoBehaviour
    {
        ///Variable
        [field: SerializeField, Saveable]
        public ResourceType Type { get; set; }
        
        ///Variable
        [field: SerializeField, Saveable]
        public int Current { get; set; }
        
        ///Const
        [field: SerializeField]
        public int Capacity { get; set; }
    }
}