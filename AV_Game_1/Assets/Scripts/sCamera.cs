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

    public void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //base.TriggerAction(_actionObj, _toolToUse);

        GameManager.gm.ReturnCurrentPlayer().ToggleCameraMain(false);

        // maybe set this to the model to full follow rotation
        transform.rotation = GameManager.gm.ReturnCurrentPlayer().transform.rotation;

        cameraFPS.SetActive(true);
    }

    public void StopAction()
    {
        //base.StopAction(_actionObj);

        GameManager.gm.ReturnCurrentPlayer().ToggleCameraMain(true);

        cameraFPS.SetActive(false);
    }
}
