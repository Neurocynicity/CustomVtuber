using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : MonoBehaviour
{
    public Transform targetTransform;

    void Update()
    {
        transform.position = targetTransform.position;
    }
}
