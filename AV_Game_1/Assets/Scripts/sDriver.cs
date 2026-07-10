using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sDriver : MonoBehaviour
{
    public GameObject canvasUI;

    public sTruck truck;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        canvasUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && sTruck.isLoaded)
        {
            player = other.gameObject;
            canvasUI.SetActive(true);
        }

        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasUI.SetActive(false);
        }
    }

    public void OnLetsGo()
    {
        Debug.Log("Truck is driving");

        canvasUI.SetActive(false);

        truck.Drive();

        Camera.main.gameObject.transform.SetParent(null);

        Destroy(player);
        //GameManager.gm.KillPlayers();

        // TO DO - Add text/feedback for fail
        Debug.Log("Hatch needs to be closed to load!");
    }

    public void OnHoldUp()
    {
        canvasUI.SetActive(false);
    }
}
