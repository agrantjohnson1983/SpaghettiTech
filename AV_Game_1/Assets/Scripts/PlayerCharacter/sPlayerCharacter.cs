using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerCharacter : MonoBehaviour
{
    GameManager gm;

    //public static sPlayerCharacter playerCharacterGlobal;

    public GameObject model;

    // this is used to switch betweeen players
    int index;

    public SO_CharacterData characterData;

    public SO_EventsUI soUI;

    public SO_VFXEventChannel soVFX;

    sCharacterActionController actionController;
    sCharacterMovementController movementController;
    sCharacterGrabController grabController;
    sToolHandler toolHandler;
    sMouseClickController mouseClickController;

    private void Awake()
    {
        //if (playerCharacterGlobal == null)
        //{
        //    playerCharacterGlobal = this;
            DontDestroyOnLoad(this.gameObject);
        //}

        /*else if (playerCharacterGlobal != this)
        {
            // This is a duplicate (e.g. a test player already placed in a
            // newly loaded scene while the persisted one survives from
            // before). Destroy() is deferred to end of frame, so without
            // the early return AND deactivating the object here, Awake()
            // keeps running on this doomed instance and Start() still
            // fires later this same frame - which was calling
            // gm.AddCharacterToList(this) and adding a soon-to-be-destroyed
            // player into GameManager's list before it disappeared.
            Destroy(this.gameObject);
            gameObject.SetActive(false);
            return;
        }*/

        actionController = GetComponent<sCharacterActionController>();
        movementController = GetComponent<sCharacterMovementController>();
        grabController = GetComponentInChildren<sCharacterGrabController>();
        toolHandler = GetComponent<sToolHandler>();
        mouseClickController = GetComponent<sMouseClickController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //CharacterControlsToggle(false);

        // Safety net matching the Awake() guard - if this somehow isn't the
        // surviving global instance, don't run any of the registration logic
        //if (playerCharacterGlobal != this)
        //{
        //    return;
        //}

        gm = GameManager.gm;


        //gm.SetPlayerGlobal(this);

        if (soVFX != null)
            soVFX.Raise("StarburstLarge", this.transform.position, Quaternion.identity);
    }

    // Sets the player index - only used upon spawn
    public void SetIndex(int _index)
    {
        index = _index;
    }

    // Returns the player index
    public int GetIndex()
    {
        return index;
    }

    // Returns a reference to the action controller for the instance of the character
    public sCharacterActionController ReturnActionController()
    {
        return actionController;
    }

    // Returns a refernce to the movmement controller for the instance of the character
    public sCharacterMovementController ReturnMovementController()
    {
        return movementController;
    }

    // Returns a reference to the grab controller for the instance of the character
    public sCharacterGrabController ReturnGrabController()
    {
        return grabController;
    }

    public sToolHandler ReturnToolHandler()
    {
        return toolHandler;
    }

    // This toggles the controls on/off for a character
    public void CharacterControlsToggle(bool _isOn)
    {
        // Toggles control scripts
        movementController.enabled = _isOn;
        grabController.enabled = _isOn;
        actionController.enabled = _isOn;
        toolHandler.enabled = _isOn;

        // Changes the UI based on character data
        if (_isOn)
            soUI.TriggerCharacterChange(characterData);

        // Resets grabbing
        if (grabController.ReturnIsGrabbing())
        {
            grabController.GrabReset();
        }
    }

    public void ToggleMovement(bool _isOn)
    {
        movementController.enabled = _isOn;
    }

    public void ToggleModelVisibility(bool _isOn)
    {
        model.SetActive(_isOn);
    }
}