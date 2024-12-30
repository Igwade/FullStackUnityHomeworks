using Components.Core;
using UnityEngine;

namespace Components.Audio
{
    public class TakeDamageAudioComponent: MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;
        [SerializeField] private TakeDamageComponent takeDamageComponent;
        
        private void OnEnable() => takeDamageComponent.OnTakeDamage += OnTakeDamage;
        private void OnDisable() => takeDamageComponent.OnTakeDamage -= OnTakeDamage;
        private void OnTakeDamage(int _) => Play(); 
        private void Play() => audioSource.PlayOneShot(clip);
    }
}