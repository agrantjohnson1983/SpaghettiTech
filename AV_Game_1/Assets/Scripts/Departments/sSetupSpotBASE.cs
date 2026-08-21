using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class sSetupSpotBASE : MonoBehaviour, iActionable
{
    public float _taskTime;

    public bool hasAction = false; // use this for turning on objects that have actions

    public Vector3 offset;

    bool _canTriggerAction; // this is for controls to check if an action can be triggered

    protected bool hasBeenSet = false;

    //public bool toolNeeded = false;

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

    public SO_AudioEventChannel soAudio;

    public SO_VFXEventChannel soVFX;

    public SO_Text soText;

    public GameObject actionObject;

    //sPlayerCharacter playerRef = null;

    public IEnumerator ReleaseThenMove(GameObject _object, Vector3 _endPos, Quaternion _endRot, bool _destroyAtEnd = true)
    {
        //sPlayerCharacter.playerCharacterGlobal.ReturnGrabController().GrabReset();

        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(SmoothMovement(_object, _endPos, _endRot, _destroyAtEnd));
    }

    IEnumerator SmoothMovement(GameObject _object, Vector3 _endPos, Quaternion _endRot, bool _destroyAtEnd)
    {
        //Debug.Log("Starting smooth movement for " + _object + " from start pos of: " + _object.transform.position + " to end pos: " + _endPos);

        if(_object == null)
        {
            Debug.LogWarning("Smooth movement object is null");
            yield return null;
        }

        

        float counter = 0f;

        Collider _collider;

        if(_object.TryGetComponent<Collider>(out _collider))
        {
            _collider.enabled = false;
        }

        /*Collider[] _colliders = _object.GetComponentsInChildren<Collider>();

        foreach (Collider c in _colliders)
        {
            c.enabled = false;
        }
*/
        Rigidbody _rb;

        if(_object.TryGetComponent<Rigidbody>(out _rb))
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.isKinematic = true;
        }

        if(_object.TryGetComponent<FixedJoint>(out FixedJoint _joint))
        {
            Debug.Log("Fixed joint was found on " + _object);

            if(_object.TryGetComponent<sInteractive>(out sInteractive _interactive))
            {
                Debug.Log("Setting interactive object for setup");
                _interactive.OffGrab();
                
            }

            Destroy(_joint);

            //sPlayerCharacter.playerCharacterGlobal.ReturnGrabController().GrabReset();
        }

        Vector3 _startingPos = _object.transform.position;
        Quaternion _startingRot = _object.transform.rotation;

        while (counter < 1f)
        {
            float _time = counter / 1f;

            _object.transform.position = Vector3.Lerp(_startingPos, _endPos, _time);

            _object.transform.rotation = Quaternion.Slerp(_startingRot, _endRot, _time);

            counter += Time.deltaTime;

            yield return null;
        }

        Debug.Log("Smooth movement finished for " + _object);


        if(_rb)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if(_collider)
            _collider.enabled = true;

        /*_colliders = _object.GetComponentsInChildren<Collider>();

        foreach (Collider c in _colliders)
        {
            c.enabled = true;
        }*/

        if (soAudio != null)
            soAudio.TriggerSFX("SetupComplete");

        if (soVFX != null)
            soVFX.Raise("StarburstSmall", this.transform.position + Vector3.up, Quaternion.identity);

        if (soText != null)
            soText.SpawnTextPopup(this.transform, "SET!", 10);

        Debug.Log("Smooth movement coroutine finished for " + _object);

        if (_destroyAtEnd)
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

    public void ExitMinigame()
    {
        Debug.Log("[" + this.name + "] Exiting minigame without completing");

        if (actionObject != null)
        {
            actionObject.SetActive(false);
        }

        GameManager.gm.canvasGameplayObject.SetActive(true);
        //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);
    }

    public void WrongTool()
    {
        //Debug.Log("Wrong Tool");

        textSetup.gameObject.SetActive(true);

        textSetup.color = Color.red;

        textSetup.SetText("NEED: " + ToolTypeNeeded);

        CanTriggerAction = false;
    }

    protected bool ToolCheck(GameObject _toolCheckObj)
    {
        if (!_toolCheckObj.CompareTag("Player"))
        {
            Debug.Log("Collision is not a player object");
            return false;
        }
            

        // Checks for tool handler - located on Player
        if (_toolCheckObj.TryGetComponent<sToolHandler>(out sToolHandler _toolHandler))
        {
            //Debug.Log("Collision with player and " + this.name + " - checking tool");

            // Checks to see if there are any tools held
            if (_toolHandler.CheckIfHasTool(ToolTypeNeeded) != null)
            {

                // Toggles bool
                CanTriggerAction = true;

                // Displays text above spot
                textSetup.SetText("CORRECT TOOL");

                // Sets the color of the text to green showing the tool is good
                textSetup.color = Color.green;

                return true;
            }


            else
            {
                // If the player character doesn't have the correct tool
                WrongTool();

                return false;
            }

        }

        else
        {
            Debug.LogWarning("No tool handler on player");

            return false;
        }

    }

    public virtual void FinishSetup()
    {
        
    }
}
