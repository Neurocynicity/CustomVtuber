using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoodUI : MonoBehaviour
{
    public MoodHandler moodHandler;

    AsciiFaceMoodData myAsciiFaceMoodData;

    public TMP_InputField MoodName, MouthOpenFace, MouthClosedFace;
    public Button HotkeyButton;
    public TMP_Text hotkeyText;

    public void SetMoodData(AsciiFaceMoodData data)
    {
        myAsciiFaceMoodData = data;

        MoodName.text = data.name;
        MouthOpenFace.text = data.talkingFace;
        MouthClosedFace.text = data.notTalkingFace;

        hotkeyText.text = string.Join("+", data.MoodKeyCodes);
    }

    public void DeleteMood()
    {
        moodHandler.MoodData.Remove(myAsciiFaceMoodData);
    }

    public void StartRebindMood() => StartCoroutine(RebindMood());

    public IEnumerator RebindMood()
    {
        hotkeyText.text = "listening...";

        List<KeyCode> keycodes = new();

        // wait until no buttons pressed in case they're fucking with me
        yield return new WaitUntil(() => !Input.anyKey);

        // wait until button are now pressed
        yield return new WaitUntil(() => Input.anyKey);

        // register all buttons pressed until none pressed
        while (Input.anyKey)
        {
            if (Input.anyKeyDown)
            {
                foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (!Input.GetKeyDown(kcode)) continue;
                    keycodes.Add(kcode);
                    break;
                }
            }

            yield return null;
        }

        myAsciiFaceMoodData.MoodKeyCodes = keycodes.Distinct().ToList();
        hotkeyText.text = string.Join("+", myAsciiFaceMoodData.MoodKeyCodes);
    }
}
