using Components.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Visual
{
    public class TakeDamageAnimationComponent : MonoBehaviour
    {
        [SerializeField] private TakeDamageComponent takeDamageComponent;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color damageColor = Color.red;

        private Color _originalColor;
        private Tween _currentTween;

        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }

        private void OnEnable() => takeDamageComponent.OnTakeDamage += HandleTakeDamage;
        private void OnDisable() => takeDamageComponent.OnTakeDamage -= HandleTakeDamage;
        private void HandleTakeDamage(int _) => PlayAnimation();

        [Button]
        private void PlayAnimation()
        {
            _currentTween?.Kill();

            _currentTween = spriteRenderer
                .DOColor(damageColor, 0.1f)
                .SetLoops(6, LoopType.Yoyo)
                .OnComplete(() => spriteRenderer.color = _originalColor);
        }
    }
}