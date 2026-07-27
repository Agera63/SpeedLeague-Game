using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeTrialMode : MonoBehaviour
{
    [SerializeField] private TMP_Text timerUiText;
    private float timer;
    private bool isTimerCounting;
    private bool showingLapTime;

    //the checkpoints need to be in SUBSEQUENT ORDER 
    private GameObject[] checkpoints;
    private int checkpointCounter;

    [SerializeField] private Material whiteTransparentColor;
    [SerializeField] private Material blueTransparentColor;

    void Awake()
    {
        checkpoints= new GameObject[gameObject.transform.childCount];
        for (int counter = 0; counter < gameObject.transform.childCount; counter++) 
        {
            GameObject checkpointGO = gameObject.transform.GetChild(counter).gameObject;
            checkpoints[counter] = checkpointGO;

            Checkpoint checkpointScript = checkpointGO.GetComponent<Checkpoint>();
            checkpointScript.onCheckpointEnter += RegisterCheckpoint;
            //Allows an order to be established based on the order given in this gameobject (CheckpointManager)
            checkpointScript.checkpointNumber = counter;
        }

        //Makes the first checkpoint blue ish
        checkpoints[checkpointCounter].GetComponent<Renderer>().material = blueTransparentColor;

        //Always starts false to let the player gain momentum before the first checkpoint.
        isTimerCounting = false;
        showingLapTime = false;
        timer = 0;
        checkpointCounter = 0;
    }

    void Update()
    {
        UpdateTimer();
    }

    /// <summary>
    /// Updates the timer on the UI and in the code.
    /// </summary>
    void UpdateTimer()
    {
        if (isTimerCounting) timer += Time.deltaTime; 

        //converts the amount of seconds from timer to actual hours/minutes/seconds
        int hours = (int)timer / 3600;
        int minutes = (int)(timer - 3600 * hours) / 60;
        float seconds = timer - (3600 * hours) - (60 * minutes);

        //we make sure single digit numbers like "9" have a "09" in the timer.
        string strHours = hours < 10 ? "0" + hours.ToString() : hours.ToString();
        string strMinutes = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        //F3 makes the float have 3 digits after. So 1.234s
        string strSeconds = seconds < 10 ? "0" + seconds.ToString("F3") : seconds.ToString("F3");

        if (!showingLapTime)
            //updates the ui text to be the following -> 00:09:13.756
            timerUiText.text = strHours + ":" + strMinutes + ":" + strSeconds;
    }

    /// <summary>
    /// Once a checkpoint makes contact with the player, this method acknoledged that 
    /// </summary>
    /// <param name="cp">The checkpoint gameobject</param>
    void RegisterCheckpoint(GameObject cp)
    {
        var checkpointScript = cp.GetComponent<Checkpoint>();

        //If all checkpoints where hit in order AND the first checkpoint is hit again
        if (checkpointCounter == checkpoints.Length && checkpointScript.checkpointNumber == 0)
        {
            //For later when you need to save the time 
            float floatTime = timer;
            string strTime = timerUiText.text;    
            Debug.Log("Lap time : " +  floatTime + " seconds | " + strTime);

            LapReset();
        }
        //if a checkpoint has been hit, move on to the next one
        else if (checkpointScript.checkpointNumber == checkpointCounter)
        {
            //Make previous checkpoint white again
            checkpoints[checkpointCounter].GetComponent<Renderer>().material = whiteTransparentColor;

            checkpointCounter++;
            Debug.Log(checkpointCounter + " / " + checkpoints.Length);

            //If all checkpoints where hit, make the first checkpoint blue again
            if(checkpointCounter == checkpoints.Length)
            {
                checkpoints[0].GetComponent<Renderer>().material = blueTransparentColor;
                return;
            }

            //Make the new checkpoint blue (ish)
            checkpoints[checkpointCounter].GetComponent<Renderer>().material = blueTransparentColor;
        }

        //If the first checkpoint is hit, start the timer.
        if (!isTimerCounting && checkpointCounter != 0)
            isTimerCounting = true;
    }

    /// <summary>
    /// Once the player succesfully drove a lap around the circuit, 
    /// this method reset the ui and the code.
    /// </summary>
    void LapReset()
    {
        //Starts clock animation
        StartCoroutine(LapCompletionAnimation());
        checkpointCounter = 0;
    }

    IEnumerator LapCompletionAnimation()
    {
        timer = 0;

        //Freezes the timer on the UI
        showingLapTime = true;
        string savedStringLapTime = timerUiText.text;

        float seconds = 0;
        while(seconds < 5)
        {
            //Blinking effect
            if (seconds < 4)
            {
                int intSeconds = (int)seconds;
                float decimals = seconds - intSeconds;
                if (decimals > 0.5f)
                {
                    timerUiText.text = savedStringLapTime;
                } else
                {
                    timerUiText.text = "";
                }
            } else
            {
                //last second just shows the time
                timerUiText.text = savedStringLapTime;
            }
            seconds += Time.deltaTime;
            yield return null;
        }

        //unfreezes the timer and continues with the new lap currently in progress
        showingLapTime = false;
    }

    /// <summary>
    /// Once a reset has been called in the pause menu,
    /// the timer gets reset
    /// </summary>
    public void TimerReset()
    {
        isTimerCounting = false;
        timer = 0;

        //Resets the checkpoint color
        checkpoints[checkpointCounter].GetComponent<Renderer>().material = whiteTransparentColor;
        checkpoints[0].GetComponent<Renderer>().material = blueTransparentColor;

        checkpointCounter = 0;
    }
}
