using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
//using UnityEngine.InputSystem;

public class sOrbitalCamera : MonoBehaviour
{
    public TextMeshProUGUI textEnemyName;
    public TextMeshProUGUI textEnemyDescription;
    public Image iEnemyIcon;

    public Transform target;      // The object to orbit around
    public float rotSpeed = 1f;   // Rotation speed
    public float xSpread = 5f;    // Spread in the x-axis
    public float zSpread = 5f;    // Spread in the z-axis
    public float yOffset = 2f;    // Vertical offset from the target
    public bool rotateClockwise = true;  // Direction of rotation

    private float timer = 0f;

    public float cameraTime = 10f;

    //PlayerInputs inputs;

    public bool cancelOnAnyKey;

    private void Awake()
    {
        //inputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        //inputs.Enable();

        //inputs.PlayerMovement.AnyKey.performed += CancelCinematic;
    }

    private void OnDisable()
    {
        //inputs.Disable();

        //inputs.PlayerMovement.AnyKey.performed -= CancelCinematic;
    }

    void FixedUpdate()
    {
        if (rotateClockwise)
        {
            float x = -Mathf.Cos(timer) * xSpread;
            float z = Mathf.Sin(timer) * zSpread;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + target.position;
        }

        else
        {
            float x = Mathf.Cos(timer) * xSpread;
            float z = Mathf.Sin(timer) * zSpread;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + target.position;
        }

        transform.LookAt(target);
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cameraTime)
        {
            //GameManager.gm.EndCinematic();

            //SetTarget(sRooster.roosterGlobal.transform);

            Destroy(this.gameObject);
        }
    }

    private void LateUpdate()
    {
        
    }


    public void SetTarget(Transform _target)
    {
        target = _target;
        //transform.position = _target.position + new Vector3(offset, 0f, offset);
    }

    public void SetEnemyStats(string _name, string _desciption, Sprite _sprite)
    {
        Debug.Log("Setting stats");

        textEnemyName.text = _name;
        textEnemyDescription.text = _desciption;
        iEnemyIcon.sprite = _sprite;

        Debug.Log("Stats setting complete");
    }

    /*
    void CancelCinematic(InputAction.CallbackContext context)
    {
        if(context.performed && cancelOnAnyKey)
        {
            Debug.Log("Canceling cinematic triggered");

            //GameManager.gm.EndCinematic();

            Destroy(this.gameObject);
        }
        
    }
    */
}
