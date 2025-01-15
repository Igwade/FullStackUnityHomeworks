using UnityEngine;

namespace Components.Push
{
    public class PushVfxComponent: MonoBehaviour
    {
        [SerializeField] private ParticleSystem vfx;
        [SerializeField] private PushComponent pushComponent;

        private void OnEnable() => pushComponent.OnPush += Play;
        private void OnDisable() => pushComponent.OnPush -= Play;
        private void Play() => vfx.Play();
    }
}