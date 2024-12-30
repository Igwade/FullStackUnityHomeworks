using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Components.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerComponent: MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerStay;
        public event Action<Collider2D> OnTriggerExit;
        
        private void OnTriggerEnter2D(Collider2D other) => OnTriggerEnter?.Invoke(other);
        private void OnTriggerStay2D(Collider2D other) => OnTriggerStay?.Invoke(other);
        private void OnTriggerExit2D(Collider2D other) => OnTriggerExit?.Invoke(other);
        private void OnValidate() => Assert.IsTrue(GetComponents<Collider2D>().Any(x => x.isTrigger));
    }
}