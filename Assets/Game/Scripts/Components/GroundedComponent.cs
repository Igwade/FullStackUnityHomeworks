using Sirenix.OdinInspector;
using UnityEngine;

namespace Components
{
    public class GroundedComponent : MonoBehaviour
    {
        [SerializeField] private Transform feetPoint;
        [SerializeField] private float checkDistance;
        [SerializeField] private LayerMask groundLayer;

        [ShowInInspector, ReadOnly]
        public Transform GroundTransform { get; private set; }
        
        public bool IsGrounded() => GroundTransform != null;

        private void FixedUpdate()
        {
            var raycast = Physics2D.Raycast(feetPoint.position, Vector3.down, checkDistance, groundLayer);
            GroundTransform = raycast.transform;
        }

        private void OnDrawGizmos()
        {
            if (feetPoint == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(feetPoint.position, feetPoint.position + Vector3.down * checkDistance);
        }
    }
}