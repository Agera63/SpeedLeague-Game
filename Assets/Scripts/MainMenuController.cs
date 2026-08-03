using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private RawImage[] imageEcranMenu;
    private RawImage imageMontrer;
    private float tempsAvantProchaineImage;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject carsMenu;
    [SerializeField] private GameObject tracksMenu;
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject signUpMenu;

    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text errorMsg;

    [SerializeField] private GameObject loginBtn;
    [SerializeField] private GameObject logoutBtn;

    [SerializeField] private LevelLoader LLScript;

    async void Start()
    {
        foreach (RawImage ri in imageEcranMenu)
        {
            Color c = ri.color;
            ri.color = new Color(c.r, c.g, c.b, 0);
        }

        imageMontrer = imageEcranMenu[0];
        Color mc = imageMontrer.color;
        imageMontrer.color = new Color(mc.r, mc.g, mc.b, 255);

        tempsAvantProchaineImage = 10f;

        if (await APIMananger.instance.Validate())
        {
            loginBtn.SetActive(false);
            logoutBtn.SetActive(true);
        }
    }

    void Update()
    {
        ImageMouvement();
    }

    /// <summary>
    /// Updates the background image between 3 images.
    /// </summary>
    void ImageMouvement()
    {
        //if the counter is at 0, we change the image showing
        if (tempsAvantProchaineImage < 0)
        {
            int numSelectionner;
            do
            {
                numSelectionner = UnityEngine.Random.Range(0, imageEcranMenu.Length);
            }
            while (imageMontrer == imageEcranMenu[numSelectionner]);

            if (imageMontrer != null)
            {
                //removing the image opacity if there was 1 before
                imageMontrer.color = new Color(
                    imageMontrer.color.r,
                    imageMontrer.color.g,
                    imageMontrer.color.b,
                    0);
            }

            //Store the new image that will be shown
            imageMontrer = imageEcranMenu[numSelectionner];

            //max out opacity
            imageMontrer.color = new Color(
                    imageMontrer.color.r,
                    imageMontrer.color.g,
                    imageMontrer.color.b,
                    255);

            //set time to change images back at 10seconds
            tempsAvantProchaineImage = 10f;
        }
        else
        {
            //removes some time from the timer
            tempsAvantProchaineImage -= Time.deltaTime;
        }
    }

    //-------Main Menu Methods----------------

    /// <summary>
    /// Allows the user to play once the "Play" 
    /// button was clicked.
    /// </summary>
    public async void Play()
    {
        //Make sure user is loged in
        bool validLogin = await APIMananger.instance.Validate();

        if (PlayerPrefs.GetString("token") != "" && validLogin)
        {
            //Change scene
            LLScript.LoadNextScene(SettingsManager.instance.selectedTrack);
        } else
        {
            LogInMenu();
            errorMsg.text = "You where automatically disconnected, login again!";
        }
        
    }

    /// <summary>
    /// Redirects the user to the car menu where they can 
    /// select the car they wish to drive.
    /// </summary>
    public void CarsMenu()
    {
        carsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }


    /// <summary>
    /// Redirects the user to the track menu where they can 
    /// select the track they wish to drive on.
    /// </summary>
    public void TracksMenu()
    {
        tracksMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    /// <summary>
    /// Redirects the user to the login menu
    /// </summary>
    public void LogInMenu()
    {
        loginMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("token"); // clear the stale token
        logoutBtn.SetActive(false);
        loginBtn.SetActive(true);
        LogInMenu();
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //-------Cars Menu Methods----------------

    /// <summary>
    /// Once the Supra was selected, this is called
    /// </summary>
    public void SupraSelection()
    {
        SettingsManager.instance.SetSelectedCar("Toyota");
        carsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the BMW was selected, this is called
    /// </summary>
    public void BMWSelection()
    {
        SettingsManager.instance.SetSelectedCar("BMW");
        carsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the Lexus was selected, this is called
    /// </summary>
    public void LexusSelection()
    {
        SettingsManager.instance.SetSelectedCar("Lexus");
        carsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //-------Tracks Menu Methods----------------

    /// <summary>
    /// Once the Spa was selected, this is called
    /// </summary>
    public void SpaSelection()
    {
        SettingsManager.instance.SetSelectedTrack("Spa");
        tracksMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the Suzuka was selected, this is called
    /// </summary>
    public void SuzukaSelection()
    {
        SettingsManager.instance.SetSelectedTrack("Suzuka");
        tracksMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the Shanghai was selected, this is called
    /// </summary>
    public void ShanghaiSelection()
    {
        SettingsManager.instance.SetSelectedTrack("Shanghai");
        tracksMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //-------Login Menu Methods----------------

    public async void Login()
    {
        errorMsg.text = "Loading...";
        bool validLogin = await APIMananger.instance.Login(username.text, password.text);

        if(validLogin)
        {
            loginBtn.SetActive(false);
            logoutBtn.SetActive(true);
            LoginBack();
        } else
        {
            errorMsg.text = "Incorrect information! Please try again.";
        }
    }

    public void SignupBtn()
    {
        errorMsg.text = "Please sign up within your browser!";
        Application.OpenURL("https://speed-league.vercel.app/signup");
    }

    public void LoginBack()
    {
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
