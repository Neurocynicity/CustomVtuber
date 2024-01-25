using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnore : MonoBehaviour
{
    [SerializeField] private List<Collider> _colliders1;
    [SerializeField] private List<Collider> _colliders2;

    private void Awake()
    {
        foreach (var col1 in _colliders1)
        {
            foreach (var col2 in _colliders2)
            {
                Physics.IgnoreCollision(col1, col2);
            }
        }
    }
}
