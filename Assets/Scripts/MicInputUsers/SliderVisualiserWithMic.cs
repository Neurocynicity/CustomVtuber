using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVisualiserWithMic : MicUser
{
    public Slider slider;

    public override void NewMicLevel(float level)
    {
        slider.value = level;
    }
}
