using UnityEngine;

public class UISpin : MonoBehaviour
{
    [SerializeField] private Vector3 _spinAxis = Vector3.forward;
    [SerializeField] private float _spinSpeed = 1f;
    
    void Update()
    {
        transform.Rotate(_spinAxis, _spinSpeed * Time.deltaTime);
    }
}
