using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerCharacter : MonoBehaviour
{
    GameManager gm;

    //sPlayerCharacter playerRef;

    public SO_CharacterData characterData;

    public SO_EventsUI soUI;

    sCharacterActionController actionController;
    sCharacterMovementController movementController;
    sCharacterGrabController grabController;
    sToolHandler toolHandler;

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

        //handsList = new List<handBehavior>(handsNumber);

        hand = new handBehavior();
        handsList = new List<handBehavior>();

        for (int i = 0; i < handsNumber; i++)
        {
            handsList.Add(hand);
        }

        //Debug.Log("Hands list initialized with a count of " + handsList.Count);
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
        gm.SetCurrentPlayer(this);

        movementController.enabled = _isOn;
        
        grabController.enabled = _isOn;
        
        actionController.enabled = _isOn;
        
        toolHandler.enabled = _isOn;

        camera.gameObject.SetActive(_isOn);

        soUI.TriggerCharacterChange(characterData);

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
        Debug.Log("Setting hand");

        for (int i = 0; i < _indexArray.Length-1; i++)
        {
            handsList[_indexArray[i]].SetHand(true);    
        }

        soUI.TriggerItemHeldImage(_itemSprite, _indexArray);
    }


    // Returns -1 if hands are full.  This only checks a hand but does not set anything
    public int[] CheckHands(int _numberOfHandsNeeded)
    {
        Debug.Log("Checking hands with " + _numberOfHandsNeeded + " number of hands needed");

        int[] handsUsedIndexArray = new int[_numberOfHandsNeeded];

        //bool canUse = false;
        //handBehavior tempHand = null;

        // Set to -1 by default
        //int _tempIndex = -1;

        Debug.Log("Hands Index Array has length of " + handsUsedIndexArray.Length);

        Debug.Log("Hands List Count = " + handsList.Count);

        if(handsList == null)
        {
            Debug.Log("Hands List Null");
        }

        // -1 for the 0 offset
        for (int i = 0; i < handsList.Count; i++)
        {

            bool canUse = true;

            Debug.Log("Running check on hand list index of " + i);

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

                Debug.Log("Temp hand set to hand number " + i);
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
