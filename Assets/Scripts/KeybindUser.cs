using System.Collections.Generic;
using UnityEngine;

public abstract class KeybindUser : MonoBehaviour
{
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

    protected abstract List<KeybindManager.Keybind> GetAllKeybinds();
}
