using Sirenix.OdinInspector;
using UnityEngine;

namespace Components
{
    public class WaypointMovementComponent: MonoBehaviour
    {
        [SerializeField] private MoveComponent moveComponent;
        [SerializeField] private new Transform transform;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float stoppingDistance;
        
        [ShowInInspector]
        private int _currentWaypointIndex;
        
        private void Start()
        {
            _currentWaypointIndex = 0;
        }
        
        private void Update()
        {
            if (Vector2.Distance(transform.position, waypoints[_currentWaypointIndex].position) < stoppingDistance)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= waypoints.Length)
                {
                    _currentWaypointIndex = 0;
                }
            }
            
            var direction = waypoints[_currentWaypointIndex].position - transform.position;
            moveComponent.SetDirection(direction);
        }
    }
}