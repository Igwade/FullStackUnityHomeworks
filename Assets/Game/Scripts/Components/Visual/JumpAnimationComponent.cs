using System;
using Components.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Visual
{
    public class JumpAnimationComponent: MonoBehaviour
    {
        [SerializeField] private Transform visual;
        [SerializeField] private JumpComponent jumpComponent;
        
        [Header("Animation Settings")]
        [SerializeField] private Vector3 punchScale = new(1.2f, 1.2f, 1.2f);
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private int vibrato = 1;
        [SerializeField] private float elasticity = 0;

        private void OnEnable() => jumpComponent.OnJump += PlayAnimation;
        private void OnDisable() => jumpComponent.OnJump -= PlayAnimation;

        [Button]
        private void PlayAnimation()
        {
            visual.DOPunchScale(punchScale, duration, vibrato, elasticity);
        }
    }
}