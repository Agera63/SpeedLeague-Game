using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    public event Action<GameObject> onCheckpointEnter;

    /// <summary>
    /// Once the a vehicle makes contact with a checkpoint, it gets registered.
    /// </summary>
    public void VehicleContact()
    {
        onCheckpointEnter?.Invoke(gameObject);
    }
}
