using Components.Core;
using UnityEngine;

namespace Components.Vfx
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