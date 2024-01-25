using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helper
{
    public static bool IsHotkeyBeingInputted(IEnumerable<KeyCode> keycodes)
    {
        // if there aren't any elements then no
        if (!keycodes.Any())
            return false;

        bool oneDownThisFrame = false;
        
        foreach (var keycode in keycodes)
        {
            if (!Input.GetKey(keycode))
                return false;
            
            if (Input.GetKeyDown(keycode))
                oneDownThisFrame = true;
        }

        // at least one should be inputted this frame so it isn't true while being held
        return oneDownThisFrame;
    }
}
