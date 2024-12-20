using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class Wallet
    {
        [ShowInInspector]
        public int Coins { get; set; }
    }
}