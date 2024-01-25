using System;
using System.Collections.Generic;
using UnityEngine;

public class MoodHandler : MonoBehaviour
{
    public static event Action<AsciiFaceMoodData> OnMoodChanged;

    public List<AsciiFaceMoodData> MoodData;

    [HideInInspector]
    public AsciiFaceMoodData CurrentFaceData;

    private void Awake()
    {
        CurrentFaceData = MoodData[0];
    }

    private void Update()
    {
        foreach (var moodData in MoodData)
        {
            if (!Helper.IsHotkeyBeingInputted(moodData.MoodKeyCodes))
                continue;

            CurrentFaceData = moodData;
            OnMoodChanged?.Invoke(CurrentFaceData);
            return;
        }
    }
}