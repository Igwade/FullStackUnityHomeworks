using Sirenix.OdinInspector;
using UnityEngine;

namespace Components
{
    public class MoveComponent : MonoBehaviour
    {
        public interface ICondition
        {
            bool CanMove();
        }

        [SerializeField] private new Transform transform;
        [SerializeField] private float speed = 3f;
        [ShowInInspector] public Vector2 MoveDirection { get; private set; }

        private ICondition _condition;

        public void Construct(ICondition condition) => _condition = condition;
        private void FixedUpdate() => Move();
        public void SetDirection(Vector2 direction) => MoveDirection = direction.normalized;

        private void Move()
        {
            if (_condition != null && !_condition.CanMove())
                return;

            transform.position += (Vector3)MoveDirection.normalized * speed * Time.deltaTime;
        }
    }
}