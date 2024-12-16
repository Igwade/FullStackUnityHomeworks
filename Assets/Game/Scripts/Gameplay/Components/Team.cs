using SampleGame.Common;
using SaveLoadEntitiesExtension.Attributes;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }
    }
}