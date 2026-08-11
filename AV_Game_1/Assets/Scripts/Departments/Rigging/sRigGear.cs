using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sRigGear : sInteractive, iRiggable
{
    public eTypeRigSetup _typeRig;

    bool _isSet = false;

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

    // Virtual so subclasses (e.g. sMotor) can hook additional behavior
    // into the moment a piece actually gets rigged, without duplicating
    // rigging logic that already lives in iRiggable.SetRigging.
    public virtual bool IsSet
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

    public SO_ItemData ItemData { get { return _itemData; } set { _itemData = value; } }

    public SO_ItemData _itemData;
    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
        //Init();
        CanBeGrabbed = true;
    }



    public virtual void TriggerGrabAction()
    {

    }
    public virtual void StopGrabAction()
    {

    }

}