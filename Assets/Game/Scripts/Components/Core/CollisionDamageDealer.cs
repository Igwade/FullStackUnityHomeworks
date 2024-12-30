using System;
using Components.Core.Aspects;
using UnityEngine;

namespace Components.Core
{
    public class CollisionDamageDealer : MonoBehaviour
    {
        public event Action<Transform> OnDealDamage;

        [SerializeField] private int damage;
        [SerializeField] private CollisionComponent collisionComponent;
        
        private void OnEnable() => collisionComponent.OnCollisionEnter += HandleCollisionEnter;
        private void OnDisable() => collisionComponent.OnCollisionEnter -= HandleCollisionEnter;

        private void HandleCollisionEnter(Collision2D other)
        {
            var obj = other.transform;
            if (obj.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.TakeDamage(damage))
                {
                    OnDealDamage?.Invoke(obj);
                }
            }
        }
    }
}