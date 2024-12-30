using System;
using Components.Core;
using Components.Core.Aspects;
using UnityEngine;

namespace Objects
{
    public class Spider: MonoBehaviour, IDestroyable, MoveComponent.ICondition
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private MoveComponent moveComponent;
        [SerializeField] private TriggerDamageDealer triggerDamageDealer;
        [SerializeField] private PushComponent pushComponent;
        
        private void Awake()
        {
            moveComponent.Construct(this);
        }

        private void OnEnable()
        {
            healthComponent.OnDeath += Destroy;
            triggerDamageDealer.OnDealDamage += OnDealDamage;
        }

        private void OnDisable()
        {
            healthComponent.OnDeath -= Destroy;
            triggerDamageDealer.OnDealDamage -= OnDealDamage;
        }

        private void OnDealDamage(Transform obj)
        {
            pushComponent.ApplyDirection(obj.position - transform.position);
            pushComponent.PushTarget(obj);
        }
        
        public void Destroy() => gameObject.SetActive(false);
        bool MoveComponent.ICondition.CanMove() => healthComponent.IsAlive();
    }
}