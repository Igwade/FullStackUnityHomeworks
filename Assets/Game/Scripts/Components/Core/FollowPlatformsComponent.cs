using UnityEngine;
using Utils.TagSelector.Attributes;

namespace Components.Core
{
    // Сделал по простому, почему бы и нет. Сложнее пока и не требуется.
    public class FollowPlatformsComponent : MonoBehaviour
    {
        [SerializeField, TagSelector] private string platformTag = "Platform";
        [SerializeField] private GroundedComponent groundedComponent;
        [SerializeField] private Transform worldParent;

        public void FixedUpdate()
        {
            var ground = groundedComponent.GroundTransform;

            if (ground != null && ground.CompareTag(platformTag))
            {
                transform.SetParent(ground);
            }
            else
            {
                transform.SetParent(worldParent);
            }
        }
    }
}