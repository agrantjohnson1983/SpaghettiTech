using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sRiggingSetupSpot : MonoBehaviour, iActionable
{
    public float _taskTime;

    public bool hasAction = false; // use this for turning on objects that have actions

    public Vector3 offset;

    bool _canTriggerAction; // this is for controls to check if an action can be triggered

    bool hasBeenSet = false;

    public eTypeRigSetup rigType;

    public eToolType _toolTypeNeeded;

    public TextMeshPro textSetup;

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

    public SO_EventsUI uiEvents;

    public void TriggerAction(GameObject _actionObj, eToolType _toolToUse)
    {
        //base.TriggerAction(_actionObj, _toolToUse);

        if(CanTriggerAction && hasAction)
        {
            StartCoroutine(ActionTasking());
        }

        else
        {
            Debug.Log("Cant Trigger Action");
        }
    }

    public void StopAction(GameObject _actionObj)
    {
        //base.StopAction(_actionObj);

        Debug.Log("Stopping Action Tasking Cortoutine");

        StopCoroutine(ActionTasking());

        uiEvents.TriggerTaskGauge(0);
    }

    IEnumerator ActionTasking()
    {
        Debug.Log("Starting Action Tasking");

        uiEvents.TriggerTaskGauge(TaskTime);

        yield return new WaitForSeconds(TaskTime);

        Debug.Log("Action Task Complete");

        sRiggingManager.riggingManger.RigSet(rigType);

        Destroy(this.gameObject);
    }

    public void WrongTool()
    {
        textSetup.color = Color.red;

        CanTriggerAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasAction)
            ToolCheck(other.gameObject);

        if (other.TryGetComponent(out iRiggable _riggable))
        {
            // Checks that collided rig type is same as setup type
            if(_riggable.TypeRig == rigType)
            {
                switch (_riggable.TypeRig)
                {
                    case eTypeRigSetup.truss:
                        {
                            if (_riggable.Enabled)
                            {
                                Debug.Log("Truss Collision with Setup Spot");

                                _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                                //_riggable.Enabled = false;

                                _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                                other.gameObject.GetComponent<sRigGear>().enabled = false;

                                Destroy(this.gameObject);
                            }

                            break;

                        }

                    case eTypeRigSetup.motor:
                        {
                            if (_riggable.Enabled)
                            {
                                Debug.Log("Motor Collision with Setup Spot");

                                _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                                _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                                other.gameObject.GetComponent<sRigGear>().enabled = false;

                                Destroy(this.gameObject);
                            }

                            break;
                        }

                    case eTypeRigSetup.motorController:
                        {

                            if (_riggable.Enabled)
                            {
                                Debug.Log("Motor Collision with Setup Spot");

                                _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                                _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                                other.gameObject.GetComponent<sRigGear>().enabled = false;

                                Destroy(this.gameObject);
                            }

                            break;
                        }
                }
            }

            else
            {
                Debug.Log("Wrong setup spot type");
            }
            
        }
    }

    // TO DO - Check to see how close the rigging piece rotation is compared to the setup spot rotation


    void ToolCheck(GameObject _toolCheckObj)
    {
        // Checks for tool handler - located on Player
        if(_toolCheckObj.TryGetComponent<sToolHandler>(out sToolHandler _toolHandler))
        {
            Debug.Log("Collision with player - checking tool");

            // Checks to see if there are any tools held
            if(_toolHandler.ReturnToolHeldList() != null)
            {
                // Correct tool if true
                if (_toolHandler.CheckIfHasTool(ToolTypeNeeded))
                {
                    CanTriggerAction = true;

                    textSetup.SetText("CORRECT TOOL");

                    textSetup.color = Color.green;
                }

                else
                {
                    WrongTool();
                }  
            }

            else
            {
                WrongTool();
            }

        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player")  && HasAction)
        {
            CanTriggerAction = false;

            if(IsDoingAction)
            {
                StopAction(null);
            }
        }
        
    }
}
