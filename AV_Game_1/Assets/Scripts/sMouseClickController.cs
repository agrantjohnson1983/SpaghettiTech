using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMouseClickController : MonoBehaviour, iClickable
{
    Camera cam;
    public LayerMask mask_Clickable;
    bool isActive = false;
    sPlayerCharacter player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<sPlayerCharacter>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //handles the mouse clicking during update if active
        //if(isActive)
        HandleMouseClicking();   
    }

    void HandleMouseClicking()
    {
        // When a user clicks the left mouse button down and character isn't grabbing
        if (Input.GetMouseButtonDown(0) && !sCharacterGrabController.isGrabbing)
        {
            // creats a ray at the mouse position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // reference to a raycast hit
            RaycastHit hit;

            // shoots the ray out and outputs a reference to the hit.  Uses the Clickable layer for a layermask
            if(Physics.Raycast(ray, out hit, 100, mask_Clickable))
            {
                //Debug.Log(hit.transform.name.ToString() + " Was clicked by mouse");

                // TODO - This needs to check on instances with boxes if the player is in range to click/open - might happen on box side....

                // Checks if the raycast hits a clickable object and if so outputs a reference to the clickable
                if(hit.transform.gameObject.TryGetComponent<iClickable>(out iClickable _clickable))
                {
                    // Calls the OnClick method in the clickable interface
                    _clickable.OnClick();
                }
            }

        }
    }

    // Use this to turn on and off the click controller//
    public void ToggleMouseClickController(bool _isOn)
    {
        isActive = _isOn;
    }

    // This should change to player clicked on - This should maybe be moved to the sPlayer script, so that this script can be turned off for inactive characters
    public void OnClick()
    {
        Debug.Log(gameObject.name + " was clicked - this should switch to this player");

        // Turns off current player
        GameManager.gm.ReturnCurrentPlayer().CharacterControlsToggle(false);

        // Switches current player index to this one
        GameManager.gm.SwitchActivePlayerIndex(player.GetIndex());

        // Sets player that gets clicked to current one - also toggles on the controls
        player.SetToCurrentPlayer();

        // Sets Camera
        GameManager.gm.ReturnCanvasWorldSpace().worldCamera = Camera.main;
    }
}
