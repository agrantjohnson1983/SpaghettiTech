using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class sCharacterGrabController : MonoBehaviour
{
    sPlayerCharacter player;

    public GameObject model;

    public SO_EventsUI soUI;

    GameObject interactiveObject;

    GameObject toolObject;

    iGrabbable grabbable = null;

    public static bool isGrabbing = false;

    bool canLetGo = false;

    float letGoDelayTime = 0f;

    //public float throwPower = 10f;

    public Transform transformGrab;

    public string grabPopupText, throwPromptText;

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

    bool waitingForSpaceRelease = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<sPlayerCharacter>();
        //joint = GetComponent<ConfigurableJoint>();

        //HandIndexList = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForSpaceRelease && Input.GetKeyUp(KeyCode.Space))
        {
            waitingForSpaceRelease = false;
        }

        if(canLetGo)
        HandleGrabLetGo();
        HandleGrabToss();
    }

    void HandleGrabLetGo()
    {
        // This handles the "let go" part of the grabbing
        if (Input.GetKeyUp(KeyCode.Space) && isGrabbing)
        {
            //Debug.Log("Off Grab Control Triggered");
            GrabReset();

            canLetGo = false;
            waitingForSpaceRelease = true;
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

            if(interactiveObject.TryGetComponent<FixedJoint>(out FixedJoint _interactiveJoint))
            {
                // Destroys the joint component only
                Destroy(_interactiveJoint);
            }

            // De-selects the grabbable
            grabbable.OffSelect();

            // Triggers the off grab
            grabbable.OffGrab();

            // Sets the UI popup to null which turns it off
            //soUI.ToggleControlsPopup(null);

            // resets grabbable, interactive object to null and isGrabbing off
            grabbable = null;
            interactiveObject = null;
            isGrabbing = false;
            
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
            if(_grabbable == grabbable && isGrabbing)
            {
                //Debug.Log("Grab handler found itself and is already grabbing");
                return;
            }

            // Quick null check on the grabbable and interactive object
            if(grabbable != null && interactiveObject != null)
            {
                // Checks if the current grabbable is closer than the new one triggered and if so sets it as the new grabbable
                if (Vector3.Distance(this.gameObject.transform.position, interactiveObject.transform.position) > Vector3.Distance(this.gameObject.transform.position, _collisionObj.transform.position))
                {
                    //Debug.Log("New grabbale object is closer than the current grabbable");

                    // De-selects current grabbable
                    grabbable.OffSelect();

                    // Sets new grabbable
                    grabbable = _grabbable;
                    
                    // Sets interactive object
                    interactiveObject = _collisionObj;

                    // Selects the the grabbable
                    grabbable.OnSelect();
                }

                else
                {
                    //Debug.Log("Current grabbable object is closer");
                }
            }

            // This gets called if grabbable or interactive object is null
            else
            {
                //Debug.Log("Grabbable or Interactive Object Null - Setting to current collision object");

                grabbable = _grabbable;

                interactiveObject = _collisionObj;

                grabbable.OnSelect();
            }


            //bool bothHandsFree = true;
            int[] _tempIndexArray = new int[1] { -1 };

            //Debug.Log("Testttt");
            
            // Checks if the character is grabbing and if both hands are free and also if the grabbable can be grabbed
            if (!isGrabbing &&  _grabbable.CanBeGrabbed)
            {               
                //_tempIndexArray = GameManager.gm.ReturnCurrentPlayer().CheckHands(NumberOfHandsNeeded);

                //// iterates through the hands array.  If it returns less than 0 then....
                //for (int i = 0; i < _tempIndexArray.Length; i++)
                //{
                //    if (_tempIndexArray[i] < 0)
                //    {
                //        Debug.Log("Temp Index is too small at position " + i.ToString() + " with value of " + _tempIndexArray[i].ToString());
                //        bothHandsFree = false;
                //    }

                //    else
                //    {
                //        //Debug.Log("Temp index pos is greater than -1 at value of: " + _tempIndexArray[i]);
                //        //HandIndexList.Add(_tempIndexArray[i]);
                //    }
                //}

                //if (!bothHandsFree)
                //{
                //    //Debug.Log("Both hands not free");
                //}

                //else
                //{
                //    //Debug.Log("Both hands are free");
                //}

                //Debug.Log("Triggering Grab Popup Text");

                // Sets grab UI text
                soUI.ToggleControlsPopup(grabPopupText);
            }

            //else
            //{
            //    Debug.Log("Is already grabbing, both hands aren't free, or the grabbable can't be grabbed");
            //}

            //Debug.Log("Testttt");

            // Checks for input to start Grab, if the player is grabbing already, if both hands are free and if the grabbable object is grabbed
            if (!iGrabbable.IsGrabbed && !isGrabbing && !waitingForSpaceRelease && Input.GetKey(KeyCode.Space))
            {
                //Debug.Log("Grab Key Detected and can grab");

                // This gets called when the grabbable is first grabbed
                grabbable.OnGrab();

                // Turns off the select when grabbed?
                grabbable.OffSelect();

                // Toggles isGrabbing
                isGrabbing = true;

                //Debug.Log("Setting Hand to index of " + _tempIndex);

                // This sets the hand in the current player script to used
                //GameManager.gm.ReturnCurrentPlayer().SetHand(HandUseSprite, _tempIndexArray);

                // Creates a new list of integers based on the number of hands returned by the player
                //HandIndexList = new List<int>(_tempIndexArray);

                // Turns off the popup by sending a null
                soUI.ToggleControlsPopup(throwPromptText);

                // Sets the interactive object
                interactiveObject = _collisionObj;

                //Debug.Log("Character collided with grabbable object and grabbed it");

                // Temp RB for the player
                Rigidbody _playerRB;

                // Sets RB to this, which is on the player gameObject
                _playerRB = this.gameObject.GetComponent<Rigidbody>();

      
                    if (interactiveObject.TryGetComponent<Rigidbody>(out Rigidbody _rb))
                    {
                        //_rb.constraints = RigidbodyConstraints.FreezeAll;

                        //_rb.velocity = Vector3.zero;

                        //_joint.connectedBody = _rb;
                    }

                    if(interactiveObject.TryGetComponent<FixedJoint>(out FixedJoint _grabObjectJoint))
                    {
                        //_grabObjectJoint.
                    }

                    else
                    {

                    _grabObjectJoint = interactiveObject.AddComponent<FixedJoint>();

                    }

                    if (_grabObjectJoint.connectedBody == null)
                    {
                        _grabObjectJoint.connectedBody = _playerRB;
                        _grabObjectJoint.enablePreprocessing = false;
                    }
                   

                    else
                    {

                    }
                        //_grabObjectJoint.

                        //_grabObjectJoint.xMotion = ConfigurableJointMotion.Locked;
                        //_grabObjectJoint.yMotion = ConfigurableJointMotion.Locked;
                        //_grabObjectJoint.zMotion = ConfigurableJointMotion.Locked;
                        //_grabObject.transform.parent = _transform;

                        //_grabObject.transform.position 
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

    public bool ReturnIsGrabbing()
    {
        return isGrabbing;
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
    if (Input.GetMouseButtonDown(0) && isGrabbing)
    {
        TossGrabbedObject();
    }
}

void TossGrabbedObject()
{
    // Grab a reference to the rigidbody before GrabReset() clears interactiveObject
    if (interactiveObject != null && interactiveObject.TryGetComponent<Rigidbody>(out Rigidbody _grabbedRB))
    {
            // Cache the facing direction before GrabReset() runs
            Vector3 _tossDirection = model.transform.forward + model.transform.up * 0.5f;

        // Destroy the joint manually first so the object is free to fly
        if(interactiveObject.TryGetComponent<FixedJoint>(out FixedJoint _joint))
            {
                Destroy(_joint);
            }

        waitingForSpaceRelease = true;

        _grabbedRB.velocity = _tossDirection * throwPower;
        // If you want a little arc instead of a flat throw:
        // _grabbedRB.velocity += Vector3.up * (throwPower * 0.2f);

        // Reuses your existing cleanup: calls OffSelect/OffGrab, resets hands,
        // clears state. Runs immediately now instead of via a delayed Invoke,
        // so isGrabbing / iGrabbable.IsGrabbed can never get stranded true
        // (which happened if this GameObject was disabled - e.g. switching
        // characters - before the old 0.5s Invoke had a chance to fire).
        GrabReset();

        soUI.ToggleControlsPopup(null);
    }
    else
    {
        GrabReset();
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
                    Debug.Log("Trigger Exit from grabbing");
                    // Actively holding this object — don't clear references,
                    // just stop showing the popup. Let GrabReset() (on key-up)
                    // be the only thing that destroys the joint and clears state.
                    //soUI.ToggleControlsPopup(null);
                    return;
                }

                Debug.Log("Trigger Exit from grabbing");

                // Not grabbing yet, just hovered off it — safe to clear
                grabbable.OffSelect();
                grabbable = null;
                interactiveObject = null;
                soUI.ToggleControlsPopup(null);
            }
            else
            {
                _grabbable.OffSelect();
            }
        }
    }
}
