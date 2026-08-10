using UnityEngine;

public class sTravelDestination : MonoBehaviour
{
    public sTravelManager travelManager;


private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Truck"))
            return;

        Debug.Log("TRUCK ARRIVED AT DESTINATION");

        travelManager.ArriveAtGig();
    }
}
