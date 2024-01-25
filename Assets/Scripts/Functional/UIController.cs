using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MicrophoneListener microphoneListener;

    public GameObject allUI;
    public Slider sensitivitySlider;
    public Slider threshholdSlider;
    public TMP_Dropdown audioDeviceDropDown;

    private void Awake()
    {
        microphoneListener.Sensitivity = sensitivitySlider.value;
        microphoneListener.MinThreshold = threshholdSlider.value;

        UpdateAudioDevices();
    }

    public void UpdateAudioDevices()
    {
        audioDeviceDropDown.options.Clear();

        foreach (var device in Microphone.devices)
        {
            audioDeviceDropDown.AddOptions(new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData(device) });
        }
    }

    private void SetAudioDevice(int i)
    {
        microphoneListener.UpdateAudioDevice(audioDeviceDropDown.options[i].text);
    }

    private void OnEnable()
    {
        audioDeviceDropDown.onValueChanged.AddListener(SetAudioDevice);
    }

    private void OnDisable()
    {
        audioDeviceDropDown.onValueChanged.RemoveListener(SetAudioDevice);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        allUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        allUI.SetActive(false);
    }
}
