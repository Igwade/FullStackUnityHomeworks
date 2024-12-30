using System;
using Common;
using Components.Core.Aspects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components.Core
{
    
    public class TriggerDamageDealer : MonoBehaviour
    {
        public event Action<Transform> OnDealDamage;
        
        [SerializeField] private int damage;
        [SerializeField] private TriggerComponent triggerComponent;
        [SerializeField] private Timer cooldown;

        private void Awake() => cooldown.Reset();
        private void OnEnable() => triggerComponent.OnTriggerStay += HandleTriggerStay;
        private void OnDisable() => triggerComponent.OnTriggerStay -= HandleTriggerStay;
        private void Update() => cooldown.Tick(Time.deltaTime);

        private void HandleTriggerStay(Collider2D other)
        {
            if (!cooldown.IsTimeUp())
                return;
            
            var obj = other.transform;
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
                cooldown.Reset();
                OnDealDamage?.Invoke(obj);
            }
        }
    }
}