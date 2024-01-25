using UnityEngine;

public class TVCharacterRepresentation : MonoBehaviour, IUserRepresentation
{
    [SerializeField] private Transform _positionTransform;
    [SerializeField] private GameObject _noSignalImage;

    [SerializeField] private Transform _explosionCentre;
    [SerializeField] private float _forceMultiplier = 1f;

    [SerializeField] private Animator _animator;
    public Transform GetPosition() => _positionTransform;

    public void EnablePhysics()
    {
        _animator.enabled = false;
        _noSignalImage.SetActive(true);
        SetChildrenKinematic(false);
    }

    public void DisablePhysics()
    {
        _animator.enabled = true;
        _noSignalImage.SetActive(false);
        SetChildrenKinematic(true);
    }

    public void SetInvisible() =>
        gameObject.SetActive(false);

    public void SetVisible() =>
        gameObject.SetActive(true);

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;

    private void Awake()
    {
        // cache it to prevent multiple c++ calls
        var myTransform = transform;
        
        _startingPosition = myTransform.position;
        _startingRotation = myTransform.rotation;
    }

    public void ResetRepresentation()
    {
        transform.SetPositionAndRotation(_startingPosition, _startingRotation);
    }

    public void AddExplosionForce(float force)
    {
        GetComponentInChildren<Rigidbody>().AddExplosionForce(force * _forceMultiplier, _explosionCentre.position, 5f);
    }
    
    private Rigidbody[] _childRigidbodies;
    
    private void SetChildrenKinematic(bool kinematic)
    {
        if (_childRigidbodies == null)
            _childRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (var childRigidbody in _childRigidbodies)
            childRigidbody.isKinematic = kinematic;
    }
}
