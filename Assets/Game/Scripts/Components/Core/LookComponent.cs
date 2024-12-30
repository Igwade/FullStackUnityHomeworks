using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Core
{
    public class LookComponent : MonoBehaviour
    {
        [SerializeField] private Transform head;

        [ShowInInspector, ReadOnly] public Vector2 Direction { get; private set; } = Vector2.right;

        public void SetLookDirection(Vector2 moveDirection)
        {
            if (moveDirection == Vector2.zero)
            {
                return;
            }
            
            if (Mathf.Sign(moveDirection.x) > 0)
            {
                Direction = Vector2.right;
            }

            if (Mathf.Sign(moveDirection.x) < 0)
            {
                Direction = Vector2.left;
            }

            head.transform.rotation = Quaternion.Euler(0, Direction == Vector2.right ? 0 : 180, 0);
        }
    }
}