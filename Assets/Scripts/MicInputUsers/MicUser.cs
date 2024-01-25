using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MicUser : MonoBehaviour
{
    MicrophoneListener microphoneListener;

    protected virtual void Awake()
    {
        microphoneListener =  FindObjectOfType<MicrophoneListener>();
    }

    protected virtual void OnEnable()
    {
        microphoneListener.MicrophoneUsers.Add(this);
    }

    protected virtual void OnDisable()
    {
        microphoneListener.MicrophoneUsers.Remove(this);
    }

    public abstract void NewMicLevel(float level);
}
