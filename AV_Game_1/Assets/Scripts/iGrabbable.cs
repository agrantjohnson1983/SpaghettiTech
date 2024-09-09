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

    void OnGrab();

    void OffGrab();

}
