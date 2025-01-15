using System;
using UnityEngine;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnDeath;

        [SerializeField] private int maxPoints;
        [SerializeField] private int hitPoints;
        [SerializeField] private bool isDead;

        public bool IsAlive() => !isDead && gameObject.activeSelf;
        private void Awake() => hitPoints = maxPoints;

        public bool TakeDamage(int damage)
        {
            if (isDead)
            {
                return false;
            }

            hitPoints -= damage;
            if (hitPoints <= 0)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
            
            return true;
        }
    }
}