using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMic : MicUser
{
    public float movementAmount;

    Vector3 _startPosition;

    protected override void Awake()
    {
        base.Awake();
        _startPosition = transform.position;
    }

    public override void NewMicLevel(float level)
    {
        transform.position = _startPosition + level * movementAmount * Vector3.up;
    }
}
