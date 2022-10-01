using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public UnityEngine.UI.Slider SFXSlider, MusicSlider, SensitivitySlider;
    public UnityEngine.UI.Toggle FullscreenBox;
    public UnityEngine.UI.Dropdown ResolutionDropdown;
    public GameObject SettingsMenu;

    private int[,] Resolutions = new int[,] {
        {1024, 768},
        {1280,720},
        {1440,900},
        {1920,1080},
        {1920,1200},
        {2048,1536},
        {2560,1440},
        {2560,1600},
        {2560,2048},
        {4096,2304},
        {8192,4608}};

    public bool Fullscreen, SettingsActive;
    public int Resolution;
    public float SFX, Music, Sensitivity;
    void Start()
    {
        UpdateSettings();
        SettingsActive = false;
        Cursor.visible = false;
    }

    public void UpdateSettings()
    {
        Fullscreen = FullscreenBox.isOn;
        Resolution = ResolutionDropdown.value;
        SFX = SFXSlider.value;
        Music = MusicSlider.value;
        Sensitivity = SensitivitySlider.value;

        Screen.fullScreen = Fullscreen;
        Screen.SetResolution(Resolutions[Resolution, 0], Resolutions[Resolution, 1], Fullscreen);
        Camera.main.gameObject.GetComponent<CameraRotationControl>().Sensitivity = Sensitivity * 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeVisibilityOfSettingsMenu();
        }
    }

    public void ChangeVisibilityOfSettingsMenu()
    {
        SettingsActive = !SettingsActive;
        Cursor.visible = SettingsActive;
        SettingsMenu.SetActive(SettingsActive);
    }
}
