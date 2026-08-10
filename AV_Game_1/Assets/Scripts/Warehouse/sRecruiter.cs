using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sRecruiter : MonoBehaviour
{
    public static bool isRecruiting = false;
    canvasGameplay canvasGameplay;
    CanvasWarehouse canvasWarehouse;

    private void Start()
    {
        canvasGameplay = GameManager.gm.canvasGameplay;
        canvasWarehouse = GameManager.gm.canvasWarehouse;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRecruiting)
        {
            Debug.Log("Toggling hiring screen on");

            isRecruiting = true;
            //canvasGameplay.ToggleHireScreen();
            canvasWarehouse.ToggleHireScreen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isRecruiting)
        {
            Debug.Log("Toggling hiring screen off");

            isRecruiting = false;
            canvasWarehouse.ToggleHireScreen();
        }
    }
}
