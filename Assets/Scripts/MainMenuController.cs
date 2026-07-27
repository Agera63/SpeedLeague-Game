using System;
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

    public event Action<string> carSelection;
    public event Action<string> trackSelection;

    void Start()
    {
        foreach (RawImage ri in imageEcranMenu)
        {
            ri.color = new Color(
                    imageMontrer.color.r,
                    imageMontrer.color.g,
                    imageMontrer.color.b,
                    0);
        }

        imageMontrer = imageEcranMenu[0];
        imageMontrer.color = new Color(
                    imageMontrer.color.r,
                    imageMontrer.color.g,
                    imageMontrer.color.b,
                    255);

        tempsAvantProchaineImage = 10f;
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
    public void Play()
    {
        //Make sure user is loged in


        //Change scene
        SceneManager.LoadScene(SettingsManager.instance.selectedTrack);
    }

    /// <summary>
    /// Redirects the user to the car menu where they can 
    /// select the car they wish to drive.
    /// </summary>
    public void Cars()
    {
        carsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }


    /// <summary>
    /// Redirects the user to the track menu where they can 
    /// select the track they wish to drive on.
    /// </summary>
    public void Tracks()
    {
        tracksMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    /// <summary>
    /// Redirects the user to the login menu
    /// </summary>
    public void LogIn()
    {
        loginMenu.SetActive(true);
        mainMenu.SetActive(false);
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
        carSelection?.Invoke("Supra");
        carsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the BMW was selected, this is called
    /// </summary>
    public void BMWSelection()
    {
        carSelection?.Invoke("BMW");
        carsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the Lexus was selected, this is called
    /// </summary>
    public void LexusSelection()
    {
        carSelection?.Invoke("Lexus");
        carsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //-------Tracks Menu Methods----------------

    /// <summary>
    /// Once the Spa was selected, this is called
    /// </summary>
    public void SpaSelection()
    {
        trackSelection?.Invoke("Spa");
        tracksMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the Suzuka was selected, this is called
    /// </summary>
    public void SuzukaSelection()
    {
        trackSelection?.Invoke("Suzuka");
        tracksMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Once the Shanghai was selected, this is called
    /// </summary>
    public void ShanghaiSelection()
    {
        trackSelection?.Invoke("Shanghai");
        tracksMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //-------Login Menu Methods----------------
    
    public void LoginBack()
    {
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //-------Sign Up Menu Methods----------------

}
