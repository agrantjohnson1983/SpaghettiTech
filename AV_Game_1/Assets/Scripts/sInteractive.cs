using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sInteractive : MonoBehaviour, iGrabbable
{
    protected Rigidbody rb;

    public SO_EventsUI uiEvents;

    bool _isGrabbed = false;

    public float _taskTime;
    
    public eToolType _toolTypeNeeded;
    //public eTypeRigSetup typeOfRig;

    bool hasCorrectTool;

    bool isActive;

    public TextMeshPro textMPAbove;

    public TextMeshPro[] itemTextMP;

    public string textItem;

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

    public bool IsGrabbed
    {
        get
        {
            return _isGrabbed;
        }
        set
        {
            _isGrabbed = value;
        }
    }

    public Transform transformGrab
    {
        get
        {
            return transformGrab;
        }
        set
        {

        }
    }

    public Vector3 _UI_offset;
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

    

    // Start is called before the first frame update
    public virtual void Start()
    {
        // Sets all the texts of item - for box labels, etc;
        if(itemTextMP.Length > 0)
        {
            //Debug.Log("Setting text on " + gameObject.name);

            for (int i = 0; i < itemTextMP.Length; i++)
            {


                itemTextMP[i].SetText(textItem);
            }
        }

        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnGrab()
    {
        IsGrabbed = true;
    }

    public virtual void OffGrab()
    {
        IsGrabbed = false;
    }

}
