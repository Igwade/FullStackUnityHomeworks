using App.SaveLoad.Entities;
using App.SaveLoad.Entities.ComponentSerializers;
using UnityEngine;

namespace Game.Scripts.Gameplay.Components
{
    //Can be extended
    public sealed class Health : MonoBehaviour, ISerializableComponent<HealthSerializer>
    {
        ///Variable
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;
    }
}