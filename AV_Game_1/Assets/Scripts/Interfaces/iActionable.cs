using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iActionable
{
    float TaskTime
    {
        get;
        set;
    }

    Vector3 ui_offset
    {
        get;
        set;
    }

    bool HasAction { get; set; }

    bool CanTriggerAction
    {
        get;
        set;
    }

    bool IsDoingAction
    {
        get;
        set;
    }

    eToolType ToolTypeNeeded
    {
        get;
        set;
    }

    public void TriggerAction(GameObject _actionObj, SO_ItemData _itemData);


    public void StopAction();

}
