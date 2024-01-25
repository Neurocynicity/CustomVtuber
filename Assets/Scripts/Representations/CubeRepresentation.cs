using System.Collections.Generic;
using UnityEngine;

public class CubeRepresentation : MonoBehaviour, IUserRepresentation
{
    [Tooltip("This should contain every script overriding the movement of the cube, " +
             "so that we can disable them and allow physics to take over")]
    [SerializeField] private List<MonoBehaviour> _movementControllers;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _explosionCentre;
    private Vector3 _startingPosition;

    private void Awake()
    {
        _startingPosition = transform.position;
    }

    public Transform GetPosition() => transform;

    public void EnablePhysics()
    {
        foreach (var movementController in _movementControllers)
            movementController.enabled = false;
        
        _rigidbody.isKinematic = false;
    }

    public void DisablePhysics()
    {
        foreach (var movementController in _movementControllers)
            movementController.enabled = true;
        
        _rigidbody.isKinematic = true;
    }

    public void SetInvisible()
    {
        gameObject.SetActive(false);
    }

    public void SetVisible()
    {
        gameObject.SetActive(true);
    }

    public void ResetRepresentation()
    {
        transform.SetPositionAndRotation(
            _startingPosition,
            Quaternion.identity);
    }

    public void AddExplosionForce(float force)
    {
        _rigidbody.AddExplosionForce(force, _explosionCentre.position, 5f);
    }
}