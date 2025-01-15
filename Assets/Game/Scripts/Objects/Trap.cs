using Aspects;
using Components;
using UnityEngine;

namespace Objects
{
    public class Trap: MonoBehaviour, IDestroyable
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private CollisionComponent collisionComponent;
        [SerializeField] private DamageDealerComponent damageDealerComponent;

        private void OnEnable()
        {
            healthComponent.OnDeath += Destroy;
            damageDealerComponent.OnDealDamage += OnDealCollisionDamage;
            collisionComponent.OnCollisionEnter += damageDealerComponent.TryDealDamage;
        }

        private void OnDisable()
        {
            healthComponent.OnDeath -= Destroy;
            damageDealerComponent.OnDealDamage -= OnDealCollisionDamage;
            collisionComponent.OnCollisionEnter -= damageDealerComponent.TryDealDamage;
        }

        private void OnDealCollisionDamage(Transform _) => Destroy();
        public void Destroy() => Destroy(gameObject);
    }
}