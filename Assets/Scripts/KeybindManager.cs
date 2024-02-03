using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a class that should handle all keybinds, so I can put them all in one place
/// </summary>
public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private List<Keybind> _keybinds = new();

    private void Update()
    {
        for (int i = 0; i < _keybinds.Count; i++)
        {
            if (!Helper.IsHotkeyBeingInputted(_keybinds[i].KeyCodes))
                continue;
            
            _keybinds[i].OnPressed?.Invoke();
        }
    }

    public void AddKeybind(Keybind keybind) =>
        _keybinds.Add(keybind);
    
    public void RemoveKeybind(Keybind keybind) =>
        _keybinds.Remove(keybind);

    [Serializable]
    public struct Keybind
    {
        public KeyCode[] KeyCodes;
        public Action OnPressed;

        public Keybind(Action onPressed, IEnumerable<KeyCode> keyCodes)
        {
            OnPressed = onPressed;
            KeyCodes = keyCodes.ToArray();
        }
        
        public Keybind(Action onPressed, params KeyCode[] keyCodes)
        {
            OnPressed = onPressed;
            KeyCodes = keyCodes;
        }
    }
}
