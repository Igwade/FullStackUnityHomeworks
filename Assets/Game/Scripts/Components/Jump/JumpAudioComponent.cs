using UnityEngine;

namespace Components.Jump
{
    public class JumpAudioComponent: MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private JumpComponent jumpComponent;
        [SerializeField] private AudioClip clip;

        private void OnEnable() => jumpComponent.OnJump += PlaySound;
        private void OnDisable() => jumpComponent.OnJump -= PlaySound;
        private void PlaySound() => audioSource.PlayOneShot(clip);
    }
}