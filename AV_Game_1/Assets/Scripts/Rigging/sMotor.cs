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

    // Optional fixed point representing the physical ceiling/rig
    // mount the chain hangs from. If left unassigned, the chain's top
    // defaults to directly above the motor's ground position at
    // heightToRaise, so the chain visually shortens to nothing as the
    // motor reaches the ceiling.
    public Transform chainAnchorPoint;

    // Optional point marking exactly where on the motor model the
    // chain should visually start (e.g. a mounting bracket child
    // transform), in case the model's pivot is not at that spot.
    // Defaults to this transform if unassigned.
    public Transform chainRootPoint;

    Vector3 cachedChainTopPosition;

    //bool chainsUp = false;

    bool isMoving = false;

    bool isRigged = false;

    enum eMotorRaiseStage
    {
        AtGround,
        AtWorkingHeight,
        AtCeiling
    }

    eMotorRaiseStage currentStage = eMotorRaiseStage.AtGround;

    // Ground-level Y position recorded once the motor is actually
    // rigged into its setup spot (not at scene start, since it may
    // spawn somewhere other than its final ground/rig position).
    float groundHeight;

    bool hasRecordedGroundHeight;

    // Hooks additional motor-specific setup into the moment this piece
    // actually gets rigged (iRiggable.SetRigging already moved it to
    // its setup spot's position before setting IsSet = true). This is
    // where the chain becomes visible and the motor switches over to
    // being driven kinematically for the controlled raise.
    public override bool IsSet
    {
        get
        {
            return base.IsSet;
        }

        set
        {
            base.IsSet = value;

            if (value)
            {
                RecordGroundHeightIfNeeded();

                if (rb != null)
                {
                    // Once rigged, this motor is driven by MovePosition
                    // rather than physics forces. Kinematic is required
                    // for MovePosition to interpolate correctly and for
                    // the FixedJoint connecting the truss to this
                    // motor's Rigidbody (added later by
                    // ConnectMotorsToTruss) to reliably follow it,
                    // rather than fighting a dynamic body being
                    // teleported by a raw Transform assignment.
                    //
                    // SetRigging (iRiggable's default implementation)
                    // set constraints to FreezeAll when this motor was
                    // first placed. That was never cleared, and
                    // FreezePosition constraints were clamping/undoing
                    // MovePosition every step - the motor would appear
                    // to move mid-raise then snap back to its frozen
                    // position, dragging the jointed truss back with
                    // it. Must be cleared here.
                    rb.isKinematic = true;
                    rb.constraints = RigidbodyConstraints.None;
                }

                isRigged = true;

                SetChainsLine();

                if (lineChain != null)
                {
                    lineChain.enabled = true;
                }
            }
        }
    }

    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("[" + this.name + "] sMotor has no Rigidbody component - motor raise will not move it.", this);
        }

        lineChain = GetComponent<LineRenderer>();

        if (lineChain == null)
        {
            Debug.LogWarning("[" + this.name + "] sMotor has no LineRenderer component - chain visual will not show.", this);
        }
        else
        {
            // Hidden until the motor is actually rigged into a setup
            // spot - see IsSet override.
            lineChain.enabled = false;
        }

        sRiggingManager.riggingManger.motorList.Add(this);
    }

    // Update is called once per frame. The chain line is kept live
    // every frame once rigged (not just while actively moving), so it
    // is always correct regardless of what else might move the motor
    // or the anchor point, and works identically for both raising and
    // lowering.
    void Update()
    {
        if (isRigged && lineChain != null)
        {
            lineChain.SetPosition(0, GetChainRootPosition());

            // Anchor point itself could move if it is a transform the
            // level designer repositions - keep it live too rather than
            // relying only on the cached value from SetChainsLine.
            if (chainAnchorPoint != null)
            {
                lineChain.SetPosition(1, chainAnchorPoint.position);
            }
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
        Debug.Log("[" + this.name + "] StartMotorRaise called - currentStage = " + currentStage + ", isMoving = " + isMoving);

        if (rb == null)
        {
            Debug.LogWarning("[" + this.name + "] Cannot raise - no Rigidbody assigned.", this);
            return;
        }

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
                    Debug.Log("[" + this.name + "] Motor is already at ceiling height.");
                    break;
                }
        }
    }

    // Mirror of StartMotorRaise for the down control. From the ceiling,
    // drops back to working height. From working height, drops back to
    // ground. Already at ground is a no-op. Reuses the same movement
    // coroutine since it is just "move to an absolute target height",
    // regardless of direction.
    public void StartMotorLower()
    {
        Debug.Log("[" + this.name + "] StartMotorLower called - currentStage = " + currentStage + ", isMoving = " + isMoving);

        if (rb == null)
        {
            Debug.LogWarning("[" + this.name + "] Cannot lower - no Rigidbody assigned.", this);
            return;
        }

        if (isMoving)
        {
            return;
        }

        switch (currentStage)
        {
            case eMotorRaiseStage.AtCeiling:
                {
                    StartCoroutine(MotorRaiseMovement(workingHeight, eMotorRaiseStage.AtWorkingHeight));
                    break;
                }

            case eMotorRaiseStage.AtWorkingHeight:
                {
                    StartCoroutine(MotorRaiseMovement(0f, eMotorRaiseStage.AtGround));
                    break;
                }

            case eMotorRaiseStage.AtGround:
                {
                    Debug.Log("[" + this.name + "] Motor is already at ground.");
                    break;
                }
        }
    }

    // _targetHeight is an absolute height above the motor's ground
    // position (not a delta from the current position), so
    // workingHeight, heightToRaise, and ground (0) can all be authored
    // as simple "height above ground" values. Uses
    // Rigidbody.MovePosition rather than Transform assignment so the
    // FixedJoint connecting the truss to this motor follows correctly,
    // in both directions.
    IEnumerator MotorRaiseMovement(float _targetHeight, eMotorRaiseStage _resultingStage)
    {
        Debug.Log("[" + this.name + "] Moving motor toward " + _resultingStage);

        isMoving = true;

        float counter = 0f;

        Vector3 startPos = rb.position;

        Vector3 destination = new Vector3(startPos.x, groundHeight + _targetHeight, startPos.z);

        while (counter < motorRaiseTime)
        {
            rb.MovePosition(Vector3.Lerp(startPos, destination, (counter / motorRaiseTime)));

            counter += Time.deltaTime;

            yield return null;
        }

        rb.MovePosition(destination);

        currentStage = _resultingStage;
        isMoving = false;

        Debug.Log("[" + this.name + "] Reached " + _resultingStage);
    }

    void RecordGroundHeightIfNeeded()
    {
        if (!hasRecordedGroundHeight)
        {
            groundHeight = transform.position.y;
            hasRecordedGroundHeight = true;
        }
    }

    Vector3 GetChainRootPosition()
    {
        return chainRootPoint != null ? chainRootPoint.position : this.gameObject.transform.position;
    }

    // Sets up the chain line: point 0 tracks the motor's current
    // (moving) position, point 1 is the fixed anchor at the top - either
    // chainAnchorPoint if assigned, or directly above the motor's ground
    // position at heightToRaise, so the chain visually shortens to zero
    // length as the motor reaches the ceiling.
    void SetChainsLine()
    {
        if (lineChain == null)
        {
            return;
        }

        lineChain.positionCount = 2;
        lineChain.useWorldSpace = true;

        cachedChainTopPosition = chainAnchorPoint != null
            ? chainAnchorPoint.position
            : new Vector3(transform.position.x, groundHeight + heightToRaise, transform.position.z);

        lineChain.SetPosition(0, GetChainRootPosition());
        lineChain.SetPosition(1, cachedChainTopPosition);
    }

    // This sets the height the motor will raise to (the final ceiling
    // height, not the working height)
    public void SetMotorRaiseHeight(float _amount)
    {
        heightToRaise = _amount;
    }
}