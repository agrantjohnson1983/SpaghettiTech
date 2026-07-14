using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasWarehouse : MonoBehaviour
{
    public GameObject startScreen;

    bool isHiring = false;

    public GameObject hiringPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        hiringPanel.SetActive(isHiring);

        //GameManager.gm.ToggleOrbitCamera(isHiring);
    }
}
