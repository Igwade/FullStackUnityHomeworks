using Components.Core;
using Components.Core.Aspects;
using UnityEngine;

namespace Objects
{
    public class Snake: MonoBehaviour, IDestroyable, MoveComponent.ICondition
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private MoveComponent moveComponent;
        [SerializeField] private LookComponent lookComponent;
        [SerializeField] private TriggerDamageDealer triggerDamageDealer;
        [SerializeField] private PushComponent pushComponent;

        private void Awake()
        {
            moveComponent.Construct(this);
        }

        private void OnEnable()
        {
            healthComponent.OnDeath += Destroy;
            triggerDamageDealer.OnDealDamage += pushComponent.PushTarget;
        }

        private void OnDisable()
        {
            healthComponent.OnDeath -= Destroy;
            triggerDamageDealer.OnDealDamage -= pushComponent.PushTarget;
        }

        private void Update()
        {
            lookComponent.SetLookDirection(moveComponent.MoveDirection);
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }

        bool MoveComponent.ICondition.CanMove() => healthComponent.IsAlive();
    }
}