using System;
using Aspects;
using Common;
using UnityEngine;

namespace Components
{
    public class DamageDealerComponent : MonoBehaviour
    {
        public event Action<Transform> OnDealDamage;

        // Можно было бы вынести в сам объект, но пока оно и тут нормально
        [SerializeField] private int damage;
        [SerializeField] private Timer cooldown;
        
        private void Awake() => cooldown.Reset();
        private void Update() => cooldown.Tick(Time.deltaTime);
        
        public bool TryDealDamage(Transform component)
        {
            if (!cooldown.IsTimeUp())
                return false;
            
            if (component.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.TakeDamage(damage))
                {
                    cooldown.Reset();
                    OnDealDamage?.Invoke(component);
                    return true;
                }
            }

            return false;
        }

        public void TryDealDamage(Collision2D collision)
            => TryDealDamage(collision.transform);
        
        public void TryDealDamage(Collider2D collider)
            => TryDealDamage(collider.transform);
    }
}