using NUnit.Framework.Constraints;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; private set; } = new();

    public GameObject selectedCar;
    public string selectedTrack;

    [SerializeField] private MainMenuController controller;

    [SerializeField] private GameObject Supra;
    [SerializeField] private GameObject BMW;
    [SerializeField] private GameObject Lexus;

    void Awake()
    {
        //Makes sure the settings are kept while the application is open
        DontDestroyOnLoad(gameObject);

        controller.carSelection += onCarChange;
        controller.trackSelection += onTrackChange;

        //Default values are Supra and Spa track;
        instance.selectedCar = Supra;
        instance.selectedTrack = "Spa";
    }

    private void onCarChange(string newCar)
    {
        switch (newCar)
        {
            case "Supra": 
                instance.selectedCar = Supra;
                break;
            case "BMW" :
                instance.selectedCar = BMW;
                break;
            case "Lexus": 
                instance.selectedCar = Lexus;
                break;
        }
    }

    private void onTrackChange(string newTrack)
    {
        instance.selectedTrack = newTrack;
    }
}
