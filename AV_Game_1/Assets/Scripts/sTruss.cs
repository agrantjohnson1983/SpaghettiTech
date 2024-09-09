using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTruss : sRigGear
{

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

    public eTypeRigSetup TypeRig
    {
        get;
        set;
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

    private void Start()
    {
        sRiggingManager.riggingManger.trussList.Add(this);
    }


}
