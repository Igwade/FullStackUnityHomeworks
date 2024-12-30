using Components.Core;
using Components.Core.Aspects;
using UnityEngine;

namespace Objects
{
    public class Character: MonoBehaviour, IDestroyable, JumpComponent.ICondition, MoveComponent.ICondition, PushComponent.ICondition
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private MoveComponent moveComponent;
        [SerializeField] private LookComponent lookComponent;
        [SerializeField] private GroundedComponent groundedComponent;
        [SerializeField] private JumpComponent jumpComponent;
        [SerializeField] private PushAreaComponent pushComponent;
        [SerializeField] private PushAreaComponent tossComponent;
        
        private void Awake()
        {
            jumpComponent.Construct(this);
            moveComponent.Construct(this);
            pushComponent.Construct(this);
            tossComponent.Construct(this);
        }

        private void Update()
        {
            lookComponent.SetLookDirection(moveComponent.MoveDirection);
            pushComponent.ApplyDirection(lookComponent.Direction);
        }

        private void OnEnable() => healthComponent.OnDeath += Destroy;
        private void OnDisable() => healthComponent.OnDeath -= Destroy;
        public void Destroy() => gameObject.SetActive(false);

        bool JumpComponent.ICondition.CanJump() => groundedComponent.IsGrounded();
        bool MoveComponent.ICondition.CanMove() => healthComponent.IsAlive();
        bool PushComponent.ICondition.CanPush() => healthComponent.IsAlive();

        public void Push() => pushComponent.PushArea();
        public void Toss() => tossComponent.PushArea();
    }
}