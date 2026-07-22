using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class sComputer : MonoBehaviour
{
    public GameObject gigSelectScreen;

    bool isOnSelectScreen = false;

    public TextMeshProUGUI textCurrentGig;

    private void Start()
    {
        textCurrentGig.text = "UNEMPLOYED - GET A GIG ASSHOLE!";
    }

    void ToggleSelectScreen(bool _isOn)
    {
        gigSelectScreen.SetActive(_isOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isOnSelectScreen)
        {
            isOnSelectScreen = true;

            ToggleSelectScreen(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOnSelectScreen)
        {
            isOnSelectScreen = false;

            ToggleSelectScreen(false);
        }
    }
}
