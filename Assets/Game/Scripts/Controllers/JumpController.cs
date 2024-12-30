using Components.Core;
using UnityEngine;

namespace SampleGame
{
    public class JumpController: MonoBehaviour
    {
        [SerializeField]
        private GameObject character;
        
        private JumpComponent _jumpComponent;
        
        private void Awake()
        {
            _jumpComponent = character.GetComponent<JumpComponent>();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpComponent.Jump();
            }
        }
    }
}