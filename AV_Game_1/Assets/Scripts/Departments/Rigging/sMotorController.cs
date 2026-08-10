using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMotorController : sRigGear
{
    GameObject canvasMotorController;

    //bool isShowingControls = false;

    //bool isSetup;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
            soUI.ToggleMotorControlDisplay(true);
    }


    private void OnTriggerExit(Collider other)
    {
        if (IsSet && other.gameObject.CompareTag("Player"))
            soUI.ToggleMotorControlDisplay(false);
    }

}
