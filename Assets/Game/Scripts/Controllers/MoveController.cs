using Components;
using Components.Core;
using UnityEngine;

namespace SampleGame
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField]
        private GameObject character;
        
        private MoveComponent _moveComponent;
        
        private void Awake()
        {
            _moveComponent = character.GetComponent<MoveComponent>();
        }

        private void Update()
        {
            Move(Vector3.zero);
            
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Move(Vector2.left);
            }
            
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Move(Vector2.right);
            }
        }

        private void Move(Vector2 direction) => _moveComponent.SetDirection(direction);
    }
}