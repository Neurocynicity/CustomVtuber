using System;
using UnityEngine;

public class ConstantConfettiManager : MonoBehaviour
{
    public ParticleSystem constantConfetti;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (constantConfetti.isPlaying)
                constantConfetti.Stop();
            else
                constantConfetti.Play();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            var emission = constantConfetti.emission;

            if (emission.rateOverTimeMultiplier == 50f)
                emission.rateOverTimeMultiplier = 125f;
            else
                emission.rateOverTimeMultiplier = 50f;
        }
    }
}
