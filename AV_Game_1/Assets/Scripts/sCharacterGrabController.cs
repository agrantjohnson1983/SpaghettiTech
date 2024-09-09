using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterGrabController : MonoBehaviour
{
    public SO_EventsUI soUI;

    GameObject interactiveObject;

    GameObject toolObject;

    iGrabbable grabbable;

    bool isGrabbing = false;

    bool canLetGo = false;

    float letGoDelayTime = 0f;

    public float throwPower = 10f;

    public Transform transformGrab;

    public string grabPopupText = "Press SPACE To Grab";

    //FixedJoint joint;

    //Transform oldTransform;

    // Start is called before the first frame update
    void Start()
    {
        //joint = GetComponent<ConfigurableJoint>();
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

            soUI.ToggleControlsPopup(grabPopupText, interactiveObject.transform.position + grabbable.ui_offset);

            grabbable = null;
            interactiveObject = null;
            isGrabbing = false;
            soUI.TriggerConnectionHeldImage(null);
        }
    }

    void HandleGrabbing(GameObject _collisionObj)
    {
        // Does quick check to see if grabbing so this doesn't change around

        if (_collisionObj.TryGetComponent<iGrabbable>(out iGrabbable _grabbable))
        {
            grabbable = _grabbable;

            if(!isGrabbing)
            soUI.ToggleControlsPopup(grabPopupText, _collisionObj.transform.position + grabbable.ui_offset);

            // Checks for input to start Grab
            if (!iGrabbable.IsGrabbed && !isGrabbing && Input.GetKey(KeyCode.Space))
            {
                grabbable.OnGrab();

                isGrabbing = true;

                soUI.ToggleControlsPopup(null, Vector3.zero);

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
    // checks object held and triggers event ui to show connection held image
    void PlugHoldCheck(GameObject collisionObj)
    {
        if (collisionObj.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        {
            //Debug.Log("Pluggable checked and firing UI event");

            soUI.TriggerConnectionHeldImage(_pluggable.connectionSprite);
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

                soUI.ToggleControlsPopup(null, Vector3.zero);
            }
        }
    }
}
