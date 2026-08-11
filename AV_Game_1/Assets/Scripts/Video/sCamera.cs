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
    public float TaskTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public eToolType ToolTypeNeeded { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        cam = GetComponentInChildren<Camera>();

        cameraFPS.SetActive(false);
    }

    public void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //base.TriggerAction(_actionObj, _toolToUse);

        sPlayerCharacter.playerCharacterGlobal.ToggleCameraMain(false);

        // maybe set this to the model to full follow rotation
        transform.rotation = sPlayerCharacter.playerCharacterGlobal.transform.rotation;

        cameraFPS.SetActive(true);
    }

    public void StopAction()
    {
        //base.StopAction(_actionObj);

        sPlayerCharacter.playerCharacterGlobal.ToggleCameraMain(true);

        cameraFPS.SetActive(false);
    }
}
