using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterGrabController : MonoBehaviour, iRequireHands
{
    sPlayerCharacter player;

    public SO_EventsUI soUI;

    GameObject interactiveObject;

    GameObject toolObject;

    iGrabbable grabbable;

    bool isGrabbing = false;

    bool canLetGo = false;

    float letGoDelayTime = 0f;

    //public float throwPower = 10f;

    public Transform transformGrab;

    public string grabPopupText = "Press SPACE To Grab";

    public int _numberOfHandsNeeded;
    public int NumberOfHandsNeeded
    {
        get
        {
            return _numberOfHandsNeeded;
        }

        set
        {
            _numberOfHandsNeeded = value;
        }
    }

    List<int> _handIndexList;

    public List<int> HandIndexList
    {
        get
        {
            return _handIndexList;
        }

        set
        {
            _handIndexList = value;
        }
    }

    public Sprite _handUseSprite;

    public Sprite HandUseSprite
    {
        get
        {
            return _handUseSprite;
        }

        set
        {
            _handUseSprite = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<sPlayerCharacter>();
        //joint = GetComponent<ConfigurableJoint>();

        HandIndexList = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canLetGo)
        HandleGrabLetGo();
    }

    void HandleGrabLetGo()
    {
        // This handles the "let go" part of the grabbing
        if (Input.GetKeyUp(KeyCode.Space) && isGrabbing)
        {
            //Debug.Log("Off Grab Control Triggered");
            GrabReset();

            canLetGo = false;
        }
    }
    IEnumerator LetGoDelay()
    {
        float counter = 0;

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
                //_interactiveJoint.connectedBody = null;

                Destroy(_interactiveJoint);
                //_interactiveJoint.xMotion = ConfigurableJointMotion.Free;
                //_interactiveJoint.yMotion = ConfigurableJointMotion.Free;
                //_interactiveJoint.zMotion = ConfigurableJointMotion.Free;
            }

            grabbable.OffGrab();

            soUI.ToggleControlsPopup(null);

            grabbable = null;
            interactiveObject = null;
            isGrabbing = false;
            //soUI.TriggerConnectionHeldImage(null);

            // Checks the whole hand array and sends the indexes to let go back to the player
            for (int i = 0; i < HandIndexList.Count; i++)
            {
                // Resets Hand in player script
                GameManager.gm.ReturnCurrentPlayer().ResetHand(HandIndexList.ToArray());
            }

            HandIndexList = null;
        }
    }

    void HandleGrabbing(GameObject _collisionObj)
    {
        if (_collisionObj.TryGetComponent<iGrabbable>(out iGrabbable _grabbable))
        {
            grabbable = _grabbable;

            bool bothHandsFree = true;
            int[] _tempIndexArray = new int[1] { -1 };
            
            
            if (!isGrabbing && bothHandsFree && _grabbable.CanBeGrabbed)
            {               
                _tempIndexArray = GameManager.gm.ReturnCurrentPlayer().CheckHands(NumberOfHandsNeeded);

                for (int i = 0; i < _tempIndexArray.Length; i++)
                {
                    if (_tempIndexArray[i] < 0)
                    {
                        Debug.Log("Temp Index is too small at position " + i.ToString() + " with value of " + _tempIndexArray[i].ToString());
                        bothHandsFree = false;
                    }

                    else
                    {
                        //HandIndexList.Add(_tempIndexArray[i]);
                    }
                }

                if (!bothHandsFree)
                {
                    Debug.Log("Both hands not free");
                }

                // Sets grab UI text
                soUI.ToggleControlsPopup(grabPopupText);
            }


            // Checks for input to start Grab
            if (!iGrabbable.IsGrabbed && !isGrabbing && Input.GetKey(KeyCode.Space) && bothHandsFree)
            {
                grabbable.OnGrab();

                isGrabbing = true;

                //Debug.Log("Setting Hand to index of " + _tempIndex);

                // This sets the hand in the current player script to used
                GameManager.gm.ReturnCurrentPlayer().SetHand(HandUseSprite, _tempIndexArray);
                HandIndexList = new List<int>(_tempIndexArray);

                soUI.ToggleControlsPopup(null);

                //oldTransform = _collisionObj.transform;

                interactiveObject = _collisionObj;

                //Debug.Log("Character collided with grabbable object and grabbed it");

                

                //FixedJoint _playerJoint;
                //ConfigurableJoint _grabObjectJoint;

                Rigidbody _playerRB;

                //_playerJoint = this.gameObject.GetComponent<FixedJoint>();

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

                    //ConfigurableJoint _grabObjectJoint;

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

                //GetComponent<sCharacterActionController>().SetGrabbable(_grabbable);
                //if (!hasTool)
                //    ToolCheck(_collision);
                PlugHoldCheck(_collisionObj);

                StartCoroutine(LetGoDelay());

                return;
            }
        
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
        if(other.gameObject.TryGetComponent<iGrabbable>(out iGrabbable _grabbable) && grabbable != null)
        {
            if(_grabbable == grabbable)
            {
                //Debug.Log("Grabbable has exited trigger");

                soUI.ToggleControlsPopup(null);
            }
        }
    }
}
