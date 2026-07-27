using System;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenuScript;
    void Start()
    {
        //Checks if there is currently a player in the pause menu
        if (pauseMenuScript.player != null) return;

        if (SettingsManager.instance.selectedTrack.Equals("Spa"))
        {
            pauseMenuScript.player = Instantiate(SettingsManager.instance.selectedCar,
                    SpawnInfo.Spa.position,
                    SpawnInfo.Spa.rotation);
        } else if (SettingsManager.instance.selectedTrack.Equals("Suzuka"))
        {
            pauseMenuScript.player = Instantiate(SettingsManager.instance.selectedCar,
                    SpawnInfo.Suzuka.position,
                    SpawnInfo.Suzuka.rotation);
        } else if (SettingsManager.instance.selectedTrack.Equals("Shanghai"))
        {
            pauseMenuScript.player = Instantiate(SettingsManager.instance.selectedCar,
                    SpawnInfo.Shanghai.position,
                    SpawnInfo.Shanghai.rotation);
        }
    }
}
