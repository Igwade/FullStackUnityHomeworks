using System.Collections.Generic;
using Modules.ComponentSerialization.Runtime.Attributes;
using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    [SaveComponent]
    public sealed class ProductionOrder : MonoBehaviour
    {
        ///Variable
        [SerializeField]
        private List<EntityConfig> _queue;
        
        [Save]
        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }
    }
}