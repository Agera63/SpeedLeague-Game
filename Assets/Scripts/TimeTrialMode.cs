using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeTrialMode : MonoBehaviour
{
    [SerializeField] private TMP_Text timerUiText;
    private float timer;
    private bool isTimerCounting;

    //the checkpoints need to be in SUBSEQUENT ORDER 
    private GameObject[] checkpoints;
    private int checkpointCounter;

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

        //Always starts false to let the player gain momentum before the first checkpoint.
        isTimerCounting = false;
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

        if (checkpointCounter == checkpoints.Length && checkpointScript.checkpointNumber == 0)
        {
            //For later when you need to save the time 
            float floatTime = timer;
            string strTime = timerUiText.text;
            Debug.Log("Lap time : " +  floatTime + " seconds | " + strTime);

            //Add animation showing time

            LapReset();
        }
        //if checkpoint 1 has been hit, move on to the next one
        else if (checkpointScript.checkpointNumber == checkpointCounter)
        {
            checkpointCounter++;
            Debug.Log(checkpointCounter + " / " + checkpoints.Length);
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
        checkpointCounter = 0;
        timer = 0;
        timerUiText.text = "00:00:00.000";
    }
}
