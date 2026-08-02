using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; private set; }

    public GameObject selectedCar;
    public string selectedCarString;
    public string selectedTrack;

    [SerializeField] private GameObject Toyota;
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
        if(selectedCar == null)
            selectedCar = Toyota;
        if (string.IsNullOrEmpty(selectedTrack))
            selectedTrack = "Spa";
    }

    public void SetSelectedCar(string newCar)
    {
        selectedCarString = newCar;
        switch (newCar)
        {
            case "Toyota":
                selectedCar = Toyota;
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