using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenManager : MonoBehaviour
{
    public UIScreen[] screens;

    UIScreen currentScreen;

    private void Awake()
    {
        foreach (UIScreen screen in screens)
            SetGroupActive(screen.group, false);
       
        currentScreen = screens[0];
        SetGroupActive(currentScreen.group, true);
    }
    public void SetScreen(string name)
    {
        foreach (UIScreen screen in screens)
        {
            if (screen.name == name)
            {
                UpdateScreen(screen);
                return;
            }
        }
        Debug.LogWarning("Couldn't find screen called: " + name);
    }

    private void UpdateScreen(UIScreen screen)
    {
        SetGroupActive(currentScreen.group, false);
        currentScreen = screen;
        SetGroupActive(currentScreen.group, true);
    }

    private void SetGroupActive(CanvasGroup group, bool active)
    {
        group.alpha = active ? 1 : 0;
        group.interactable = active;
        group.blocksRaycasts = active;
    }

    [System.Serializable]
    public struct UIScreen
    {
        public string name;
        public CanvasGroup group;
    }
}
