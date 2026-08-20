using UnityEngine;
using UnityEngine.InputSystem;

public class sPlayerInput : MonoBehaviour
{
    public InputAction Move { get; private set; }

    PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        Move = playerInput.actions["Move"];
    }
}