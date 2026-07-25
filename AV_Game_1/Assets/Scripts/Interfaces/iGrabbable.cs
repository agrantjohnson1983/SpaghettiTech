using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iGrabbable
{
    Transform transformGrab
    {
        get;
        set;
    }

    Vector3 ui_offset
    {
        get;
        set;
    }

    static bool IsGrabbed
    {
        get;
        set;
    }


    // Use this to toggle the grab system on/off externally from the grab controller, like when a connection is connected, etc.
    public bool CanBeGrabbed
    {
        get;
        set;
    }

    void OnGrab();

    void OffGrab();

    void OnSelect();

    void OffSelect();

}
