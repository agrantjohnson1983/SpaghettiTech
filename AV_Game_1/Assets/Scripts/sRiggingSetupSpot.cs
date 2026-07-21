using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sRiggingSetupSpot : MonoBehaviour, iActionable
{
    public float _taskTime;

    public bool hasAction = false; // use this for turning on objects that have actions

    public Vector3 offset;

    bool _canTriggerAction; // this is for controls to check if an action can be triggered

    bool hasBeenSet = false;

    public eTypeRigSetup rigType;

    public bool toolNeeded = false;

    public eToolType _toolTypeNeeded;

    public TextMeshPro textSetup;

    public Vector3 _UI_offset;

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

    public Vector3 ui_offset
    {
        get
        {
            return _UI_offset;
        }

        set
        {
            _UI_offset = value;
        }
    }

    public bool HasAction
    {
        get
        {
            return hasAction;
        }

        set
        {
            hasAction = value;
        }
    }

    public float TaskTime
    {
        get
        {
            return _taskTime;
        }

        set
        {
            _taskTime = value;
        }
    }

    public bool CanTriggerAction
    {
        get
        {
            return _canTriggerAction;
        }

        set
        {
            _canTriggerAction = value;
        }
    }

    public bool IsDoingAction
    {
        get;
        set;
    }

    public eToolType ToolTypeNeeded
    {
        get
        {
            return _toolTypeNeeded;
        }

        set
        {
            _toolTypeNeeded = value;
        }
    }

    public SO_EventsUI soUI;

    public GameObject actionObject;

    private void Start()
    {
        if (textSetup != null)
            textSetup.gameObject.SetActive(false);
    }

    public void TriggerAction(GameObject _actionObj, SO_ItemData _itemData)
    {
        //base.TriggerAction(_actionObj, _toolToUse);

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

        return sRiggingManager.riggingManger.AreNeighboringTrussSet(neighborTrussIndexA, neighborTrussIndexB);
    }

    // Gate that actually controls whether actionObject (the bolt
    // minigame canvas, for bolt spots) is allowed to switch on.
    void TryActivateActionObject(GameObject _playerObj)
    {
        if (!NeighborsAreReady())
        {
            Debug.Log("Neighboring truss pieces are not rigged yet - blocking bolt minigame activation");

            if (textSetup != null)
            {
                textSetup.gameObject.SetActive(true);
                textSetup.SetText("NEIGHBORING TRUSS NOT SET");
                textSetup.color = Color.red;
            }

            return;
        }

        if (toolNeeded && ToolCheck(_playerObj) == false)
        {
            return;
        }

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

    public void StopAction()
    {
        Debug.Log("Stopping Action Tasking Cortoutine");

        StopCoroutine(ActionTasking());

        StopAllCoroutines();

        sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);

        soUI.TriggerTaskGauge(0, Vector3.zero);
    }

    void StartAction()
    {
        sPlayerCharacter.playerCharacterGlobal.ToggleMovement(false);
    }

    IEnumerator ActionTasking()
    {
        Debug.Log("Starting Action Tasking");



        //soUI.TriggerTaskGauge(TaskTime, this.transform.position + ui_offset);

        yield return new WaitForSeconds(TaskTime);

        //FinishSetup();
    }

    public void FinishSetup()
    {
        Debug.Log("Action Task Complete");

        //actionObject.SetActive(false);

        sRiggingManager.riggingManger.RigSet(rigType);

        //GameManager.gm.ReturnCurrentPlayer().ToggleMovement(true);

        sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);

        Destroy(this.gameObject);
    }

    public void WrongTool()
    {
        Debug.Log("Wrong Tool");

        textSetup.gameObject.SetActive(true);

        //textSetup.color = Color.red;

        CanTriggerAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rigging Setup On Trigger Enter");

        if (HasAction)
        {
            isPlayerInsideTrigger = true;
            cachedPlayerObject = other.gameObject;

            TryActivateActionObject(other.gameObject);
        }


        if (other.TryGetComponent(out iRiggable _riggable))
        {
            // Checks that collided rig type is same as setup type
            if (_riggable.TypeRig == rigType)
            {
                switch (_riggable.TypeRig)
                {
                    case eTypeRigSetup.truss:
                        {
                            if (_riggable.Enabled)
                            {
                                Debug.Log("Truss Collision with Setup Spot");

                                _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                                //_riggable.Enabled = false;

                                _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                                other.gameObject.GetComponent<sRigGear>().enabled = false;

                                // Let the manager know this truss slot is
                                // filled, so any bolt spots gating on
                                // setupIndex can now proceed.
                                sRiggingManager.riggingManger.RegisterTrussPiece(setupIndex, other.gameObject);

                                Destroy(this.gameObject);
                            }

                            break;

                        }

                    case eTypeRigSetup.motor:
                        {
                            if (_riggable.Enabled)
                            {
                                Debug.Log("Motor Collision with Setup Spot");

                                _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                                _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                                other.gameObject.GetComponent<sRigGear>().enabled = false;

                                Destroy(this.gameObject);
                            }

                            break;
                        }

                    case eTypeRigSetup.motorController:
                        {

                            if (_riggable.Enabled)
                            {
                                Debug.Log("Motor Collision with Setup Spot");

                                _riggable.SetRigging(other.gameObject, this.gameObject, offset);

                                _riggable.RiggingObjectComplete(other.gameObject, _riggable.TypeRig);

                                other.gameObject.GetComponent<sRigGear>().enabled = false;

                                Destroy(this.gameObject);
                            }

                            break;
                        }
                }
            }

            else
            {
                Debug.Log("Wrong setup spot type");
            }

        }
    }

    // TO DO - Check to see how close the rigging piece rotation is compared to the setup spot rotation


    bool ToolCheck(GameObject _toolCheckObj)
    {
        // Checks for tool handler - located on Player
        if (_toolCheckObj.TryGetComponent<sToolHandler>(out sToolHandler _toolHandler))
        {
            //Debug.Log("Collision with player and " + this.name + " - checking tool");

            // Checks to see if there are any tools held
            if (_toolHandler.ReturnToolHeldList() != null)
            {
                //Debug.Log("Tool list isn't null");

                SO_ItemData tempToolData;

                tempToolData = _toolHandler.CheckIfHasTool(ToolTypeNeeded);

                // Debug.Log(tempTool + " temp tool");

                // Returns null if tool is not correct
                if (tempToolData != null)
                {
                    //Debug.Log("Temp Tool Not Null and Setting UI");

                    // Toggles bool
                    CanTriggerAction = true;

                    // Displays text above spot
                    textSetup.SetText("CORRECT TOOL");

                    // Sets the color of the text to green showing the tool is good
                    textSetup.color = Color.green;

                    return true;
                }

                else
                {
                    Debug.Log("Temp Tool Data is null");

                    // If the player character doesn't have the correct tool
                    WrongTool();

                    return false;
                }
            }

            else
            {
                Debug.Log("Tool Handler List is null");

                // If the player character doesn't have the correct tool
                WrongTool();

                return false;
            }

        }

        else
        {
            return false;
        }

    }

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