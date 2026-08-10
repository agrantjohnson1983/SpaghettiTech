using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class sComputer : MonoBehaviour
{
    public static sComputer computerGlobal;

    //public GameObject gigSelectScreen;

    bool isOnSelectScreen = false;

    public TMP_Text[] textCurrentGig;

    public string noGigText = "UNEMPLOYED - GET A GIG ASSHOLE!";

    private void Awake()
    {
        if (computerGlobal == null)
            computerGlobal = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        SetText(noGigText);
    }

    public void SetText(string _text)
    {
        foreach(TMP_Text t in textCurrentGig)
        {
            if(t != null)
                t.text = _text;
        }

        GameManager.gm.canvasWarehouse.SetGigText(_text);
    }

    void ToggleSelectScreen(bool _isOn)
    {
        GameManager.gm.canvasWarehouse.ToggleGigSelectScreen(_isOn);
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
