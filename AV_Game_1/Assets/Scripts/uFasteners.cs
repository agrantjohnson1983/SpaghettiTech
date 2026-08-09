using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uFasteners : MonoBehaviour
{
    public static uFasteners instance;

    public SO_EventsUI soUI;

    public TMP_Text textBolt, textNut;

    public static int numberOfBolts = 0;
    public static int numberOfNuts = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        soUI.nutPickup.AddListener(UpdateNuts);
        soUI.boltPickup.AddListener(UpdateBolts);
    }

    private void OnDisable()
    {
        soUI.nutPickup.RemoveListener(UpdateNuts);
        soUI.boltPickup.RemoveListener(UpdateBolts);
    }

    private void Start()
    {
        textBolt.text = "x " + numberOfBolts;
        textNut.text = "x " + numberOfNuts;
    }

    void UpdateBolts()
    {
        numberOfBolts++;
        textBolt.text = "x " + numberOfBolts;
    }

    void UpdateNuts()
    {
        numberOfNuts++;
        textNut.text = "x " + numberOfNuts;
    }

    // Called by sSharedWrench once a hole finishes tightening, to
    // consume the bolt+nut pair that hole used out of the player's
    // collected count.
    public void RemoveBolt()
    {
        if (numberOfBolts <= 0)
        {
            return;
        }

        numberOfBolts--;
        textBolt.text = "x " + numberOfBolts;
    }

    public void RemoveNut()
    {
        if (numberOfNuts <= 0)
        {
            return;
        }

        numberOfNuts--;
        textNut.text = "x " + numberOfNuts;
    }
}