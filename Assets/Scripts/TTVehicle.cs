using UnityEngine;

public class TTVehicule : MonoBehaviour
{
    private GameObject hitCheckpoint;

    void OnTriggerEnter(Collider other)
    {
        //Once a checkpoint has been hit, we register it
        if (other.TryGetComponent(out Checkpoint cp) && (hitCheckpoint == null || hitCheckpoint.GetComponent<Checkpoint>() != cp))
        {
            //Stores the hit check point to prevent lag
            hitCheckpoint = other.gameObject;
            cp.VehicleContact();

        }
    }
}
