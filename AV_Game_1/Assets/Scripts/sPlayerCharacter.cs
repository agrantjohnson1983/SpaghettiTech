using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerCharacter : MonoBehaviour
{
    GameManager gm;

    // this is used to switch betweeen players
    int index;

    public SO_CharacterData characterData;

    public SO_EventsUI soUI;

    sCharacterActionController actionController;
    sCharacterMovementController movementController;
    sCharacterGrabController grabController;
    sToolHandler toolHandler;
    sMouseClickController mouseClickController;

    Camera camera;

    public int handsNumber = 2;
    public List<handBehavior> handsList;

    handBehavior hand;

    // Use this class for future hand behaviors
    public class handBehavior
    {
        bool isBeingUsed;

        public void SetHand(bool _isUsed)
        {
            isBeingUsed = _isUsed;
        }

        public bool ReturnIsBeingUsed()
        {
            return isBeingUsed;
        }
    }

    private void Awake()
    {
        //playerGlobal = this;

        camera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        actionController = GetComponent<sCharacterActionController>();
        movementController = GetComponent<sCharacterMovementController>();
        grabController = GetComponent<sCharacterGrabController>();
        toolHandler = GetComponent<sToolHandler>();
        mouseClickController = GetComponent<sMouseClickController>();

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

        // adds to the GM's list of players
        gm.AddCharacterToList(this);

        // Sets the index to the list index -1 to match array start
        SetIndex(gm.ReturnPlayerList().Count - 1);

        //handsList = new List<handBehavior>(handsNumber);

        hand = new handBehavior();
        handsList = new List<handBehavior>();

        for (int i = 0; i < handsNumber; i++)
        {
            handsList.Add(hand);
        }

        //Debug.Log("Hands list initialized with a count of " + handsList.Count);
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

    // Sets this instance to the current player in the GameManager and turns on the controlls
    public void SetToCurrentPlayer()
    {
        gm.SetCurrentPlayer(this);

        CharacterControlsToggle(true);
    }

    // This toggles the controls on/off for a character
    public void CharacterControlsToggle(bool _isOn)
    {
        // Toggles control scripts
        movementController.enabled = _isOn;       
        grabController.enabled = _isOn;     
        actionController.enabled = _isOn;
        toolHandler.enabled = _isOn;

        // Keep the mouse on for all characters so any of them can be clicked?

        //mouseClickController.enabled = _isOn;
        //mouseClickController.ToggleMouseClickController(_isOn);

        // turns camera on/off
        camera.gameObject.SetActive(_isOn);

        // Changes the UI based on character data
        if(_isOn)
        soUI.TriggerCharacterChange(characterData);

        // Resets grabbing
        if(grabController.ReturnIsGrabbing())
        {
            grabController.GrabReset();
        }

        //soUI.ToggleControlsPopup(null, Vector3.zero);
    }

    public void ToggleCameraMain(bool _isOn)
    {
        camera.gameObject.SetActive(_isOn);
    }

    // This will reset a hand and trigger the UI
    public void ResetHand(int[] _indexArray)
    {
        Debug.Log("Resetting hand");

        for (int i = 0; i < _indexArray.Length; i++)
        {
            if (_indexArray[i] > 0)
            {
                handsList[_indexArray[i]].SetHand(false);   
            }
        }

        soUI.TriggerItemHeldImage(null, _indexArray);
    }

    // This will set a hand to use and trigger the UI
    public void SetHand(Sprite _itemSprite, int[] _indexArray)
    {
        //Debug.Log("Setting hand");

        for (int i = 0; i < _indexArray.Length-1; i++)
        {
            handsList[_indexArray[i]].SetHand(true);    
        }

        soUI.TriggerItemHeldImage(_itemSprite, _indexArray);
    }


    // Returns -1 if hands are full.  This only checks a hand but does not set anything
    public int[] CheckHands(int _numberOfHandsNeeded)
    {
        //Debug.Log("Checking hands with " + _numberOfHandsNeeded + " number of hands needed");

        int[] handsUsedIndexArray = new int[_numberOfHandsNeeded];

        //bool canUse = false;
        //handBehavior tempHand = null;

        // Set to -1 by default
        //int _tempIndex = -1;

        //Debug.Log("Hands Index Array has length of " + handsUsedIndexArray.Length);

        //Debug.Log("Hands List Count = " + handsList.Count);

        if(handsList == null)
        {
            Debug.Log("Hands List Null");
        }

        // -1 for the 0 offset
        for (int i = 0; i < handsList.Count; i++)
        {

            bool canUse = true;

           // Debug.Log("Running check on hand list index of " + i);

            // Checks if a hand is available to use
            if(!handsList[i].ReturnIsBeingUsed())
            {
                //canUse = true;
                //handsArray[i].SetHand(true);
                //handsArray[i].SetSprite(_itemSprite);
                //soUI.TriggerItemHeldImage(_itemSprite, i);
                //tempHand = handsArray[i];
                if(handsUsedIndexArray == null)
                {
                    Debug.Log("HandsUsedIndexArray is null");
                }

                //Debug.Log("Temp hand set to hand number " + i);
                //_tempIndex = i;
                handsUsedIndexArray[i] = i;
                //return tempHand;
            }

            else
            {
                handsUsedIndexArray[i] = -1;
                soUI.ToggleControlsPopup("HANDS FULL");
                canUse = false;
            }

            
            //if(canUse == true)
            //{
             //   return null;
            //}
        }


        return handsUsedIndexArray;
    }

    public void ToggleMovement(bool _isOn)
    {
        movementController.enabled = _isOn;
    }
        
}
