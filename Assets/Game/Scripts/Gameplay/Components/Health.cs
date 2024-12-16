using System;
using System.Collections.Generic;
using SaveLoadEntitiesExtension.Attributes;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be 
    [SaveComponent]
    public sealed class Health : MonoBehaviour
    {
        ///Variable
        [field: SerializeField, Saveable]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;
    }
}