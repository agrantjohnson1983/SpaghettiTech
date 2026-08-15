using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class sSetupSpotBASE : MonoBehaviour, iActionable
{
    public float _taskTime;

    public bool hasAction = false; // use this for turning on objects that have actions

    public Vector3 offset;

    bool _canTriggerAction; // this is for controls to check if an action can be triggered

    bool hasBeenSet = false;

    public bool toolNeeded = false;

    public eToolType _toolTypeNeeded;

    public TextMeshPro textSetup;

    public Vector3 _UI_offset;

    protected bool isSetup = false;

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

    public SO_AudioEventChannel soAudio;

    public GameObject actionObject;


    public IEnumerator SmoothMovement(GameObject _object, Vector3 _endPos, Quaternion _endRot)
    {
        Debug.Log("Starting smooth movement for " + _object + " from start pos of: " + _object.transform.position + " to end pos: " + _endPos);

        float counter = 0f;

        _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        _object.GetComponent<Rigidbody>().useGravity = false;

        Vector3 _startingPos = _object.transform.position;
        Quaternion _startingRot = _object.transform.rotation;

        while (counter < 0.5f)
        {
            _object.transform.position = Vector3.Lerp(_startingPos, _endPos, (counter / 0.5f));

            _object.transform.rotation = Quaternion.Slerp(_startingRot, _endRot, (counter / 0.5f));

            counter += Time.deltaTime;

            yield return null;
        }

        //Debug.Log("Smooth movement finished for)

        _object.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        Destroy(this.gameObject, 0.5f);

    }

    public virtual void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //throw new System.NotImplementedException();
    }

    public virtual void StopAction()
    {
        //throw new System.NotImplementedException();
    }
}
