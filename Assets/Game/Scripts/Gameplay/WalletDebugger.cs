using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Gameplay
{
    public class WalletDebugger: MonoBehaviour
    {
        [FormerlySerializedAs("Wallet")] [Inject] public Wallet wallet;
    }
}