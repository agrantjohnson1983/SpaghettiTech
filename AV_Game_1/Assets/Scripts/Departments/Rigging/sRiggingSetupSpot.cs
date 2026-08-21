using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class sRiggingSetupSpot : sSetupSpotBASE
{
    public eTypeRigSetup rigType;

    // Index into sRiggingManager.trussSetupLocations that this spot
    // represents, when rigType == truss. Assigned dynamically by
    // sRiggingManager at spawn time in SpawnSetupObjects. Used to
    // register this slot as filled once its truss piece is rigged.
    public int setupIndex = -1;

    // Indices into sRiggingManager.trussSetupLocations for the two
    // neighboring truss slots that must be rigged before this spot's
    // action (actionObject, which for bolt spots is the bolt minigame
    // canvas itself) can activate. Only relevant when rigType == bolts.
    // Assigned dynamically by sRiggingManager at spawn time, since bolt
    // setup spots are instantiated at runtime and cannot hold a direct
    // inspector reference to truss pieces that do not exist yet.
    [Header("Neighbor Truss Requirement (assigned dynamically by sRiggingManager)")]
    public bool requireNeighborsSetup = true;

    public int neighborTrussIndexA = -1;

    public int neighborTrussIndexB = -1;

    // Tracked so a truss piece finishing while the player is already
    // standing in this trigger zone can still activate the spot, since
    // OnTriggerEnter will not fire again on its own in that case.
    bool isPlayerInsideTrigger = false;

    GameObject cachedPlayerObject;

    // For gear types that snap into place physically but need a
    // follow-up minigame before rigging is truly complete (lights, and
    // later speakers/screens) - remembers which physical object was
    // placed here so FinishSetup can call RiggingObjectComplete on it
    // once the minigame finishes.
    GameObject placedGearObject;

    // World-space Y offset applied only while this spot is active
    // (rigType == light), so it visually sits below the truss body
    // (e.g. where a light hangs from a yoke) rather than at whatever
    // local position it was authored at inside the truss prefab.
    // Negative values move it down. Applied fresh from
    // originalLocalPosition every time SetOverheadGearActive runs, so
    // repeated toggles never compound the offset.
    [Header("Overhead Gear Positioning")]
    public float activeWorldYOffset = -1f;

    Vector3 originalLocalPosition;
    bool hasRecordedOriginalLocalPosition;



    public void Start()
    {
        if (textSetup != null)
            textSetup.gameObject.SetActive(false);

        sRiggingManager.riggingMangerGlobal.RegisterRiggingSetup(rigType, this.gameObject);

        if (rigType == eTypeRigSetup.light)
        {
            RecordOriginalLocalPositionIfNeeded();

            sRiggingManager.riggingMangerGlobal.RegisterOverheadGearSpot(this);
        }
    }

    void RecordOriginalLocalPositionIfNeeded()
    {
        if (!hasRecordedOriginalLocalPosition)
        {
            originalLocalPosition = transform.localPosition;
            hasRecordedOriginalLocalPosition = true;
        }
    }

    // Called by sRiggingManager instead of raw gameObject.SetActive, so
    // the world-space offset is applied/removed alongside visibility.
    // Always resets to the original authored local position first, then
    // (if activating) translates down in world space - this way toggling
    // on and off repeatedly (raise, lower, raise again) never stacks the
    // offset on top of itself.
    public void SetOverheadGearActive(bool _active)
    {
        RecordOriginalLocalPositionIfNeeded();

        transform.localPosition = originalLocalPosition;

        if (_active)
        {
            transform.Translate(0f, activeWorldYOffset, 0f, Space.World);
        }

        gameObject.SetActive(_active);
    }

    public override void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //base.TriggerAction();

        if (!NeighborsAreReady())
        {
            Debug.Log("Neighboring truss pieces are not rigged yet - cannot start bolt setup");

            if (textSetup != null)
            {
                textSetup.gameObject.SetActive(true);
                textSetup.SetText("NEIGHBORING TRUSS NOT SET");
                textSetup.color = Color.red;
            }

            return;
        }

        if (CanTriggerAction && hasAction)
        {
            //StartCoroutine(ActionTasking());

            StartAction();

            //soUI.TriggerItemHeldImage(_itemData.itemSprite, );
        }

        else
        {
            Debug.Log("Cant Trigger Action");
        }
    }

    // Checks the two neighbor truss slot indices via sRiggingManager.
    // Only applies to bolt spots. Fails open (returns true) only if the
    // indices were never assigned at all (-1), meaning something
    // upstream forgot to wire them - logged as a warning so it gets
    // caught rather than silently blocking forever.
    bool NeighborsAreReady()
    {
        if (!requireNeighborsSetup || rigType != eTypeRigSetup.bolts)
        {
            return true;
        }

        if (neighborTrussIndexA < 0 || neighborTrussIndexB < 0)
        {
            Debug.LogWarning("sRiggingSetupSpot neighbor truss indices not assigned on " + this.name, this);
            return true;
        }

        return sRiggingManager.riggingMangerGlobal.AreNeighboringTrussSet(neighborTrussIndexA, neighborTrussIndexB);
    }

    // Gates gear that mounts onto the truss itself (lights, and later
    // speakers/screens) behind the truss being raised to working height
    // or above. A spot for any other rig type is unaffected.
    //
    // NOTE: the call site for this in SetupGear is currently commented
    // out (see below) - flagging in case that was left off from testing
    // rather than intentionally, since it means lights can currently be
    // mounted at any truss height, not just working height or above.
    bool TrussReadyForOverheadGear()
    {
        if (rigType != eTypeRigSetup.light)

        {
            return true;
        }

        // UCOMMENT THIS

        Debug.LogWarning("NEED TO UN-COMMENT CODE in Rigging setup spot");

        if (sRiggingManager.riggingMangerGlobal.debugBypassLightHeightGate)
        {
            return true;
        }

        return sRiggingManager.riggingMangerGlobal.AreAllMotorsAtOrAboveWorkingHeight();
    }

    // Gate that actually controls whether actionObject (the bolt
    // minigame canvas, for bolt spots) is allowed to switch on. Every
    // outcome is logged distinctly so it is obvious from the console
    // which gate is actually blocking activation.
    void TryActivateActionObject(GameObject _playerObj)
    {
        if (!NeighborsAreReady())
        {
            Debug.Log("[" + this.name + "] Blocked - neighboring truss pieces not rigged yet");

            if (textSetup != null)
            {
                textSetup.gameObject.SetActive(true);
                textSetup.SetText("NEIGHBORING TRUSS NOT SET");
                textSetup.color = Color.red;
            }

            return;
        }

        if (ToolTypeNeeded != eToolType.NONE && ToolCheck(_playerObj) == false)
        {
            Debug.Log("[" + this.name + "] Blocked - player missing required tool (" + ToolTypeNeeded + ")");
            return;
        }

        if (rigType == eTypeRigSetup.bolts && sRiggingManager.riggingMangerGlobal.debugBypassBoltMinigame)
        {
            Debug.Log("[" + this.name + "] DEBUG bypass enabled - skipping bolt minigame, completing instantly");
            FinishSetup();
            return;
        }

        Debug.Log("[" + this.name + "] Neighbors ready and tool check passed - activating action object");
        actionObject.SetActive(true);
    }

    // Called by sRiggingManager after a truss slot this spot depends on
    // gets registered, in case the player is already standing in the
    // trigger zone waiting on it.
    public void RecheckNeighborReadiness()
    {
        if (!isPlayerInsideTrigger || cachedPlayerObject == null)
        {
            return;
        }

        TryActivateActionObject(cachedPlayerObject);
    }

    public override void StopAction()
    {
        base.StopAction();

        //Debug.Log("Stopping Action Tasking Cortoutine");

        //StopCoroutine(ActionTasking());

        //StopAllCoroutines();

        //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);

        soUI.TriggerTaskGauge(0, Vector3.zero);
    }

    void StartAction()
    {
        //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(false);
    }

    IEnumerator ActionTasking()
    {
        Debug.Log("Starting Action Tasking");



        //soUI.TriggerTaskGauge(TaskTime, this.transform.position + ui_offset);

        yield return new WaitForSeconds(TaskTime);

        //FinishSetup();
    }

    public override void FinishSetup()
    {
        Debug.Log("[" + this.name + "] FinishSetup called - Action Task Complete");

        if (actionObject != null)
        {
            actionObject.SetActive(false);
        }

        if (rigType == eTypeRigSetup.bolts)
        {
            Debug.Log("[" + this.name + "] FinishSetup step 1 - welding truss neighbors "
                + neighborTrussIndexA + " and " + neighborTrussIndexB);

            // Bolting complete - physically weld the two truss pieces
            // this spot connects, anchored at the bolt spot itself.
            sRiggingManager.riggingMangerGlobal.WeldTrussNeighbors(neighborTrussIndexA, neighborTrussIndexB, this.transform.position);

            Debug.Log("[" + this.name + "] FinishSetup step 1 complete");
        }

        if (placedGearObject != null && placedGearObject.TryGetComponent(out iRiggable placedRiggable))
        {
            // A gear object (e.g. a light) was snapped into place
            // earlier but had its completion deferred until now -
            // RiggingObjectComplete handles RigSet, weight tracking,
            // and marking it Enabled = false all at once.
            Debug.Log("[" + this.name + "] FinishSetup step 2 - completing rigging for " + placedGearObject.name);

            placedRiggable.RiggingObjectComplete(placedGearObject, rigType);
        }
        else
        {
            Debug.Log("[" + this.name + "] FinishSetup step 2 - calling RigSet(" + rigType + ")");

            sRiggingManager.riggingMangerGlobal.RigSet(rigType);
        }

        Debug.Log("[" + this.name + "] FinishSetup step 2 complete");

        //GameManager.gm.ReturnCurrentPlayer().ToggleMovement(true);

        //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);

        Debug.Log("[" + this.name + "] FinishSetup step 3 complete - destroying setup spot");

        if (soAudio != null)
            soAudio.TriggerSFX("SetupComplete");

        if (soVFX != null)
            soVFX.Raise("StarburstSmall", this.transform.position + Vector3.up, Quaternion.identity);

        Destroy(this.gameObject, 0.25f);
    }

    // Wire this to an "Exit"/"Leave" button inside the minigame canvas.
    // Unlike FinishSetup, this does NOT destroy the spot, call RigSet,
    // or mark anything complete - it just closes the UI and gives
    // movement back, so the player can go collect more bolts/nuts and
    // come back later. Whatever progress exists on each sBoltHoleSlot
    // (or on placedGearObject for a light) is untouched, since that
    // state lives on those objects directly rather than being reset
    // here.

    void SetupGear(iRiggable _riggable, GameObject other)
    {
        //_riggable.IsSet = true;

        switch (_riggable.TypeRig)
        {
            case eTypeRigSetup.truss:

            case eTypeRigSetup.motor:

            case eTypeRigSetup.motorController:
                {
                    if (_riggable.Enabled)
                    {
                        Debug.Log(_riggable + " collision with Setup Spot");

                        _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                        _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                        StartCoroutine(ReleaseThenMove(other, this.gameObject.transform.position + offset, this.transform.rotation));
                    }
                    else
                    {
                        Debug.Log("[" + this.name + "] MotorController " + other.gameObject.name
                            + " collided but iRiggable.Enabled is false - setup spot will not complete");
                    }

                    break;
                }

            case eTypeRigSetup.light:
                {
                    if (!_riggable.Enabled)
                    {
                        Debug.Log("[" + this.name + "] Light " + other.gameObject.name
                            + " collided but iRiggable.Enabled is false - setup spot will not complete");
                        break;
                    }

                    if (!TrussReadyForOverheadGear())
                    {
                        Debug.Log("[" + this.name + "] Truss is not at working height yet - cannot mount light here.");
                        break;
                    }

                    Debug.Log("[" + this.name + "] Light snapped into place - starting crescent wrench tightening");

                    // Snap it into position, but do NOT call
                    // RiggingObjectComplete or destroy this spot
                    // yet - that is deferred to FinishSetup,
                    // called once the crescent wrench minigame
                    // (actionObject) reports completion.
                    StartCoroutine(ReleaseThenMove(other, this.gameObject.transform.position + offset, this.transform.rotation, false));

                    _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                    other.gameObject.GetComponent<sRigGear>().enabled = false;

                    placedGearObject = other.gameObject;

                    if (actionObject != null)
                    {
                        actionObject.SetActive(true);
                    }

                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenSet)
            return;

        //Debug.Log("Rigging Setup On Trigger Enter");

        if (HasAction)
        {
            isPlayerInsideTrigger = true;
            cachedPlayerObject = other.gameObject;

            TryActivateActionObject(other.gameObject);
        }

        if (other.TryGetComponent(out iRiggable _riggable))
        {
            // Checks that collided rig type is same as setup type
            if (_riggable.TypeRig == rigType && !_riggable.IsSet && !hasBeenSet)
            {
                hasBeenSet = true;

                // Do NOT set _riggable.IsSet here. SetRigging (called
                // inside SetupGear below) already sets IsSet = true
                // internally, but only AFTER it has repositioned the
                // object. Setting it here first meant sMotor's
                // RecordGroundHeightIfNeeded() (triggered by IsSet)
                // locked in the motor's pre-placement position - since
                // that guard only records once, the correct position
                // from SetRigging's own later IsSet=true never
                // overwrote it. groundHeight (and everything derived
                // from it: the chain's cached top anchor, raise/lower
                // target heights) ended up wrong as a result. The
                // !_riggable.IsSet check above still protects against
                // the same object colliding with a second setup spot,
                // since SetRigging sets it synchronously very early
                // within SetupGear, before that second spot's
                // OnTriggerEnter would run.
                SetupGear(_riggable, other.gameObject);
            }

            else
            {
                Debug.Log("[" + this.name + "] Wrong setup spot type - spot expects " + rigType
                    + " but " + other.gameObject.name + " is " + _riggable.TypeRig);
            }

        }
    }

    // TO DO - Check to see how close the rigging piece rotation is compared to the setup spot rotation




    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && HasAction)
        {
            isPlayerInsideTrigger = false;

            cachedPlayerObject = null;

            CanTriggerAction = false;

            if (IsDoingAction)
            {
                StopAction();
            }

            textSetup.gameObject.SetActive(false);
        }

    }
}