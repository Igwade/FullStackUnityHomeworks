using Common;
using UnityEngine;

namespace Components.Core
{
    // Эксперименты
    public class PushAreaComponent : PushComponent
    {
        [SerializeField] private OverlapSphereComponent overlapSphereComponent;

        public void PushArea()
        {
            if (!CanPush())
                return;

            foreach (var col in overlapSphereComponent.GetColliders())
            {
                Push(col);
            }

            AfterPush();
        }
    }
}