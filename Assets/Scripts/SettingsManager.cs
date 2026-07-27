using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; private set; }

    public GameObject selectedCar;
    public string selectedTrack;

    [SerializeField] private GameObject Supra;
    [SerializeField] private GameObject BMW;
    [SerializeField] private GameObject Lexus;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Only set defaults the very first time this is created
        if (selectedCar == null)
            selectedCar = Supra;
        if (string.IsNullOrEmpty(selectedTrack))
            selectedTrack = "Spa";
    }

    public void SetSelectedCar(string newCar)
    {
        switch (newCar)
        {
            case "Supra":
                selectedCar = Supra;
                break;
            case "BMW":
                selectedCar = BMW;
                break;
            case "Lexus":
                selectedCar = Lexus;
                break;
        }
    }

    public void SetSelectedTrack(string newTrack)
    {
        selectedTrack = newTrack;
    }
}