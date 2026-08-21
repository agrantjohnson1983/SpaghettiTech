using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class sCharacterMovementController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;
    [SerializeField] private InputActionReference dashAction;

    private InputAction moveInput, sprintInput, dashInput;

    PlayerInput playerInput;

    [Header("Movement")]
    public float characterSpeed = 5f;

    [Header("Sprint")]
    public float sprintMultiplier = 2f;
    private bool isSprinting;

    [Header("Ground")]
    public LayerMask groundMask = -1;
    public float groundCheckDistance = 0.6f;
    public float maxSlopeAngle = 45f;

    [Header("Gravity")]
    public float gravityMultiplier = 3f;
    public float stickToGroundForce = 10f;

    [Header("Dash")]
    public float dashPower = 5f;

    public GameObject characterModel;

    private Rigidbody rb;

    private Vector2 inputVelocity;
    private Vector3 direction;

    private bool isGrounded;
    private RaycastHit groundHit;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError(
                $"No PlayerInput found on {gameObject.name}",
                this);

            return;
        }

        moveInput = playerInput.actions.FindAction(moveAction.action.id);
        sprintInput = playerInput.actions.FindAction(sprintAction.action.id);
        dashInput = playerInput.actions.FindAction(dashAction.action.id);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        direction = Vector3.forward;
    }


    private void OnEnable()
    {
        if(playerInput != null)
        {
            moveInput.Enable();
            sprintInput.Enable();
            dashInput.Enable();

            dashInput.performed += OnDash;
        }

        
    }


    private void OnDisable()
    {
        dashAction.action.performed -= OnDash;

        moveAction.action.Disable();
        sprintAction.action.Disable();
        dashAction.action.Disable();
    }


    private void Update()
    {
        GetInput();
        CheckGround();
        RotateCharacter();
    }


    private void FixedUpdate()
    {
        MoveCharacter();

        // Extra gravity for snappier falling
        if (!isGrounded)
        {
            rb.AddForce(
                Physics.gravity * gravityMultiplier,
                ForceMode.Acceleration);
        }
        else
        {
            // Keeps player planted on ramps
            rb.AddForce(
                -transform.up * stickToGroundForce,
                ForceMode.Acceleration);
        }
    }


    private void GetInput()
    {
        inputVelocity = moveInput.ReadValue<Vector2>();

        // Prevent diagonal movement from being faster
        inputVelocity = Vector2.ClampMagnitude(inputVelocity, 1f);

        isSprinting = sprintInput.IsPressed();
    }


    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            out groundHit,
            groundCheckDistance,
            groundMask);
    }


    private void RotateCharacter()
    {
        if (inputVelocity.sqrMagnitude < 0.01f)
            return;

        direction = new Vector3(
            inputVelocity.x,
            0,
            inputVelocity.y);

        float targetAngle =
            Mathf.Atan2(direction.x, direction.z) *
            Mathf.Rad2Deg;

        characterModel.transform.rotation =
            Quaternion.Euler(0, targetAngle, 0);
    }


    private void MoveCharacter()
    {
        if (inputVelocity.sqrMagnitude < 0.001f)
        {
            rb.velocity = new Vector3(
                0,
                rb.velocity.y,
                0);

            return;
        }

        float speed = characterSpeed;

        if (isSprinting)
            speed *= sprintMultiplier;

        Vector3 moveDirection =
            new Vector3(
                inputVelocity.x,
                0,
                inputVelocity.y);

        if (isGrounded)
        {
            float slopeAngle =
                Vector3.Angle(Vector3.up, groundHit.normal);

            if (slopeAngle <= maxSlopeAngle)
            {
                moveDirection = Vector3.ProjectOnPlane(
                    moveDirection,
                    groundHit.normal);
            }
        }

        moveDirection.Normalize();

        rb.velocity = new Vector3(
            moveDirection.x * speed,
            rb.velocity.y,
            moveDirection.z * speed);
    }


    private void OnDash(InputAction.CallbackContext context)
    {
        // Uncomment when you implement dash
        // rb.AddForce(direction * dashPower, ForceMode.Impulse);
    }
}