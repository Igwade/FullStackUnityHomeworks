using UnityEngine;

namespace Common
{
    // Наверно увлёкся уже
    public class ForceApplier
    {
        public static void ApplyForce(Rigidbody2D rigidbody, Vector2 force, ForceMode2D forceMode)
        {
            rigidbody.AddForce(force, forceMode);
        }
        
        public static bool ApplyForce(Component component, Vector2 force, ForceMode2D forceMode)
        {
            if (component.TryGetComponent(out Rigidbody2D rigidbody))
            {
                ApplyForce(rigidbody, force, forceMode);
                return true;
            }

            return false;
        }
    }
}