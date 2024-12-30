using Components.Core;
using Components.Core.Aspects;
using UnityEngine;

namespace Objects
{
    public class Trap: MonoBehaviour, IDestroyable
    {
        [SerializeField] private CollisionDamageDealer collisionDamageDealer;
        [SerializeField] private HealthComponent healthComponent;

        private void OnEnable()
        {
            healthComponent.OnDeath += Destroy;
            collisionDamageDealer.OnDealDamage += OnDealCollisionDamage;
        }

        private void OnDisable()
        {
            healthComponent.OnDeath -= Destroy;
            collisionDamageDealer.OnDealDamage -= OnDealCollisionDamage;
        }

        private void OnDealCollisionDamage(Transform _) => Destroy();
        public void Destroy() => Destroy(gameObject);
    }
}