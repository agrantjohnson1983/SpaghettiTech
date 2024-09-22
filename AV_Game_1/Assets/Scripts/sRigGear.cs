using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sRigGear : MonoBehaviour, iRiggable, iGrabbable
{
    public SO_EventsUI eventsUI;

    public eTypeRigSetup _typeRig;

    bool _isGrabbed = false;

    bool _isSet = false;

    public bool _grabTriggersAction;
    
    public bool CanBeGrabbed
    {
        get;
        set;
    }

    bool enabled = true;

    public bool Enabled

    {
        get
        {
            return enabled;
        }

        set
        {
            enabled = value;
        }
    }

    public bool IsSet
    {
        get
        {
            return _isSet;
        }

        set
        {
            _isSet = value;
        }
    }

    public eTypeRigSetup TypeRig
    {
        get
        {
            return _typeRig;
        }

        set
        {
            _typeRig = value;
        }
    }

    // Used for interface to find location of rig
    public Transform TransformRig
    {
        get
        {
            return this.transform;
        }

        set
        {

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

    private void Start()
    {
        CanBeGrabbed = true;
    }

    // Update is called once per frame

    void Init()
    {
        switch(TypeRig)
        {
            case eTypeRigSetup.truss:
                {
                    //sRiggingManager.riggingManger.trussList.Add(this.gameObject);

                    break;
                }

            case eTypeRigSetup.bolts:
                {
                    break;
                }

            case eTypeRigSetup.motor:
                {
                    break;
                }
            case eTypeRigSetup.speakerStand:
                {
                    break;
                }
            case eTypeRigSetup.videoScreenStand:
                {
                    break;
                }
        }
    }

    public void OnGrab()
    {

    }

    public void OffGrab()
    {

    }

    public virtual void TriggerGrabAction()
    {

    }
    public virtual void StopGrabAction()
    {

    }
}
