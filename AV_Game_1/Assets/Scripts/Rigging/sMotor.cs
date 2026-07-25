using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMotor : sRigGear
{
    Rigidbody rb;

    LineRenderer lineChain;

    // Total distance from ground to final ceiling position.
    public float heightToRaise = 50;

    // Distance from ground to the intermediate "working height" where
    // the truss is parked so lights and other gear can be rigged
    // before the final raise.
    public float workingHeight = 20;

    public float motorRaiseTime = 10f;

    public float motorRaiseSpeed = 10f;

    bool chainsUp = false;

    bool isMoving = false;

    enum eMotorRaiseStage
    {
        AtGround,
        AtWorkingHeight,
        AtCeiling
    }

    eMotorRaiseStage currentStage = eMotorRaiseStage.AtGround;

    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lineChain = GetComponent<LineRenderer>();

        sRiggingManager.riggingManger.motorList.Add(this);

        RecordGroundHeightIfNeeded();

        //SetChainsLine();

        //StartMotorRaise();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            lineChain.SetPosition(0, this.gameObject.transform.position);
        }
    }

    public void TriggerAction(GameObject _actionObj, eToolType _toolToUse)
    {

    }

    public void StopAction(GameObject _actionObj)
    {

    }

    // This handles the movement raising the motor. Called each time the
    // player triggers the motor control. First call raises to
    // workingHeight and stops there; a second call continues from there
    // up to heightToRaise (the ceiling). Further calls once at the
    // ceiling are no-ops.
    public void StartMotorRaise()
    {
        if (isMoving)
        {
            return;
        }

        switch (currentStage)
        {
            case eMotorRaiseStage.AtGround:
                {
                    StartCoroutine(MotorRaiseMovement(workingHeight, eMotorRaiseStage.AtWorkingHeight));
                    break;
                }

            case eMotorRaiseStage.AtWorkingHeight:
                {
                    StartCoroutine(MotorRaiseMovement(heightToRaise, eMotorRaiseStage.AtCeiling));
                    break;
                }

            case eMotorRaiseStage.AtCeiling:
                {
                    Debug.Log("Motor is already at ceiling height.");
                    break;
                }
        }
    }

    // _targetHeight is an absolute height above the motor's starting
    // ground position (not a delta from the current position), so
    // workingHeight and heightToRaise can both be authored as simple
    // "height above ground" values in the inspector.
    IEnumerator MotorRaiseMovement(float _targetHeight, eMotorRaiseStage _resultingStage)
    {
        Debug.Log("Raising Motor toward " + _resultingStage);

        isMoving = true;

        float counter = 0f;

        Vector3 startPos = rb.transform.position;

        Vector3 destination = new Vector3(startPos.x, groundHeight + _targetHeight, startPos.z);

        while (counter < motorRaiseTime)
        {
            rb.gameObject.transform.position = Vector3.Lerp(startPos, destination, (counter / motorRaiseTime));

            counter += Time.deltaTime;

            yield return null;
        }

        rb.gameObject.transform.position = destination;

        currentStage = _resultingStage;
        isMoving = false;
    }

    // Ground-level Y position recorded on first use as the reference
    // point that workingHeight/heightToRaise are measured from.
    float groundHeight;

    bool hasRecordedGroundHeight;

    void RecordGroundHeightIfNeeded()
    {
        if (!hasRecordedGroundHeight)
        {
            groundHeight = transform.position.y;
            hasRecordedGroundHeight = true;
        }
    }

    // This sets the chains to start before the motor moves
    void SetChainsLine()
    {
        lineChain.SetVertexCount(2);

        lineChain.SetPosition(0, this.gameObject.transform.position);
        lineChain.SetPosition(1, this.gameObject.transform.position + Vector3.up * 50f);
    }

    // This sets the height the motor will raise to (the final ceiling
    // height, not the working height)
    public void SetMotorRaiseHeight(float _amount)
    {
        heightToRaise = _amount;
    }
}