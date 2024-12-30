using Components.Core;
using UnityEngine;

namespace Components.Audio
{
    public class PushAudioComponent: MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip pushClip;
        [SerializeField] private PushComponent pushComponent;
        
        private void OnEnable() => pushComponent.OnPush += Play;
        private void OnDisable() => pushComponent.OnPush -= Play;
        private void Play() => audioSource.PlayOneShot(pushClip);
    }
}