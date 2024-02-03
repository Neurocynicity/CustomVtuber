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

    private KeybindManager keybindManager;
    
    private void OnEnable()
    {
        keybindManager = KeybindManager.Instance;
        
        foreach (var keybind in _keybinds)
            keybindManager.AddKeybind(keybind);
    }

    private void OnDisable()
    {
        foreach (var keybind in _keybinds)
            keybindManager.RemoveKeybind(keybind);
    }

    private List<KeybindManager.Keybind> _cachedKeybinds;

    private List<KeybindManager.Keybind> _keybinds
    {
        get
        {
            if (_cachedKeybinds != null)
                return _cachedKeybinds;

            _cachedKeybinds = GetAllKeybinds();
            return _cachedKeybinds;
        }
    }

    private List<KeybindManager.Keybind> GetAllKeybinds()
    {
        List<KeybindManager.Keybind> keybinds = new();

        foreach (var asciiFaceMoodData in MoodData)
        {
            keybinds.Add(new KeybindManager.Keybind(
                () => UpdateMood(asciiFaceMoodData),
                asciiFaceMoodData.MoodKeyCodes
            ));
        }
        
        return keybinds;
    }

    private void UpdateMood(AsciiFaceMoodData data)
    {
        CurrentFaceData = data;
        OnMoodChanged?.Invoke(CurrentFaceData);
    }
}