using Aspects;
using Components;
using Components.Jump;
using Components.Push;
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
        [SerializeField] private PushComponent pushComponent;
        [SerializeField] private PushComponent tossComponent;
        
        private void Awake()
        {
            jumpComponent.Construct(this);
            moveComponent.Construct(this);
            pushComponent.Construct(new PushCondition(this));
            tossComponent.Construct(new TossCondition(this));
        }

        private void Update()
        {
            lookComponent.SetLookDirection(moveComponent.MoveDirection);
            pushComponent.ApplyDirection(lookComponent.Direction);
        }

        private void OnEnable() => healthComponent.OnDeath += Destroy;
        private void OnDisable() => healthComponent.OnDeath -= Destroy;
        public void Destroy() => gameObject.SetActive(false);

        bool JumpComponent.ICondition.CanJump() => groundedComponent.IsGrounded() && healthComponent.IsAlive();
        bool MoveComponent.ICondition.CanMove() => healthComponent.IsAlive();
        bool PushComponent.ICondition.CanPush() => healthComponent.IsAlive();

        public void Push() => pushComponent.PushArea();
        public void Toss() => tossComponent.PushArea();
        
        class PushCondition : PushComponent.ICondition
        {
            private readonly Character _character;
            public PushCondition(Character character) => _character = character;
            public bool CanPush() => _character.healthComponent.IsAlive();
        }
        
        class TossCondition : PushComponent.ICondition
        {
            private readonly Character _character;
            public TossCondition(Character character) => _character = character;
            public bool CanPush() => _character.healthComponent.IsAlive() && _character.groundedComponent.IsGrounded();
        }
    }
}