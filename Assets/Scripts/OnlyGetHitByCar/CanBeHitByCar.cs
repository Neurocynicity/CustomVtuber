using System.Collections.Generic;
using UnityEngine;

public class CanBeHitByCar : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10)]
    private float _forceMultiplier = 1f;
    
    [SerializeField]
    private Rigidbody _rootRigidbody;

    private Rigidbody[] _bodyRigidbodies;

    private readonly Dictionary<Rigidbody, Pose> _boneStartPositions = new();

    private void Awake()
    {
        _bodyRigidbodies = _rootRigidbody.gameObject.GetComponentsInChildren<Rigidbody>();

        foreach (var bodyRigidbody in _bodyRigidbodies)
            _boneStartPositions.Add(bodyRigidbody, new Pose(bodyRigidbody.position, bodyRigidbody.rotation));
        
        DisablePhysics();
    }

    public void EnablePhysics() => SetPhysicsActive(true);
    public void DisablePhysics() => SetPhysicsActive(false);

    private void SetPhysicsActive(bool active)
    {
        foreach (var bodyRigidbody in _bodyRigidbodies)
            bodyRigidbody.isKinematic = !active;
    }

    public void ResetBody()
    {
        foreach (var bodyRigidbody in _bodyRigidbodies)
        {
            bodyRigidbody.transform.SetPositionAndRotation(
                _boneStartPositions[bodyRigidbody].position,
                _boneStartPositions[bodyRigidbody].rotation
                );
        }
    }

    public void AddExplosionForce(float intensity, Vector3 centre)
    {
        _rootRigidbody.AddExplosionForce(intensity * _forceMultiplier, centre, 5f);
    }
    
    private void Reset()
    {
        TryGetComponent(out _rootRigidbody);
    }
}
