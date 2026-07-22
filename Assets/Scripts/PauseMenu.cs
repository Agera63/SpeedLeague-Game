using MVC;
using MVC.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider volumeSlider;
    private ToolkitSettings settings;

    void Start()
    {
        settings = ToolkitSettings.LoadData();

        if (settings == null)
        {
            Debug.LogError("Could not load MVC ToolkitSettings.");
            return;
        }

        // Restore saved volume (defaults to whatever's currently on the asset if no save exists)
        settings.SFXVolume = PlayerPrefs.GetFloat("MVC_SFXVolume", settings.SFXVolume);

        // Sync the slider to the restored value WITHOUT firing the listener
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.SetValueWithoutNotify(settings.SFXVolume);

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            TogglePause();
        }
    }

    private void OnVolumeChanged(float value)
    {
        settings.SFXVolume = value;
        PlayerPrefs.SetFloat("MVC_SFXVolume", value);
    }

    private void TogglePause()
    {
        bool isPaused = pauseMenu.activeSelf;
        pauseMenu.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1 : 0;
    }

    //-------Menu Methods----------------

    public void ResumeOption()
    {
        TogglePause();
    }

    public void ResetOption()
    {
        
    }

    public void ExitOption()
    {
        SceneManager.LoadScene("StartingMenu");
    }
}