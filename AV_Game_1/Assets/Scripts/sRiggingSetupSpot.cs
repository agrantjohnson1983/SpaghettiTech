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

    public SO_EventsUI soUI;

    public void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //base.TriggerAction(_actionObj, _toolToUse);

        if(CanTriggerAction && hasAction)
        {
            StartCoroutine(ActionTasking());

            //soUI.TriggerItemHeldImage(_itemData.itemSprite, );
        }

        else
        {
            Debug.Log("Cant Trigger Action");
        }
    }

    public void StopAction()
    {
        Debug.Log("Stopping Action Tasking Cortoutine");

        StopCoroutine(ActionTasking());

        StopAllCoroutines();

        GameManager.gm.ReturnCurrentPlayer().ToggleMovement(true);

        soUI.TriggerTaskGauge(0, Vector3.zero);
    }

    IEnumerator ActionTasking()
    {
        Debug.Log("Starting Action Tasking");

        GameManager.gm.ReturnCurrentPlayer().ToggleMovement(false);

        soUI.TriggerTaskGauge(TaskTime, this.transform.position + ui_offset);

        yield return new WaitForSeconds(TaskTime);

        Debug.Log("Action Task Complete");

        sRiggingManager.riggingManger.RigSet(rigType);

        GameManager.gm.ReturnCurrentPlayer().ToggleMovement(true);

        Destroy(this.gameObject);
    }

    public void WrongTool()
    {
        Debug.Log("Wrong Tool");

        textSetup.color = Color.red;

        CanTriggerAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rigging Setup On Trigger Enter");

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
            //Debug.Log("Collision with player and " + this.name + " - checking tool");

            // Checks to see if there are any tools held
            if(_toolHandler.ReturnToolHeldList() != null)
            {
                //Debug.Log("Tool list isn't null");

                SO_ItemData tempToolData;

                tempToolData = _toolHandler.CheckIfHasTool(ToolTypeNeeded);

               // Debug.Log(tempTool + " temp tool");

                // Returns null if tool is not correct
                if (tempToolData != null)
                {
                    //Debug.Log("Temp Tool Not Null and Setting UI");

                    // Toggles bool
                    CanTriggerAction = true;

                    // Displays text above spot
                    textSetup.SetText("CORRECT TOOL");

                    // Sets the color of the text to green showing the tool is good
                    textSetup.color = Color.green;
                }

                else
                {
                    Debug.Log("Temp Tool Data is null");

                    // If the player character doesn't have the correct tool
                    WrongTool();
                }  
            }

            else
            {
                Debug.Log("Tool Handler List is null");

                // If the player character doesn't have the correct tool
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
                StopAction();
            }
        }
        
    }
}
