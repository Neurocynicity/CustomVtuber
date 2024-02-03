using UnityEditor;
using UnityEngine;

namespace CustomRagdollCreator
{
    public static class CustomEditorUtility
    {
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
            => new Vector3(
                a.x * b.x,
                a.y * b.y,
                a.z * b.z
            );
        
        public static Vector3 Divide(this Vector3 a, Vector3 b)
            => new Vector3(
                a.x / b.x,
                a.y / b.y,
                a.z / b.z
            );
    }
}