using System.Collections.Generic;
using Modules.Entities;
using SaveLoadEntitiesExtension.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class ProductionOrder : MonoBehaviour
    {
        ///Variable
        [FormerlySerializedAs("_queue")] [SerializeField]
        private List<EntityConfig> queue;
        
        [Saveable]
        public IReadOnlyList<EntityConfig> Queue
        {
            get { return queue; }
            set { queue = new List<EntityConfig>(value); }
        }
    }
}