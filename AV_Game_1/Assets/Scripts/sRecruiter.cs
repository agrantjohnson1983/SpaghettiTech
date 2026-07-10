using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sRecruiter : MonoBehaviour
{
    public static bool isRecruiting = false;
    public canvasGameplay canvasGameplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRecruiting)
        {
            isRecruiting = true;
            canvasGameplay.ToggleHireScreen();
        }
    }
/*
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasUI.SetActive(false);
        }
    }*/
}
