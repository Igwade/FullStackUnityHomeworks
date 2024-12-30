using System;
using Common;
using SampleGame;
using UnityEngine;
using Timer = Common.Timer;

namespace Components.Core
{
    public class JumpComponent: MonoBehaviour
    {
        public interface ICondition
        {
            bool CanJump();
        }
        
        public event Action OnJump;
        
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private Timer cooldown;
        
        private ICondition _condition;

        public void Construct(ICondition condition)
        {
            _condition = condition;
        }

        public void Update()
        {
            cooldown.Tick(Time.deltaTime);
        }

        public void Jump()
        {
            if ((_condition != null && !_condition.CanJump()) || cooldown.IsInProgress())
            {
                return;
            }
            
            ForceApplier.ApplyForce(rigidbody, jumpForce, ForceMode2D.Impulse);
            cooldown.Reset();
            OnJump?.Invoke();
        }
    }
}