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

    // Per-instance - each grabbable tracks whether IT is currently held.
    // This must not be static: a static property here is one single flag
    // shared by every object in the game, which means it can never actually
    // answer "is this specific object grabbed" and any check against it is
    // either always true or always false for everyone.
    bool IsGrabbed
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

    void OnGrab(sPlayerCharacter _player);

    void OffGrab();

    void OnSelect(sPlayerCharacter _player);

    void OffSelect();

}