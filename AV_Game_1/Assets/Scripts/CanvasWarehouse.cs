using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasWarehouse : MonoBehaviour
{
    public GameObject startScreen;

    bool isHiring = false;

    public GameObject hiringPanel;

    public SO_EventsUI soUI;

    public TextMeshProUGUI textMoney;

    float money;

    private void Awake()
    {
        //if (GameManager.gm.canvasGameplay.gameObject != this.gameObject)
        //    Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        soUI.characterHire.AddListener(OnCharacterHire);
    }

   
    private void OnDisable()
    {
        soUI.characterHire.RemoveListener(OnCharacterHire);
    }

    // Start is called before the first frame update
    void Start()
    {
        hiringPanel.SetActive(false);
    }

    private void OnCharacterHire(float _amount)
    {

    }

    public void OnStartButton()
    {
        startScreen.SetActive(false);
        GameManager.gm.ToggleOrbitCamera(false);
    }

    public void ToggleHireScreen()
    {
        isHiring = !isHiring;

        //hiringButton.SetActive(!isHiring);

        Debug.Log("Setting hiring panel to : " + isHiring);

        if(isHiring)
            hiringPanel.SetActive(isHiring);

        else
            hiringPanel.GetComponent<sHiringScreen>().Close();

        //GameManager.gm.ToggleOrbitCamera(isHiring);
    }
}
