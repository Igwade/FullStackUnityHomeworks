using UnityEngine;

namespace Components.Push
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