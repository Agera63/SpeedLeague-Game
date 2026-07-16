using UnityEngine;

public class TTVehicule : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Once a checkpoint has been hit, we register it
        if (other.TryGetComponent(out Checkpoint cp))
        {
            cp.VehicleContact();
        }
    }
}
