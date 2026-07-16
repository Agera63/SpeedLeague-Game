using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    public event Action<GameObject> onCheckpointEnter;

    void Awake()
    {
        //This number will be overrided in TimeTrialMode class
        checkpointNumber = 0;
    }

    /// <summary>
    /// Once the a vehicle makes contact with a checkpoint, it gets registered.
    /// </summary>
    public void VehicleContact()
    {
        onCheckpointEnter?.Invoke(gameObject);
    }
}
