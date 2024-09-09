using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMotorController : sRigGear
{
    GameObject canvasMotorController;

    bool isShowingControls = false;

    //bool isSetup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerGrabAction()
    {
        //base.TriggerGrabAction();

        
    }

    public override void StopGrabAction()
    {
        //base.StopGrabAction();

 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsSet && other.gameObject.CompareTag("Player"))
            eventsUI.ToggleMotorControlDisplay(true);
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (IsSet && other.gameObject.CompareTag("Player"))
            eventsUI.ToggleMotorControlDisplay(false);
    }

}
