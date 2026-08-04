using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class sTruckHatch : MonoBehaviour
{
    public GameObject canvasUI;

    public Animator animator;

    public float _taskTime;

    public bool hasAction = false; // use this for turning on objects that have actions

    public Vector3 offset;

    bool _canTriggerAction; // this is for controls to check if an action can be triggered

    //bool hasBeenSet = false;

    //public eTypeRigSetup rigType;

    public eToolType _toolTypeNeeded;

    //public TextMeshPro textSetup;

    public Vector3 _UI_offset;

    public GameObject textTruckCloseHatch;

    public GameObject truckHatchPhysical;

    public Vector3 ui_offset
    {
        get
        {
            return _UI_offset;
        }

        set
        {
            _UI_offset = value;
        }
    }

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

    public float TaskTime
    {
        get
        {
            return _taskTime;
        }

        set
        {
            _taskTime = value;
        }
    }

    public bool CanTriggerAction
    {
        get
        {
            return _canTriggerAction;
        }

        set
        {
            _canTriggerAction = value;
        }
    }

    public bool IsDoingAction
    {
        get;
        set;
    }

    public eToolType ToolTypeNeeded
    {
        get
        {
            return _toolTypeNeeded;
        }

        set
        {
            _toolTypeNeeded = value;
        }
    }

    public SO_EventsUI soUI;

    public void StopAction()
    {
        //throw new System.NotImplementedException();
    }

    public void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sTruck.isLoaded || !sGigManager.gigManagerGlobal.CheckIfHasAGig())
            return;

        if (other.CompareTag("Player"))
        {
            canvasUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sTruck.isLoaded || !sGigManager.gigManagerGlobal.CheckIfHasAGig())
            return;

        if (other.CompareTag("Player"))
        {
            canvasUI.SetActive(false);
        }
    }

    public void OnLetsGo()
    {
        GetComponent<MeshRenderer>().enabled = true;

        animator.SetTrigger("Close");

        sTruck.isLoaded = true;

        textTruckCloseHatch.SetActive(false);

        truckHatchPhysical.SetActive(false);

        Destroy(canvasUI.gameObject);

        // truck drives off and then scene changes...
    }

    public void OnHoldUp()
    {
        canvasUI.SetActive(false);
    }

}
