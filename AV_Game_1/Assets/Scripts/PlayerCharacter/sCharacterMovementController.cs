using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterMovementController : MonoBehaviour
{
    
    [Header("Movemement")]
    public float characterSpeed = 0.01f;
    private float characterStartingSpeed;
    private Vector2 inputVelocity;
    private Vector3 startingPosition;

    // TURNING
    private Vector3 direction;
    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;

    // DASH

    public float dashPower = 5;

    public GameObject characterModel;

    Rigidbody rb;

    [Header("Sprint")]
    public float sprintMultiplier = 2f;
    private bool isSprinting;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.6f;
    public LayerMask groundMask = -1;

    private RaycastHit groundHit;
    private bool isGrounded;

    //
    //GameObject grabObject;
    //bool isGrabbing = false;
    //public float throwPower = 10f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterStartingSpeed = characterSpeed;
        startingPosition = rb.position;
        inputVelocity = new Vector2();
        direction = new Vector3();

    }

    // Update is called once per frame
    void Update()
    {
        // Checks ground
        CheckGround();

        // Controller Input
        GetInput();

        // Stops Everything if Controller Not Moving
        if (inputVelocity.sqrMagnitude == 0)
        {
            /*
            if (rb.velocity == Vector3.zero)
            {
                Debug.Log("Velocity at 0");

                return;
            }
                

            else
            {
                Debug.Log("Deccelerating in progress.  Current velocity is " + rb.velocity.ToString());

                rb.velocity -= new Vector3(inputVelocity.x * characterSpeed *10f, 0f, inputVelocity.y * characterSpeed * 10f);
            }

            */

            rb.velocity = Vector3.zero;
            return;
        }


        float totalSpeed = characterSpeed;

        if (isSprinting)
        {
            totalSpeed *= sprintMultiplier;
        }

        // Turn Handler
        direction = new Vector3(inputVelocity.x, 0.0f, inputVelocity.y);
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // SMOOTH ANGLE - doesn't work for some reason - probably the current Velocity ref
        //var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        characterModel.transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);

        // Movement
        //rb.velocity = new Vector3(inputVelocity.x * totalSpeed, 0, inputVelocity.y * totalSpeed);

        // Movement for ramps/inclines

        Vector3 moveDirection = new Vector3(inputVelocity.x, 0f, inputVelocity.y);

        if (isGrounded)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, groundHit.normal);
        }

        rb.velocity = new Vector3(
            moveDirection.x * totalSpeed,
            rb.velocity.y,
            moveDirection.z * totalSpeed
            );
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            out groundHit,
            groundCheckDistance,
            groundMask);
    }

    void GetInput()
    {
        //CHARACTER MOVEMENT
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVelocity.Normalize();

        // Sprint
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        CheckDash();

        //CheckGrab();

        //CheckThrow();
    }

    void CheckDash()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Dash Triggered");

            //rb.AddForce(direction * dashPower, ForceMode.Impulse);
        }
    }

    /*
    void CheckGrab()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Off Grab Control Triggered");

            isGrabbing = false;
        }
    }

    void CheckThrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrabbing)
            {
                Debug.Log("Throw Control Triggered");

                isGrabbing = false;

                //grabObject.GetComponent<iGrabbable>().Throw();

                grabObject.transform.parent = null;

                Debug.Log("Throwing in " + direction.ToString() + " direction with power of " + throwPower.ToString());

                grabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                grabObject.GetComponent<Rigidbody>().AddForce(direction * throwPower, ForceMode.Impulse);
            }
        }
    }
    */

    /*
    private void OnCollisionEnter(Collision collision)
    {
        iGrabbable grabbable = collision.gameObject.GetComponent<iGrabbable>();

        if(grabbable != null)
        {
            Debug.Log("Rooster collided with grabbable object");
        }
    }
    */

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Rooster is colliding with " + collision.gameObject.name.ToString());
        /*
        iGrabbable grabbable = collision.gameObject.GetComponent<iGrabbable>();

        if (grabbable != null)
        {

            if (!isGrabbing && grabbable.isGrabbed)
            {
                Debug.Log("Rooster collided with grabbable object and let go of it");

                grabbable.OffGrab();

                grabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                grabObject.transform.parent = null;

                grabObject = null;

                return;
            }

            if (Input.GetMouseButton(1) && !grabbable.isGrabbed && !isGrabbing)
            {
                isGrabbing = true;

                grabObject = collision.gameObject;

                Debug.Log("Rooster collided with grabbable object and grabbed it");

                grabbable.OnGrab();

                grabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                grabObject.transform.parent = roosterModel.transform;

                return;
            }

            else
            {
                //isGrabbing = false;
            }
        }

        if (grabbable != null)
        {



        }
        */
    }
        
}
