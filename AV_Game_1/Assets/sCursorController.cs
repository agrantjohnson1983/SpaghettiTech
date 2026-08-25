using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sCursorController : MonoBehaviour
{
    public RectTransform cursor;

    private Vector2 cursorInput;

    public float cursorSpeed = 800f;

    PlayerInput playerInput;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput.currentControlScheme == "KeyboardAndMouse")
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        CursorHandler();
    }

    void CursorHandler()
    {
        Vector2 movement = cursorInput;

        if (movement.sqrMagnitude > 0.01f)
        {
            cursor.anchoredPosition +=
                movement * cursorSpeed * Time.unscaledDeltaTime;
        }
    }

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        cursorInput = context.ReadValue<Vector2>();
    }
}
