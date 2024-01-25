using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMoodUI : MonoBehaviour
{
    public MoodHandler handler;
    public Transform moodsViewArea;
    public GameObject MoodPrefab;

    private void Awake()
    {
        GenerateMoodsUI();
    }

    public void GenerateMoodsUI()
    {
        foreach (var moodData in handler.MoodData)
        {
            GameObject newMoodUI = Instantiate(MoodPrefab, moodsViewArea);
            MoodUI moodUI = newMoodUI.GetComponent<MoodUI>();
            moodUI.moodHandler = handler;
            moodUI.SetMoodData(moodData);
        }
    }
}
