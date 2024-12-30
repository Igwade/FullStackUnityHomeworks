using Components.Core;
using UnityEngine;

namespace Components.Audio
{
    public class DestroyerAudioComponent: MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private DestroyerComponent destroyerComponent;
        [SerializeField] private AudioClip clip;

        private void OnEnable() => destroyerComponent.OnDestroyed += OnDestroyed;
        private void OnDisable() => destroyerComponent.OnDestroyed -= OnDestroyed;
        private void OnDestroyed(Transform _) => PlaySound();
        private void PlaySound() => audioSource.PlayOneShot(clip);
    }
}