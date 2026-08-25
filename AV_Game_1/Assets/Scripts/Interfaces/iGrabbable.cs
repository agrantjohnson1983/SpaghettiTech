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
    // Read-only: grabbed state is derived from how many players are
    // actively holding this object (see HolderCount on sInteractive), not
    // set directly, since more than one player can hold the same object at
    // once now.
    bool IsGrabbed
    {
        get;
    }


    // Use this to toggle the grab system on/off externally from the grab controller, like when a connection is connected, etc.
    public bool CanBeGrabbed
    {
        get;
        set;
    }

    void OnGrab(sPlayerCharacter _player);

    // _player is specifically who is letting go. Only that player's hold is
    // released - other current holders, if any, are unaffected.
    void OffGrab(sPlayerCharacter _player);

    void OnSelect(sPlayerCharacter _player);

    void OffSelect();

}