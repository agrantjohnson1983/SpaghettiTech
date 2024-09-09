using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCamera : sInteractive, iActionable
{
    public GameObject cameraFPS;

    Camera cam;

    bool hasAction = true;

    public bool HasAction
    {
        get
        {
            return hasAction;
        }
        set
        {
            hasAction = value;
        }
    }
    public bool CanTriggerAction { get; set; }
    public bool IsDoingAction
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        cameraFPS.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAction(GameObject _actionObj, eToolType _toolToUse)
    {
        //base.TriggerAction(_actionObj, _toolToUse);

        sPlayerCharacter.playerGlobal.ToggleCameraMain(false);

        // maybe set this to the model to full follow rotation
        transform.rotation = sPlayerCharacter.playerGlobal.transform.rotation;

        cameraFPS.SetActive(true);
    }

    public void StopAction(GameObject _actionObj)
    {
        //base.StopAction(_actionObj);

        sPlayerCharacter.playerGlobal.ToggleCameraMain(true);

        cameraFPS.SetActive(false);
    }
}
