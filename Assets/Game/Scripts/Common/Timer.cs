using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common
{
    [Serializable]
    public class Timer
    {
        public event Action OnTimeIsUp;

        [SerializeField] private float duration;
        [SerializeField, ReadOnly] private float time;

        public bool IsTimeUp() => time <= 0;
        public bool IsInProgress() => time > 0;

        public void SetDuration(float duration)
        {
            this.duration = duration;
            time = duration;
        }

        public void Tick(float deltaTime)
        {
            if (time <= 0)
            {
                return;
            }
            
            time -= deltaTime;
            if (time <= 0)
            {
                OnTimeIsUp?.Invoke();
            }
        }

        [Button]
        public void Reset()
        {
            time = duration;
        }
    }
}