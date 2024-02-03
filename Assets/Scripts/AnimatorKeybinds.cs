using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorKeybinds : KeybindUser
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<AnimationBindingData> _animationBindings;

    protected override List<KeybindManager.Keybind> GetAllKeybinds()
    {
        List<KeybindManager.Keybind> keybinds = new();

        foreach (var animationBinding in _animationBindings)
        {
            keybinds.Add(new KeybindManager.Keybind(
                () => _animator.Play(animationBinding.animationName),
                animationBinding.binding
                ));
        }

        return keybinds;
    }
    
    private void Update()
    {
        foreach (var animationBinding in _animationBindings)
        {
            if (!Helper.IsHotkeyBeingInputted(animationBinding.binding))
                continue;
            
            _animator.Play(animationBinding.animationName);
            return;
        }
    }

    private void Reset()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    [Serializable]
    public struct AnimationBindingData
    {
        public string animationName;
        public List<KeyCode> binding;
    }
}
