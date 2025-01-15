using Components;
using Components.Push;
using UnityEngine;

namespace Objects
{
    public class Trampoline: MonoBehaviour
    {
        [SerializeField] private PushComponent pushComponent;
        [SerializeField] private TriggerComponent triggerComponent;
        
        private void OnEnable() => triggerComponent.OnTriggerEnter += pushComponent.PushTarget;
        private void OnDisable() => triggerComponent.OnTriggerEnter -= pushComponent.PushTarget;
    }
}