using UnityEngine;
using UnityEngine.InputSystem;

public class sMouseClickController : MonoBehaviour, iClickable
{
    [Header("Input")]
    [SerializeField] private InputActionReference clickAction;

    [Header("Click Settings")]
    [SerializeField] private LayerMask mask_Clickable;

    private sPlayerCharacter player;
    private Camera cam;


    private void Start()
    {
        player = GetComponent<sPlayerCharacter>();
    }


    private void OnEnable()
    {
        if (clickAction != null)
        {
            clickAction.action.Enable();
            clickAction.action.performed += OnClickInput;
        }
    }


    private void OnDisable()
    {
        if (clickAction != null)
        {
            clickAction.action.performed -= OnClickInput;
            clickAction.action.Disable();
        }
    }


    private void OnClickInput(InputAction.CallbackContext context)
    {
        HandleMouseClicking();
    }


    private void HandleMouseClicking()
    {
        // Don't allow clicking while the character is grabbing
        if (player.ReturnGrabController().isGrabbing)
            return;

        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("Camera.main was null - no clicky");
            return;
        }

        // Get the current mouse position from the new Input System
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Create a ray from the mouse position
        Ray ray = cam.ScreenPointToRay(mousePosition);

        // Shoot the ray using the Clickable layer mask
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask_Clickable))
        {
            // Check whether the hit object implements iClickable
            if (hit.transform.gameObject.TryGetComponent<iClickable>(
                out iClickable clickable))
            {
                // Call the clickable object's OnClick method
                clickable.OnClick();
            }
        }
    }


    // Use this to turn on and off the click controller
    public void ToggleMouseClickController(bool isOn)
    {
        if (clickAction == null)
            return;

        if (isOn)
            clickAction.action.Enable();
        else
            clickAction.action.Disable();
    }


    // Called when this player character itself is clicked
    public void OnClick()
    {
        Debug.Log(
            gameObject.name +
            " was clicked - this should switch to this player");

        //// Turns off current player
        //GameManager.gm.ReturnCurrentPlayer().CharacterControlsToggle(false);

        //// Switches current player index to this one
        //GameManager.gm.SwitchActivePlayerIndex(player.GetIndex());

        //// Sets player that gets clicked to current one - also toggles on the controls
        //player.SetToCurrentPlayer();

        //// Sets Camera
        //GameManager.gm.ReturnCanvasWorldSpace().worldCamera = Camera.main;
    }
}