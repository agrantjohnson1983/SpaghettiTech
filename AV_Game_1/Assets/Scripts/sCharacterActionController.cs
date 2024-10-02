using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterActionController : MonoBehaviour
{
    public SO_EventsUI soUI;

    sToolHandler toolHandler;

    GameObject interactiveObject = null;

    iActionable actionable = null;

    bool isTouchingActionable = false;

    bool isDoingAction = false;

    SO_ItemData itemData = null;

    public string popupControlText = "Left Click For Action";


    // Start is called before the first frame update
    void Start()
    {
        toolHandler = GetComponent<sToolHandler>();
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
            // This checks for input to trigger an actionable.  Does a null check for actionable interface and interactive object
            if (Input.GetMouseButton(0))// && actionable != null && interactiveObject != null)
            {
                if(actionable!=null && interactiveObject != null)
                {
                    isDoingAction = true;

                    Debug.Log("Action triggered on " + actionable.ToString());

                    actionable.TriggerAction(interactiveObject, itemData);
                }   
            }

            else
            {
                if (actionable != null && interactiveObject != null)
                {
                    isDoingAction = false;

                    Debug.Log("Action Stopped in  " + interactiveObject.name);

                    actionable.StopAction();
                }
            }

            
            // This stops an action
            if (Input.GetMouseButtonUp(0) && actionable != null)
            {
                isDoingAction = false;

                Debug.Log("Action Stopped on  " + actionable);

                actionable.StopAction();      
            }
            
        }

        else
        {
            // This stops an active action.  Does a check for an actionable
            if (Input.GetMouseButtonUp(0) && actionable != null)
            {
                isDoingAction = false;

                Debug.Log("Action Stopped with " + actionable.ToString());

                actionable.StopAction();
            }
        }
    }

    // This checks if an item has an actionable interface and sets it if so and triggers UI
    void ActionableCheck(GameObject _collisionObj)
    {
        if (_collisionObj.TryGetComponent(out iActionable _actionable))
        {
            // Sets the actionable
            actionable = _actionable;
            interactiveObject = _collisionObj;
            isTouchingActionable = true;

            // This sets a temp tool that will be null if the player doesn't have the right tool
            SO_ItemData tempToolData = toolHandler.CheckIfHasTool(actionable.ToolTypeNeeded);

            // Checks if the tool isn't null
            if(tempToolData != null)
            {
                Debug.Log("Item data set from " + tempToolData.name);

                // Sets the item data to the tools item data
                itemData = tempToolData;
            }
            


            if(_actionable.HasAction)
            {
                soUI.ToggleControlsPopup(popupControlText);
            }
        }
    }

    /*
    public bool CheckIfRightToolHeld(eToolType _toolNeeded)
    {
        if(toolHandler.CheckIfHasTool(_toolNeeded) == true)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    */

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
                soUI.ToggleControlsPopup(null);

                isTouchingActionable = false;

                interactiveObject = null;

                actionable = null;
                //Debug.Log(_actionable.ToString() + "Actionable has excited trigger");
            }
        }
    }
}
