using App.SaveLoad.Entities;
using App.SaveLoad.Entities.ComponentSerializers;
using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class TargetObject : MonoBehaviour, ISerializableComponent<TargetObjectSerializer>
    {
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }
    }
}