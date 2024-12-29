using App.SaveLoad.Entities;
using App.SaveLoad.Entities.ComponentSerializers;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour, ISerializableComponent<TeamSerializer>
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }
    }
}