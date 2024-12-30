using System;
using Components.Core.Aspects;
using UnityEngine;

namespace Components.Core
{
    public class DestroyerComponent : MonoBehaviour
    {
        public event Action<Transform> OnDestroyed;
        
        public void Destroy(Component other)
        {
            if (other.TryGetComponent<IDestroyable>(out var destroyable))
            {
                destroyable.Destroy();
                OnDestroyed?.Invoke(other.transform);
            }
        }
    }
}