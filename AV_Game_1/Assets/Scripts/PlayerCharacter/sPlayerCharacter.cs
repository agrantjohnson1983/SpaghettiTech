using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerCharacter : MonoBehaviour
{
    GameManager gm;

    public static sPlayerCharacter playerCharacterGlobal;

    public GameObject model;

    // this is used to switch betweeen players
    int index;

    public SO_CharacterData characterData;

    public SO_EventsUI soUI;

    sCharacterActionController actionController;
    sCharacterMovementController movementController;
    sCharacterGrabController grabController;
    sToolHandler toolHandler;
    sMouseClickController mouseClickController;

    Camera cam;

    Vector3 camPos;
    Quaternion camRot;

    public int handsNumber = 2;
    public List<handBehavior> handsList;

    handBehavior hand;

    bool isInTruckMode = false;

    CinemachineVirtualCamera vCam;

    public Vector3 camOffset, camStart;

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
        if (playerCharacterGlobal == null)
        {
            playerCharacterGlobal = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else if (playerCharacterGlobal != this)
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
        }

        vCam = GetComponentInChildren<CinemachineVirtualCamera>();

        actionController = GetComponent<sCharacterActionController>();
        movementController = GetComponent<sCharacterMovementController>();
        grabController = GetComponent<sCharacterGrabController>();
        toolHandler = GetComponent<sToolHandler>();
        mouseClickController = GetComponent<sMouseClickController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterControlsToggle(false);

        // Safety net matching the Awake() guard - if this somehow isn't the
        // surviving global instance, don't run any of the registration logic
        if (playerCharacterGlobal != this)
        {
            return;
        }

        gm = GameManager.gm;

        cam = gm.ReturnCameraGameplay().GetComponentInChildren<Camera>();

        gm.SetPlayerGlobal(this);

        //// Sets first player spawned active
        //if (gm != null)
        //    if (gm.ReturnPlayerList().Count <= 0)
        //    {
        //        //gm.ToggleOverheadCamera(false);

        //        CharacterControlsToggle(false);
        //    }

        //    else
        //    {

        //        CharacterControlsToggle(false);
        //    }

        //// adds to the GM's list of players
        //gm.AddCharacterToList(this);

        //// Sets the index to the list index -1 to match array start
        //SetIndex(gm.ReturnPlayerList().Count - 1);

        //handsList = new List<handBehavior>(handsNumber);

        hand = new handBehavior();
        handsList = new List<handBehavior>();

        for (int i = 0; i < handsNumber; i++)
        {
            handsList.Add(hand);
        }

        camStart = vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        //Debug.Log("Cam start is " + camStart);

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

    public sToolHandler ReturnToolHandler()
    {
        return toolHandler;
    }

    //// Sets this instance to the current player in the GameManager and turns on the controlls
    //public void SetToCurrentPlayer()
    //{
    //    gm.SetCurrentPlayer(this);

    //    CharacterControlsToggle(true);
    //}

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

        if (cam == null)
            cam = GameManager.gm.ReturnCameraGameplay().GetComponent<Camera>();

        // turns camera on/off
        //cam.gameObject.SetActive(_isOn);

        // Changes the UI based on character data
        if (_isOn)
            soUI.TriggerCharacterChange(characterData);

        // Resets grabbing
        if (grabController.ReturnIsGrabbing())
        {
            grabController.GrabReset();
        }

        //soUI.ToggleControlsPopup(null, Vector3.zero);
    }

    public void ToggleCameraMain(bool _isOn)
    {
        cam.gameObject.SetActive(_isOn);
    }

    Coroutine truckCamRoutine;

    public void ToggleTruckCamera(bool _isOn, float _moveTime)
    {
        // Ignore duplicate calls - already in the requested state
        if (_isOn == isInTruckMode)
        {
            Debug.LogWarning($"ToggleTruckCamera({_isOn}) ignored - already in that state.");
            return;
        }

        isInTruckMode = _isOn;

        if (truckCamRoutine != null)
            StopCoroutine(truckCamRoutine);

        if (_isOn)
        {
            camPos = cam.transform.localPosition;
            camRot = cam.transform.localRotation;

            cam.transform.SetParent(null);

            truckCamRoutine = StartCoroutine(TruckCamMovementWorld(0.5f, true));
        }

        else
        {
            cam.transform.SetParent(this.gameObject.transform);
            cam.transform.localRotation = camRot;

            truckCamRoutine = StartCoroutine(TruckCamMovementLocal(_moveTime, false));
        }
    }

    // Used only while unparented (entry) — world space lerp is correct here
    IEnumerator TruckCamMovementWorld(float _time, bool _isOn)
    {
        Debug.Log("Starting truck movement");

        float _counter = 0f;

        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        if(_isOn)
        {
            startPoint = camStart;
            endPoint = camOffset;
        }

        else
        {
            startPoint = camOffset;
            endPoint = camStart;
        }

        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();

        while (_counter < _time)
        {
            transposer.m_FollowOffset = Vector3.Lerp(startPoint, endPoint, _counter / _time);
            _counter += Time.deltaTime;
            yield return null;
        }

        //cam.transform.position = _endPos; // snap, same pattern as sCharacterMover fix

        //if (_isOn)
        //    cam.transform.LookAt(this.transform);
    }

    // Used only while parented (exit) — local space lerp tracks the moving player
    IEnumerator TruckCamMovementLocal(float _time, bool _isOn)
    {
        float _counter = 0f;

        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        if (_isOn)
        {
            startPoint = camStart;
            endPoint = camOffset;
        }

        else
        {
            startPoint = camOffset;
            endPoint = camStart;
        }

        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();

        while (_counter < _time)
        {
             transposer.m_FollowOffset = Vector3.Lerp(startPoint, endPoint, _counter / _time);
            _counter += Time.deltaTime;
            yield return null;
        }

        //Debug.Log($"Reset complete. localPosition = {cam.transform.localPosition}, expected = {_endLocalPos}");

        // camera.transform.localPosition = _endLocalPos; // snap to guarantee exact reset
    }


    // This will reset a hand and trigger the UI
    //public void ResetHand(int[] _indexArray)
    //{
    //    //Debug.Log("Resetting hand");

    //    for (int i = 0; i < _indexArray.Length; i++)
    //    {
    //        if (_indexArray[i] > 0)
    //        {
    //            handsList[_indexArray[i]].SetHand(false);   
    //        }
    //    }

    //    soUI.TriggerItemHeldImage(null, _indexArray);
    //}

    // This will set a hand to use and trigger the UI
    //public void SetHand(Sprite _itemSprite, int[] _indexArray)
    //{
    //    //Debug.Log("Setting hand");

    //    for (int i = 0; i < _indexArray.Length-1; i++)
    //    {
    //        handsList[_indexArray[i]].SetHand(true);    
    //    }

    //    soUI.TriggerItemHeldImage(_itemSprite, _indexArray);
    //}


    // Returns -1 if hands are full.  This only checks a hand but does not set anything
    //public int[] CheckHands(int _numberOfHandsNeeded)
    //{
    //    //Debug.Log("Checking hands with " + _numberOfHandsNeeded + " number of hands needed");

    //    int[] handsUsedIndexArray = new int[_numberOfHandsNeeded];

    //    //bool canUse = false;
    //    //handBehavior tempHand = null;

    //    // Set to -1 by default
    //    //int _tempIndex = -1;

    //    //Debug.Log("Hands Index Array has length of " + handsUsedIndexArray.Length);

    //    //Debug.Log("Hands List Count = " + handsList.Count);

    //    if(handsList == null)
    //    {
    //        Debug.Log("Hands List Null");
    //    }

    //    // -1 for the 0 offset
    //    for (int i = 0; i < handsList.Count; i++)
    //    {

    //        bool canUse = true;

    //       // Debug.Log("Running check on hand list index of " + i);

    //        // Checks if a hand is available to use
    //        if(!handsList[i].ReturnIsBeingUsed())
    //        {
    //            //canUse = true;
    //            //handsArray[i].SetHand(true);
    //            //handsArray[i].SetSprite(_itemSprite);
    //            //soUI.TriggerItemHeldImage(_itemSprite, i);
    //            //tempHand = handsArray[i];
    //            if(handsUsedIndexArray == null)
    //            {
    //                Debug.Log("HandsUsedIndexArray is null");
    //            }

    //            //Debug.Log("Temp hand set to hand number " + i);
    //            //_tempIndex = i;
    //            handsUsedIndexArray[i] = i;
    //            //return tempHand;
    //        }

    //        else
    //        {
    //            handsUsedIndexArray[i] = -1;
    //            soUI.ToggleControlsPopup("HANDS FULL");
    //            canUse = false;
    //        }


    //        //if(canUse == true)
    //        //{
    //         //   return null;
    //        //}
    //    }


    //    return handsUsedIndexArray;
    //}

    public void ToggleMovement(bool _isOn)
    {
        movementController.enabled = _isOn;
    }

    public void ToggleModelVisibility(bool _isOn)
    {
        model.SetActive(_isOn);
    }
}