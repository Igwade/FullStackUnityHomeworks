using System;
using Components.Core.Aspects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Core
{
    [RequireComponent(typeof(HealthComponent))]
    public class TakeDamageComponent: MonoBehaviour, IDamageable
    {
        public event Action<int> OnTakeDamage;

        private HealthComponent _healthComponent;
        
        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
        }

        [Button]
        public bool TakeDamage(int damage)
        {
            if (_healthComponent.TakeDamage(damage))
            {
                OnTakeDamage?.Invoke(damage);
                return true;
            }
            
            return false;
        }
    }
}