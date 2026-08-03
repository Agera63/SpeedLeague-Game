using MVC;
using MVC.Core;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TimeTrialMode timeTrialScript;

    [SerializeField] private LevelLoader LLScript;

    private ToolkitSettings settings;
    public GameObject player;

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
        //assures that the cars momentum is not kept and gets reset too
        var carRB = player.GetComponent<Rigidbody>();
        carRB.linearVelocity = Vector3.zero;
        carRB.angularVelocity = Vector3.zero;

        Vector3 spawnPos;
        Quaternion spawnRot;

        //Checks for the track that the player is playing and respawns the car
        switch (SettingsManager.instance.selectedTrack)
        {
            case "Spa":
                spawnPos = SpawnInfo.Spa.position;
                spawnRot = SpawnInfo.Spa.rotation;
                break;
            case "Suzuka":
                spawnPos = SpawnInfo.Suzuka.position;
                spawnRot = SpawnInfo.Suzuka.rotation;
                break;
            case "Shanghai":
                spawnPos = SpawnInfo.Shanghai.position;
                spawnRot = SpawnInfo.Shanghai.rotation;
                break;
            default:
                spawnPos = player.transform.position;
                spawnRot = player.transform.rotation;
                break;
        }

        // Set BOTH transform and rigidbody, then force physics to acknowledge it now
        player.transform.SetPositionAndRotation(spawnPos, spawnRot);
        carRB.position = spawnPos;
        carRB.rotation = spawnRot;

        //forces physics engine to update immediately, instead of waiting for next FixedUpdate
        Physics.SyncTransforms();

        //Timer reset
        timeTrialScript.TimerReset();

        //Close menu
        TogglePause();
    }

    public void ExitOption()
    {
        Time.timeScale = 1;
        TogglePause();
        LLScript.LoadNextScene("StartingMenu");
    }
}