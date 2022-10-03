using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public bool MainMenu = false;
    public UnityEngine.UI.Slider SFXSlider, MusicSlider, SensitivitySlider;
    public UnityEngine.UI.Toggle FullscreenBox;
    public UnityEngine.UI.Dropdown ResolutionDropdown;
    public UIManager UIManager_;

    private int[,] Resolutions = new int[,] {
        {1280,720},
        {1920,1080},
        {2560,1440}};

    public bool Fullscreen;
    public int Resolution;
    public float SFX, Music, Sensitivity;
    private CameraRotationControl cameraRotationControl;
    void Start()
    {
        cameraRotationControl = FindObjectOfType<CameraRotationControl>();
        LoadSettings();
        UpdateSettings();
        if (!MainMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        UIManager_ = FindObjectOfType<UIManager>();
    }

    private void LoadSettings()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        SensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 0.8f);
        FullscreenBox.isOn = PlayerPrefs.GetInt("Fullscreen", 1) > 0;
        if (!PlayerPrefs.HasKey("Resolution2"))
        {
            // Find resolution
            Resolution res = Screen.currentResolution;
            for (int i = 0; i < Resolutions.Length; i++)
            {
                if (Resolutions[i, 0] == res.width && Resolutions[i, 1] == res.height)
                {
                    ResolutionDropdown.value = i;
                    return;
                }
            }
            // No resolution
            ResolutionDropdown.value = 0;
            Fullscreen = false;
        }
        else
        {
            ResolutionDropdown.value = PlayerPrefs.GetInt("Resolution2");
        }
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
        SoundController.Volume = SFX;
        CrossfadeMusicPlayer.Instance.Volume = Music;
        if (cameraRotationControl != null)
        {
            cameraRotationControl.Sensitivity = Sensitivity * 2;
        }

        // Save
        PlayerPrefs.SetFloat("SFXVolume", SFX);
        PlayerPrefs.SetFloat("MusicVolume", Music);
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
        PlayerPrefs.SetInt("Fullscreen", Fullscreen ? 1 : 0);
        PlayerPrefs.SetInt("Resolution1", Resolution);
    }

    private void Update()
    {
        if (!MainMenu && Input.GetKeyDown(KeyCode.Escape) && ScoreManager.Current.Playing)
        {
            ChangeVisibilityOfSettingsMenu();
        }
    }

    public void ChangeVisibilityOfSettingsMenu()
    {
        if(UIManager_.IsAnyWindowOpen())
        {
            Conductor.Resume();
            Time.timeScale = 1;
            UIManager_.CloseWindows();
        }
        else
        {
            Conductor.Pause();
            Time.timeScale = 0;
            UIManager_.OpenWindow(UIManager.UIElements.Settings);
            LoadSettings();
        }
   }
}
