using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] UIWindows;
    public GameObject GameInterface;
    public enum UIElements {Settings, NextLevel, GameOver, LevelMenu}
    public bool IsAnyWindowOpen()
    {
        bool windowActive = false;
        foreach (var x in UIWindows)
            windowActive = windowActive || x.activeInHierarchy;
        return windowActive;
    }

    public void OpenWindow(UIElements window)
    {
        GameInterface.SetActive(false);
        UIWindows[(int)window].SetActive(true);
        Cursor.visible = true;
    }

    public void CloseWindows()
    {
        Cursor.visible = false;
        GameInterface.SetActive(true);
        foreach (var x in UIWindows)
            x.SetActive(false);
    }

    public bool IsWindowOpen(UIElements window)
    {
        return UIWindows[(int)window].activeInHierarchy;
    }


}
