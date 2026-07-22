using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    void Start()
    {
        if (SettingsManager.instance.selectedTrack.Equals("Spa"))
        {
            Instantiate(SettingsManager.instance.selectedCar,
                    SpawnInfo.Spa.position,
                    SpawnInfo.Spa.rotation);
        } else if (SettingsManager.instance.selectedTrack.Equals("Suzuka"))
        {
            Instantiate(SettingsManager.instance.selectedCar,
                    SpawnInfo.Suzuka.position,
                    SpawnInfo.Suzuka.rotation);
        } else if (SettingsManager.instance.selectedTrack.Equals("Suzuka"))
        {
            Instantiate(SettingsManager.instance.selectedCar,
                    SpawnInfo.Shandghai.position,
                    SpawnInfo.Shandghai.rotation);
        }
    }
}
