using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class sCharacterGrabController : MonoBehaviour
{
    sPlayerCharacter player;

    public GameObject model;

    public SO_EventsUI soUI;

    GameObject interactiveObject;

    GameObject toolObject;

    iGrabbable grabbable = null;

    public static bool isGrabbing = false;

    bool isPressingGrab = false;

    bool canLetGo = false;

    float letGoDelayTime = 0f;

    //public float throwPower = 10f;

    public Transform transformGrab;

    private string grabControlText, throwControlText;

    public SO_VFXEventChannel soVFX;

    [Header("Input")]
    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference throwAction;

    private InputAction grabInput;
    private InputAction throwInput;

    PlayerInput playerInput;

    //public int _numberOfHandsNeeded;
    //public int NumberOfHandsNeeded
    //{
    //    get
    //    {
    //        return _numberOfHandsNeeded;
    //    }

    //    set
    //    {
    //        _numberOfHandsNeeded = value;
    //    }
    //}

    //List<int> _handIndexList;

    //public List<int> HandIndexList
    //{
    //    get
    //    {
    //        return _handIndexList;
    //    }

    //    set
    //    {
    //        _handIndexList = value;
    //    }
    //}

    //public Sprite _handUseSprite;

    //public Sprite HandUseSprite
    //{
    //    get
    //    {
    //        return _handUseSprite;
    //    }

    //    set
    //    {
    //        _handUseSprite = value;
    //    }
    //}

    // THROWING
    public float throwPower = 10f;

    bool waitingForGrabRelease = false;

    // Tracks the specific FixedJoint this script creates for a grab, so
    // cleanup only ever touches a joint this script owns. Never search the
    // object for "a" FixedJoint via TryGetComponent - objects like cable
    // pieces can already carry their own FixedJoint for unrelated purposes,
    // and that search has no way to tell the two apart.
    FixedJoint _grabJoint;

    void Awake()
    {
        player = GetComponentInParent<sPlayerCharacter>();

        playerInput = GetComponentInParent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError(
                $"No PlayerInput found on {gameObject.name}",
                this);

            return;
        }

        grabInput = playerInput.actions.FindAction(grabAction.action.id);
        throwInput = playerInput.actions.FindAction(throwAction.action.id);
    }

    // Start is called before the first frame update
    void Start()
    {
        grabControlText = grabInput.GetBindingDisplayString();
        throwControlText = grabInput.GetBindingDisplayString();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (grabInput != null)
        {
            grabInput.Enable();
            grabInput.performed += OnGrabInput;
            grabInput.canceled += OnGrabRelease;
        }

        if (throwInput != null)
        {
            throwInput.Enable();
            throwInput.performed += OnThrowInput;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (grabInput != null)
        {
            grabInput.performed -= OnGrabInput;
            grabInput.canceled -= OnGrabRelease;
            grabInput.Disable();
        }

        if (throwInput != null)
        {
            throwInput.performed -= OnThrowInput;
            throwInput.Disable();
        }
    }

    // isGrabbing is static, so it survives a scene load (only a full domain
    // reload clears it, not SceneManager.LoadScene). If a grab was active,
    // or mid-cleanup, when a scene change happened - e.g. GameManager.StartGig()
    // calling SceneManager.LoadScene - the old interactiveObject/joint get
    // destroyed by the unload before GrabReset() ever runs, and isGrabbing
    // is left stuck true forever, blocking every grab attempt from then on.
    // Force a clean reset on every scene load so nothing can carry over.
    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        isGrabbing = false;
        waitingForGrabRelease = false;
        canLetGo = false;
        grabbable = null;
        interactiveObject = null;
        _grabJoint = null;
    }

    private void OnGrabInput(InputAction.CallbackContext context)
    {
        // Don't allow a new grab until the previous grab input
        // has actually been released.
        if (waitingForGrabRelease)
            return;

        isPressingGrab = true;
    }

    private void OnGrabRelease(InputAction.CallbackContext context)
    {
        isPressingGrab = false;

        // The player has now physically released the grab button.
        // A new grab is allowed.
        waitingForGrabRelease = false;

        HandleGrabLetGo();
    }

    private void OnThrowInput(InputAction.CallbackContext context)
    {
        HandleGrabToss();
    }

    void HandleGrabLetGo()
    {
        isPressingGrab = false;

        if (canLetGo && isGrabbing)
        {
            GrabReset();

            canLetGo = false;
            waitingForGrabRelease = true;
        }
    }

    IEnumerator LetGoDelay()
    {
        float counter = 0;

        //soUI.ToggleControlsPopup(null);

        while (counter < letGoDelayTime)
        {

            counter += Time.deltaTime;

            yield return null;
        }

        canLetGo = true;
    }
    public void GrabReset()
    {
        //Debug.Log("Grab Reset Triggered on " + interactiveObject.name);

        if (interactiveObject != null && grabbable != null)
        {
            //Debug.Log("Grab Reset Triggered on " + interactiveObject.name);

            if (_grabJoint != null)
            {
                // Destroys only the joint this script created
                Destroy(_grabJoint);
                _grabJoint = null;
            }

            // De-selects the grabbable
            grabbable.OffSelect();
            SetObjectHighlight(interactiveObject, false);

            // Triggers the off grab
            grabbable.OffGrab();

            // Sets the UI popup to null which turns it off
            //soUI.ToggleControlsPopup(null);

            // resets grabbable, interactive object to null and isGrabbing off
            grabbable = null;
            interactiveObject = null;
            isGrabbing = false;

            soUI.TriggerControlsPopup("", "Throw");

            //// Checks if hand index list is null
            //if(HandIndexList != null)
            //{
            //    // Checks the whole hand array.  Minus 1 cause it's a list to array.
            //    for (int i = 0; i < HandIndexList.Count - 1; i++)
            //    {
            //        // Resets Hand in player script based on the hand index array
            //        GameManager.gm.ReturnCurrentPlayer().ResetHand(HandIndexList.ToArray());
            //    }

            //    //Resets hand index list to null
            //    HandIndexList = null;
            //}
        }
    }

    void HandleGrabbing(GameObject _collisionObj)
    {
        // Checks for a grabbable interface in collision
        if (_collisionObj.TryGetComponent<iGrabbable>(out iGrabbable _grabbable))
        {
            // If the collision is with the same grabbable then the function returns
            if (_grabbable == grabbable && isGrabbing)
            {
                //Debug.Log("Grab handler found itself and is already grabbing");
                return;
            }

            // Quick null check on the grabbable and interactive object
            if (grabbable != null && interactiveObject != null)
            {
                // Checks if the current grabbable is closer than the new one triggered and if so sets it as the new grabbable
                if (Vector3.Distance(this.gameObject.transform.position, interactiveObject.transform.position) > Vector3.Distance(this.gameObject.transform.position, _collisionObj.transform.position))
                {
                    //Debug.Log("New grabbale object is closer than the current grabbable");

                    // De-selects current grabbable
                    grabbable.OffSelect();
                    SetObjectHighlight(interactiveObject, false);

                    // Sets new grabbable
                    grabbable = _grabbable;

                    // Sets interactive object
                    interactiveObject = _collisionObj;

                    // Selects the the grabbable
                    grabbable.OnSelect();
                    SetObjectHighlight(interactiveObject, true);
                }
            }

            // This gets called if grabbable or interactive object is null
            else
            {
                //Debug.Log("Grabbable or Interactive Object Null - Setting to current collision object");

                grabbable = _grabbable;

                interactiveObject = _collisionObj;

                grabbable.OnSelect();
                SetObjectHighlight(interactiveObject, true);
            }

            int[] _tempIndexArray = new int[1] { -1 };

            //Debug.Log("Testttt");

            // Checks if the character is grabbing and if both hands are free and also if the grabbable can be grabbed
            if (!isGrabbing && _grabbable.CanBeGrabbed)
            {
                // Sets grab UI text
                soUI.TriggerControlsPopup(grabControlText, "Grab");
            }

            // Checks for input to start Grab, if the player is grabbing already, if both hands are free and if the grabbable object is grabbed
            if (!iGrabbable.IsGrabbed && !isGrabbing && !waitingForGrabRelease && isPressingGrab)
            {
                //Debug.Log("Grab Key Detected and can grab");

                // This gets called when the grabbable is first grabbed
                grabbable.OnGrab();

                // Turns off the select when grabbed?
                grabbable.OffSelect();
                SetObjectHighlight(interactiveObject, false);

                // Toggles isGrabbing
                isGrabbing = true;

                //Debug.Log("Setting Hand to index of " + _tempIndex);

                // This sets the hand in the current player script to used
                //GameManager.gm.ReturnCurrentPlayer().SetHand(HandUseSprite, _tempIndexArray);

                // Creates a new list of integers based on the number of hands returned by the player
                //HandIndexList = new List<int>(_tempIndexArray);

                soUI.TriggerControlsPopup("", "Grab");

                // Turns off the popup by sending a null
                soUI.TriggerControlsPopup(throwControlText, "Throw");

                // Sets the interactive object
                interactiveObject = _collisionObj;

                //Debug.Log("Character collided with grabbable object and grabbed it");

                // Temp RB for the player
                Rigidbody _playerRB;

                // Sets RB to this, which is on the player gameObject
                _playerRB = this.gameObject.GetComponentInParent<Rigidbody>();


                if (interactiveObject.TryGetComponent<Rigidbody>(out Rigidbody _rb))
                {
                    //_rb.constraints = RigidbodyConstraints.FreezeAll;

                    //_rb.velocity = Vector3.zero;

                    //_joint.connectedBody = _rb;
                }

                // Always create a fresh joint dedicated to this grab rather
                // than searching the object for an existing FixedJoint - on
                // cable pieces that already carry their own FixedJoint for
                // the cable system's own connections, that search could
                // grab the wrong joint and overwrite or destroy it later.
                if (_grabJoint != null)
                {
                    Destroy(_grabJoint);
                    _grabJoint = null;
                }

                StartCoroutine(GrabMovement());

                _grabJoint = interactiveObject.AddComponent<FixedJoint>();
                _grabJoint.connectedBody = _playerRB;
                _grabJoint.enablePreprocessing = false;

                
            }

            else
            {
                //Debug.Log("No Key input working");
            }

            //Debug.Log("Mid Test");

            //GetComponent<sCharacterActionController>().SetGrabbable(_grabbable);
            //if (!hasTool)
            //    ToolCheck(_collision);

            PlugHoldCheck(_collisionObj);

            StartCoroutine(LetGoDelay());

            //return;
        }

        else

        {
            //Debug.Log("No keyboard input, grabbable is already grabbed or is already grabbing or both hands aren't free");
        }

        //Debug.Log("End of Handle Grabbing Function");
    }

    IEnumerator GrabMovement()
    {
        soVFX?.Raise("Grab", interactiveObject.transform.position, Quaternion.identity);

        Vector3 startingPos = player.model.transform.position;
        Vector3 endPos = interactiveObject.transform.position;

        Vector3 localPos = player.model.transform.localPosition;

        Quaternion startingRot = player.transform.rotation;
        Quaternion endRot = transformGrab.rotation;

        float counter = 0f;

        Transform _targetTransform = interactiveObject.transform;

        while (counter < 0.25f)
        {
            player.model.transform.position = Vector3.Lerp(startingPos, endPos, (counter / 0.25f));
            player.model.transform.LookAt(_targetTransform);

            counter += Time.deltaTime;

            yield return null;
        }

        player.model.transform.localPosition = localPos;
    }
    public bool ReturnIsGrabbing()
    {
        return isGrabbing;
    }

    // Toggles the sObjectHighlighter component on a grabbable object, if it has one.
    // Safe to call on objects that do not have a highlighter attached.
    void SetObjectHighlight(GameObject _obj, bool _on)
    {
        if (_obj != null && _obj.TryGetComponent<sObjectHighlighter>(out sObjectHighlighter _highlighter))
        {
            _highlighter.SetHighlight(_on);
        }
    }

    // checks object held and triggers event ui to show connection held image
    void PlugHoldCheck(GameObject collisionObj)
    {
        if (collisionObj.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        {
            //Debug.Log("Pluggable checked and firing UI event");

            //soUI.TriggerItemHeldImage(_pluggable.connectionSprite);
        }
    }

    void HandleGrabToss()
    {
        if (isGrabbing)
        {
            TossGrabbedObject();
        }
    }

    void TossGrabbedObject()
    {
        if (interactiveObject != null &&
            interactiveObject.TryGetComponent<Rigidbody>(out Rigidbody _grabbedRB))
        {
            Vector3 _tossDirection =
                model.transform.forward +
                model.transform.up * 0.5f;

            if (_grabJoint != null)
            {
                Destroy(_grabJoint);
                _grabJoint = null;
            }

            // The current grab input must be released before another
            // object can be grabbed.
            waitingForGrabRelease = true;

            GrabReset();

            soVFX?.Raise("Throw", transform.position, Quaternion.identity);

            _grabbedRB.AddForce(
                _tossDirection * throwPower,
                ForceMode.Impulse);

            _grabbedRB.AddTorque(
                (model.transform.right + model.transform.up) * throwPower,
                ForceMode.Impulse);
        }
        else
        {
            GrabReset();

            // Still require the player to release the grab button.
            waitingForGrabRelease = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleGrabbing(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleGrabbing(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<iGrabbable>(out iGrabbable _grabbable) && grabbable != null)
        {
            if (_grabbable == grabbable)
            {
                if (isGrabbing)
                {
                    //Debug.Log("Trigger Exit from grabbing");
                    // Actively holding this object - don't clear references,
                    // just stop showing the popup. Let GrabReset() (on key-up)
                    // be the only thing that destroys the joint and clears state.
                    //soUI.ToggleControlsPopup(null);
                    return;
                }

                //Debug.Log("Trigger Exit from grabbing");

                // Not grabbing yet, just hovered off it - safe to clear
                grabbable.OffSelect();
                SetObjectHighlight(interactiveObject, false);
                grabbable = null;
                interactiveObject = null;
                soUI.TriggerControlsPopup("", "Grab");
            }
            else
            {
                _grabbable.OffSelect();
                SetObjectHighlight(other.gameObject, false);
            }
        }
    }
}