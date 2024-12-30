using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Core
{
    public class OverlapSphereComponent: MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform center;
        
        private readonly Collider2D[] _colliders = new Collider2D[10];
        
        public IEnumerable<Collider2D> GetColliders()
        {
            var count = Physics2D.OverlapCircleNonAlloc(center.position, radius, _colliders, layerMask);
            return _colliders.Take(count);
        }
        
        private void OnDrawGizmos()
        {
            if (center == null)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center.position, radius);
        }
    }
}