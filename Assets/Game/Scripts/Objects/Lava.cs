using Components;
using Components.Destroyer;
using UnityEngine;

namespace Objects
{
    public class Lava: MonoBehaviour
    {
        [SerializeField] private TriggerComponent triggerComponent;
        [SerializeField] private DestroyerComponent destroyerComponent;
        
        private void OnEnable() => triggerComponent.OnTriggerEnter += destroyerComponent.Destroy;
        private void OnDisable() => triggerComponent.OnTriggerEnter -= destroyerComponent.Destroy;
    }
}