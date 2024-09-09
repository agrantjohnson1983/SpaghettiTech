using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerCharacter : MonoBehaviour
{
    GameManager gm;

    public static sPlayerCharacter playerGlobal;

    public SO_CharacterData characterData;

    public SO_EventsUI soUI;

    sCharacterActionController actionController;
    sCharacterMovementController movementController;
    sCharacterGrabController grabController;
    sToolHandler toolHandler;

    Camera camera;

    private void Awake()
    {
        playerGlobal = this;

        camera = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        actionController = GetComponent<sCharacterActionController>();
        movementController = GetComponent<sCharacterMovementController>();
        grabController = GetComponent<sCharacterGrabController>();
        toolHandler = GetComponent<sToolHandler>();

        gm = GameManager.gm;

        // Sets first player spawned active
        if(gm.ReturnPlayerList().Count <= 0)
        {
            //gm.ToggleOverheadCamera(false);

            CharacterControlsToggle(false);
        }

        else
        {
            CharacterControlsToggle(false);
        }

        gm.AddCharacterToList(this);

        
    }
    public sCharacterActionController ReturnActionController()
    {
        return actionController;
    }

    public sCharacterMovementController ReturnMovementController()
    {
        return movementController;
    }

    public sCharacterGrabController ReturnGrabController()
    {
        return grabController;
    }

    public void CharacterControlsToggle(bool _isOn)
    {
        movementController.enabled = _isOn;
        grabController.enabled = _isOn;
        actionController.enabled = _isOn;
        toolHandler.enabled = _isOn;

        camera.gameObject.SetActive(_isOn);

        soUI.TriggerCharacterChange(characterData);
    }

    public void ToggleCameraMain(bool _isOn)
    {
        camera.gameObject.SetActive(_isOn);
    }
}
