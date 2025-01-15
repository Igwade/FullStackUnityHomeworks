using System;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Push
{
    public class PushComponent : MonoBehaviour
    {
        public interface ICondition
        {
            bool CanPush();
        }

        public event Action OnPush;

        [SerializeField]
        protected Vector2 pushForce;
        
        [SerializeField]
        protected Timer cooldown;
        
        [ShowInInspector, ReadOnly]
        protected Vector2 DirectionMultiplier = Vector2.one;
        
        [SerializeField, Title("Необходимо для PushArea, но не обязательно")]
        private OverlapSphereComponent overlapSphereComponent;
        
        
        private ICondition _condition;
        protected bool CanPush() => cooldown.IsTimeUp() && (_condition?.CanPush() ?? true);

        public void Construct(ICondition condition) => _condition = condition;
        private void Start() => cooldown.Reset();
        private void Update() => cooldown.Tick(Time.deltaTime);

        public void PushTarget(Component other)
        {
            if (!CanPush())
                return;

            Push(other);
            AfterPush();
        }

        public void PushArea()
        {
            if (!CanPush())
                return;

            foreach (var col in overlapSphereComponent.GetColliders())
            {
                Push(col);
            }

            AfterPush();
        }

        protected void Push(Component other)
        {
            if (other.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(pushForce * DirectionMultiplier, ForceMode2D.Impulse);
            }
        }

        public void ApplyDirection(Vector2 direction)
        {
            DirectionMultiplier = direction.normalized;
        }

        protected void AfterPush()
        {
            cooldown.Reset();
            OnPush?.Invoke();
        }
    }
}