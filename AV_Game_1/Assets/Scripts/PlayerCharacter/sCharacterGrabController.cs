using System.Collections;
using System.Collections.Generic;
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

    public bool isGrabbing = false;

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

    // Objects currently overlapping this player's grab trigger that
    // implement iGrabbable. Populated and cleared ONLY by OnTriggerEnter and
    // OnTriggerExit - never by OnTriggerStay. PhysX stops calling
    // OnTriggerStay once a pair of colliders settles and the non-player body
    // goes to sleep, which is the normal resting state for most grabbable
    // rig pieces. That meant the old HandleGrabbing() (called from
    // OnTriggerStay) silently stopped being invoked at all the moment the
    // player stopped moving next to an object - the grab button worked, the
    // "already grabbed" and "can grab" checks worked, nothing was ever
    // running them. Grab input is now checked every Update() against
    // whatever is currently in this list, so it no longer depends on
    // physics re-evaluating the overlap on every tick.
    private List<GameObject> objectsInRange = new List<GameObject>();

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
        throwControlText = throwInput.GetBindingDisplayString();

        Debug.Log("Grab: " + grabControlText);
        Debug.Log("Throw: " + throwControlText);
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

    // isGrabbing is a normal per-instance field (one sCharacterGrabController
    // per player), so a freshly spawned player naturally starts with it
    // false - it does not need this reset to be correct on its own. This
    // reset instead covers the case where a scene load happens WHILE a grab
    // or its cleanup is in progress on a player instance that survives the
    // load (e.g. GameManager.StartGig() calling SceneManager.LoadScene
    // mid-grab): the old interactiveObject/joint get destroyed by the
    // unload before GrabReset() ever runs, which would otherwise leave
    // isGrabbing stuck true and block every further grab attempt for that
    // player. Force a clean reset on every scene load so nothing carries
    // over.
    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        isGrabbing = false;
        waitingForGrabRelease = false;
        canLetGo = false;
        grabbable = null;
        interactiveObject = null;
        _grabJoint = null;
        objectsInRange.Clear();
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
        Debug.Log("Throw input");

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
        // Save these before clearing anything.
        iGrabbable previousGrabbable = grabbable;
        GameObject previousObject = interactiveObject;

        // Destroy only our grab joint.
        if (_grabJoint != null)
        {
            Destroy(_grabJoint);
            _grabJoint = null;
        }

        // Clear our grab state FIRST so callbacks cannot see us
        // as still actively grabbing.
        grabbable = null;
        interactiveObject = null;
        isGrabbing = false;
        canLetGo = false;

        // Block a new grab until the button is physically released and
        // pressed again. Without this, tossing while still holding the grab
        // button (the normal way to throw, since grab is hold-to-carry)
        // creates a one-frame window where Update() sees isGrabbing already
        // false, the object still selectable, and isPressingGrab still true
        // - and grabs it right back before the toss force is ever applied.
        waitingForGrabRelease = true;

        // Tell the object the grab ended.
        if (previousGrabbable != null)
        {
            previousGrabbable.OffGrab();
        }

        // Clean up highlight.
        if (previousObject != null)
        {
            SetObjectHighlight(previousObject, false);
        }

        // Clear both controls.
        soUI.TriggerControlsPopup("", "Grab");
        soUI.TriggerControlsPopup("", "Throw");
    }

    void Update()
    {
        // Drop any entries that were destroyed while still "in range"
        // (consumed, despawned, etc.) instead of waiting for an
        // OnTriggerExit that will never come for a destroyed object.
        objectsInRange.RemoveAll(obj => obj == null);

        // While actively holding something, leave selection alone - the
        // held object stays selected until GrabReset() runs (on release or
        // toss).
        if (isGrabbing)
            return;

        GameObject nearestObj = FindNearestGrabbable();

        if (nearestObj != interactiveObject)
        {
            UpdateSelection(nearestObj);
        }

        if (grabbable == null || interactiveObject == null)
            return;

        PlugHoldCheck(interactiveObject);

        // Checks if the character is grabbing and if the grabbable can be
        // grabbed
        if (!isGrabbing && grabbable.CanBeGrabbed)
        {
            // Sets grab UI text
            soUI.TriggerControlsPopup(grabControlText, "Grab");
        }

        // Checks for input to start Grab, if the player isn't already
        // grabbing, and if this specific grabbable isn't already held by
        // someone else.
        if (!grabbable.IsGrabbed && !isGrabbing && !waitingForGrabRelease && isPressingGrab && grabbable.CanBeGrabbed)
        {
            PerformGrab(grabbable, interactiveObject);
        }
    }

    // Picks the closest currently-tracked object that still implements
    // iGrabbable. Distance is the only tie-breaker, matching the original
    // "closer object wins" behavior.
    GameObject FindNearestGrabbable()
    {
        GameObject nearest = null;
        float nearestDist = float.MaxValue;

        foreach (GameObject obj in objectsInRange)
        {
            if (!obj.TryGetComponent<iGrabbable>(out _))
                continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);

            if (dist < nearestDist)
            {
                nearest = obj;
                nearestDist = dist;
            }
        }

        return nearest;
    }

    // Moves selection (highlight + OnSelect/OffSelect + UI popup) from
    // whatever is currently selected to newObj, or clears selection
    // entirely if newObj is null.
    void UpdateSelection(GameObject newObj)
    {
        if (grabbable != null && interactiveObject != null)
        {
            grabbable.OffSelect();
            SetObjectHighlight(interactiveObject, false);
        }

        if (newObj == null)
        {
            grabbable = null;
            interactiveObject = null;
            soUI.TriggerControlsPopup("", "Grab");
            return;
        }

        newObj.TryGetComponent<iGrabbable>(out grabbable);
        interactiveObject = newObj;

        grabbable.OnSelect(player);
        SetObjectHighlight(interactiveObject, true);
    }

    // Everything that happens the moment a grab is actually taken - was
    // previously inline inside HandleGrabbing's if-block.
    void PerformGrab(iGrabbable _grabbable, GameObject _collisionObj)
    {
        // This gets called when the grabbable is first grabbed
        _grabbable.OnGrab(player);

        // Turns off the select when grabbed
        _grabbable.OffSelect();
        SetObjectHighlight(_collisionObj, false);

        // Toggles isGrabbing
        isGrabbing = true;

        soUI.TriggerControlsPopup("", "Grab");

        // Turns off the popup by sending a null
        soUI.TriggerControlsPopup(throwControlText, "Throw");

        // Sets the interactive object
        interactiveObject = _collisionObj;

        // Temp RB for the player
        Rigidbody _playerRB;

        // Sets RB to this, which is on the player gameObject
        _playerRB = this.gameObject.GetComponentInParent<Rigidbody>();

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

        // Starts the release-delay only now, once a grab has actually
        // happened - previously this coroutine was started unconditionally
        // on every single trigger tick (even while nothing was grabbed),
        // which spun up a new coroutine every physics tick for no reason.
        canLetGo = false;
        StartCoroutine(LetGoDelay());
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
        Debug.Log("Grab Toss control pressed");

        if (isGrabbing)
        {
            StartCoroutine(TossGrabbedObject());
        }
    }

    IEnumerator TossGrabbedObject()
    {
        Debug.Log("Starting grabb toss sequence");

        if (interactiveObject == null ||
            !interactiveObject.TryGetComponent<Rigidbody>(out Rigidbody grabbedRB))
        {
            GrabReset();
            yield break;
        }

        // Save what we need before cleanup.
        Vector3 tossDirection =
            (model.transform.forward + model.transform.up * 0.5f).normalized;

        // End the grab first.
        GrabReset();

        // Wait until the joint destruction has been processed by physics.
        yield return new WaitForFixedUpdate();

        // Now the object is completely free.
        grabbedRB.AddForce(
            tossDirection * throwPower,
            ForceMode.Impulse);

        grabbedRB.AddTorque(
            (model.transform.right + model.transform.up).normalized * throwPower,
            ForceMode.Impulse);

        soVFX?.Raise("Throw", transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<iGrabbable>(out _) && !objectsInRange.Contains(other.gameObject))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInRange.Remove(other.gameObject);

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