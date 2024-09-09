using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterActionController : MonoBehaviour
{
    public SO_EventsUI soUI;

    GameObject interactiveObject;

    GameObject toolObject;

    iActionable actionable;

    bool isTouchingActionable = false;

    bool isDoingAction = false;

    bool hasTool;

    eToolType toolTypeHeld = eToolType.NONE;

    public string popupControlText = "Left Click For Action";

    // Start is called before the first frame update
    void Start()
    {
        interactiveObject = null;

        actionable = null;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAction();
    }

    void HandleAction()
    {
        if (isTouchingActionable && !isDoingAction)
        {
            if (Input.GetMouseButton(0) && actionable != null && interactiveObject != null)
            {
                    isDoingAction = true;
                    Debug.Log("Action triggered on " + actionable.ToString());
                    actionable.TriggerAction(interactiveObject, toolTypeHeld);
            }


            if (Input.GetMouseButtonUp(0) && actionable != null)
            {
                isDoingAction = false;

                Debug.Log("Action Stopped with " + interactiveObject.name);
                actionable.StopAction(interactiveObject);
                
            }
        }

        else
        {
            if (Input.GetMouseButtonUp(0) && actionable != null)
            {
                isDoingAction = false;

                Debug.Log("Action Stopped with " + interactiveObject.name);
                actionable.StopAction(interactiveObject);
                
            }
        }
    }

    void ActionableCheck(GameObject _collisionObj)
    {
        if (_collisionObj.TryGetComponent(out iActionable _actionable))
        {
            if(_actionable.HasAction)
            {
                soUI.ToggleControlsPopup(popupControlText, _collisionObj.transform.position + _actionable.ui_offset);

                if (_collisionObj == toolObject)
                {
                    //Debug.Log("Collision with tool object");

                    return;
                }

                else
                {
                    //Debug.Log("Touching Actionable and can trigger an action with LEFT CLICK");
                    isTouchingActionable = true;
                    actionable = _actionable;
                    interactiveObject = _collisionObj;

                    //GetComponent<sCharacterGrabController>().SetActionable(actionable);
                }
            }
            
        }

        //else
        //{
            //Debug.Log("Collision with non Actionable");
            //isTouchingActionable = false;
            //actionable = null;
        //}
    }

    // Checks on collision to see if a character is touchign a tool
    void ToolCheck(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<sTool>(out sTool _tool))
        {
            hasTool = true;

            toolObject = collision.gameObject;

            toolTypeHeld = _tool.typeOfTool;

            //Debug.Log(toolTypeHeld.ToString() + " tool picked up.");

            interactiveObject = null;
        }
    }

    public bool CheckIfRightToolHeld(eToolType _toolNeeded)
    {
        if (_toolNeeded == toolTypeHeld)
            return true;
        else return false;
    }

    public eToolType ReturnToolTypeHeld()
    {
        return toolTypeHeld;
    }

    private void OnTriggerStay(Collider other)
    {
        ActionableCheck(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<iActionable>(out iActionable _actionable) && actionable != null)
        {
            if(actionable == _actionable)
            {
                if(actionable.HasAction)
                soUI.ToggleControlsPopup(null, Vector3.zero);

                //Debug.Log(_actionable.ToString() + "Actionable has excited trigger");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ActionableCheck(collision.gameObject);;
    }

    private void OnCollisionStay(Collision collision)
    {
       // ActionableCheck(collision.gameObject);
    }



}
