using UnityEngine;

namespace CustomRagdollCreator
{
    public static class UtilityFunctions
    {
        public static void EnsureAttachedComponent<T>(this GameObject gameObject, ref T component) where T : Component
        {
            if (component || gameObject.TryGetComponent(out component))
                return;

            component = gameObject.AddComponent<T>();
        }
        
        public static Vector3 GetLocalDirectionOfCapsuleCollider(this CapsuleCollider collider)
            => collider.direction switch
            {
                0 => Vector3.right,
                1 => Vector3.up,
                2 => Vector3.forward,
                _ => Vector3.zero
            };
    }
}
