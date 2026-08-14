using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum RiggingObjective
{
    BuildTruss,
    InstallMotors,
    RaiseTrim,
    SafetyInspection
}

public enum eTypeRigSetup { truss, bolts, motor, motorController, light }


public class sRiggingManager : sDepartmentManager
{
    public static sRiggingManager riggingMangerGlobal;

    //public bool isDoingTut = false;

    //int numberOfTrussToRig;
    //int numberOfBoltingSpots;
    //int numberOfMotorsToRig;
    //int numberOfSpeakeStandsToRig;
    //int numberOfScreenStandsToRig;

    //int activeIndexTrussSetup = 0;
    //int activeIndexBoltSetup = 0;

    // True once every bolt spot in boltingSetupsList has completed.
    // Gates whether the motors are allowed to raise at all - a truss
    // that isn't fully bolted shouldn't be liftable.
    bool allBoltsComplete = false;

    int activeIndexMotorSetup = 0;
    int activeIndexMotorControllerSetup = 0;

    //bool trussDone = false;
    //bool motorsDone = false;
    //bool speakerStandsDone = false;
    //bool screenStandsDone = false;

    //public Transform[] trussSetupLocations;
    //public Transform[] boltingLocations;
    //ublic Transform[] motorSetupLocations;
    //public Transform[] motorControllerSetupLocation;

    //public GameObject pTrussSetup;
    //public GameObject pBoltSetup;
    //public GameObject pMotorSetup;
    //public GameObject pMotorControllerSetup;

    List<GameObject> trussSetupsList;
    List<GameObject> boltingSetupsList;
    List<GameObject> motorSetupList;
    List<GameObject> motorControllerSetupList;

    public List<sTruss> trussList;

    public List<sMotor> motorList;

    public List<sMotorController> motorControllerList;

    public Transform trussTransform, motorTransform;

    // Tracks which truss setup slot (by index into trussSetupLocations)
    // has been rigged, and by which physical truss piece. Populated by
    // sRiggingSetupSpot.RegisterTrussPiece once a truss piece finishes
    // being set. Bolt setup spots query this by index (since they are
    // spawned dynamically and cannot hold a direct inspector reference
    // to a truss piece that does not exist yet).
    GameObject[] rigidTrussPieces;

    //private int totalMotors;
    //private int completedMotors;

    //private int totalTruss;
    //private int completedTruss;

    //private int totalMotorControllers;
    //private int completedMotorControllers;

    //private int totalBoltSets;
    //private int completedBoltSets;

    private void OnEnable()
    {
        soUI.motorControlTrigger.AddListener(GoMotor);
        soUI.motorControlStop.AddListener(MotorsStop);

        SceneManager.sceneLoaded += OnSceneLoad;
    }



    private void OnDisable()
    {
        soUI.motorControlTrigger.RemoveListener(GoMotor);
        soUI.motorControlStop.RemoveListener(MotorsStop);

        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void Awake()
    {
        riggingMangerGlobal = this;

        boltingSetupsList = new List<GameObject>();
        trussSetupsList = new List<GameObject>();
        motorSetupList = new List<GameObject>();
        motorControllerSetupList = new List<GameObject>();

        trussList = new List<sTruss>();
        motorList = new List<sMotor>();
        motorControllerList = new List<sMotorController>();

        overheadGearSpots = new List<sRiggingSetupSpot>();

        rigidTrussPieces = new GameObject[trussList.Count];
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (debugBypassBoltMinigame)
            allBoltsComplete = true;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        switch (GameManager.gm.GetGameMode())
        {
            case eGameMode.frontEnd:

                break;

            case eGameMode.warehouse:

                break;

            case eGameMode.gig:

                AddObjectives();
                //SpawnSetupObjects();

                break;
        }
    }

    void AddObjectives()
    {
        Status.objectives.Add(new ObjectiveStatus()
        {
            //id = "Truss",
            name = "Build Truss"
        });

        Status.objectives.Add(new ObjectiveStatus()
        {
            //id = "Motors",
            name = "Install Motors"
        });

        Status.objectives.Add(new ObjectiveStatus()
        {
            //id = "Raise",
            name = "Raise Truss"
        });
    }

    public void RegisterRiggingSetup(eTypeRigSetup _type, GameObject tempObj)
    {
        if (tempObj == null)
            return;

        switch (_type)
        {
            case eTypeRigSetup.truss:

                trussSetupsList.Add(tempObj);
                status.objectives[0].totalItems++;
                //sRiggingSetupSpot trussSpotScript = tempObj.GetComponentInChildren<sRiggingSetupSpot>(true);

                /*if (trussSpotScript != null)
                {
                    //trussSpotScript.setupIndex = i;

                    Debug.Log("[RiggingManager] Assigned setupIndex " + i + " to " + trussSpotScript.name
                        + " (instance id " + trussSpotScript.GetInstanceID() + ")");
                }
                else
                {
                    Debug.LogWarning("pTrussSetup prefab has no sRiggingSetupSpot component anywhere in its hierarchy: " + tempObj.name);
                }*/

                // Turns off all objects during tutorial so you only do them one at a time vs all at once
                //if (GameManager.gm.isDoingTut)
                //    tempObj.SetActive(false);


                break;

            case eTypeRigSetup.bolts:

                boltingSetupsList.Add(tempObj);

                status.objectives[0].totalItems++;

                // Assumes a linear rig layout where bolt spot i joins truss
                // slots i and i + 1. If the rig layout is not a straight
                // line matching this array order, this mapping needs to
                // change to whatever actually determines adjacency.
                sRiggingSetupSpot boltSpotScript = tempObj.GetComponentInChildren<sRiggingSetupSpot>();

                /*if (boltSpotScript != null)
                {
                    boltSpotScript.neighborTrussIndexA = i;
                    boltSpotScript.neighborTrussIndexB = i + 1;
                }
                else
                {
                    Debug.LogWarning("pBoltSetup prefab has no sRiggingSetupSpot component anywhere in its hierarchy: " + tempObj.name);
                }*/

                // Turns off all objects during tutorial so you only do them one at a time vs all at once
                //if (GameManager.gm.isDoingTut)
                //    tempObj.SetActive(false);

                break;

            case eTypeRigSetup.motor:

                motorSetupList.Add(tempObj);

                status.objectives[1].totalItems++;

                // Turns off all objects during tutorial so you only do them one at a time vs all at once
                //if (GameManager.gm.isDoingTut)
                //   tempObj.SetActive(false);

                break;

            case eTypeRigSetup.motorController:

                motorControllerSetupList.Add(tempObj);

                status.objectives[1].totalItems++;

                // Turns off all objects during tutorial so you only do them one at a time vs all at once
                //if (GameManager.gm.isDoingTut)
                //    tempObj.SetActive(false);

                break;

            case eTypeRigSetup.light:

                break;
        }

        //trussSetupsList[activeIndexTrussSetup].SetActive(true);
    }

    // This will spawn all the setup objects for each game
    /*void SpawnSetupObjects()
    {
        for (int i = 0; i < trussSetupLocations.Length; i++)
        {
            GameObject tempObj;

            tempObj = Instantiate(pTrussSetup, trussSetupLocations[i]);

            trussSetupsList.Add(tempObj);

            status.objectives[0].totalItems++;

            // Tell this spot which truss slot index it represents, so it
            // can register itself with the manager once rigged. Uses
            // GetComponentInChildren rather than TryGetComponent/
            // GetComponent since sRiggingSetupSpot may live on a child
            // of the prefab root rather than the root itself.
            sRiggingSetupSpot trussSpotScript = tempObj.GetComponentInChildren<sRiggingSetupSpot>(true);

            if (trussSpotScript != null)
            {
                trussSpotScript.setupIndex = i;

                Debug.Log("[RiggingManager] Assigned setupIndex " + i + " to " + trussSpotScript.name
                    + " (instance id " + trussSpotScript.GetInstanceID() + ")");
            }
            else
            {
                Debug.LogWarning("pTrussSetup prefab has no sRiggingSetupSpot component anywhere in its hierarchy: " + tempObj.name);
            }

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if (GameManager.gm.isDoingTut)
                tempObj.SetActive(false);

        }

        for (int i = 0; i < boltingLocations.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pBoltSetup, boltingLocations[i]);

            boltingSetupsList.Add(tempObj);

            status.objectives[0].totalItems++;

            // Assumes a linear rig layout where bolt spot i joins truss
            // slots i and i + 1. If the rig layout is not a straight
            // line matching this array order, this mapping needs to
            // change to whatever actually determines adjacency.
            sRiggingSetupSpot boltSpotScript = tempObj.GetComponentInChildren<sRiggingSetupSpot>();

            if (boltSpotScript != null)
            {
                boltSpotScript.neighborTrussIndexA = i;
                boltSpotScript.neighborTrussIndexB = i + 1;
            }
            else
            {
                Debug.LogWarning("pBoltSetup prefab has no sRiggingSetupSpot component anywhere in its hierarchy: " + tempObj.name);
            }

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if (GameManager.gm.isDoingTut)
                tempObj.SetActive(false);
        }

        for (int i = 0; i < motorSetupLocations.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pMotorSetup, motorSetupLocations[i]);

            motorSetupList.Add(tempObj);

            status.objectives[1].totalItems++;

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if (GameManager.gm.isDoingTut)
                tempObj.SetActive(false);
        }

        for (int i = 0; i < motorControllerSetupLocation.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pMotorControllerSetup, motorControllerSetupLocation[i]);

            motorControllerSetupList.Add(tempObj);

            status.objectives[1].totalItems++;

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if (GameManager.gm.isDoingTut)
                tempObj.SetActive(false);
        }

        //totalRigSets = trussSetupsList.Count + boltingSetupsList.Count + motorSetupList.Count + motorControllerSetupList.Count;

        //totalRigSets = 4;

        //totalMotors = motorSetupList.Count;

        //totalTruss = trussSetupsList.Count;

        //totalMotorControllers = motorControllerSetupList.Count;

        //totalBoltSets = boltingSetupsList.Count;

        trussSetupsList[activeIndexTrussSetup].SetActive(true);

    }*/

    // Called by a truss sRiggingSetupSpot once its truss piece finishes
    // being rigged, so bolt spots gating on this slot can query it.
    public void RegisterTrussPiece(int _index, GameObject _trussPiece)
    {
        if (_index < 0 || _index >= rigidTrussPieces.Length)
        {
            Debug.LogWarning("sRiggingManager.RegisterTrussPiece index out of range: " + _index);
            return;
        }

        rigidTrussPieces[_index] = _trussPiece;

        Debug.Log("[RiggingManager] Registered truss piece at index " + _index + ": " + _trussPiece.name);

        NotifyBoltSpotsToRecheck();
    }

    // A truss slot just filled. If a bolt spot depending on it already
    // has the player standing inside its trigger zone (waiting), it
    // would otherwise stay stuck inactive since OnTriggerEnter will not
    // fire again on its own. This gives each live bolt spot a chance to
    // re-evaluate and activate itself.
    void NotifyBoltSpotsToRecheck()
    {
        foreach (GameObject boltSpotObj in boltingSetupsList)
        {
            if (boltSpotObj != null && boltSpotObj.TryGetComponent(out sRiggingSetupSpot boltSpotScript))
            {
                boltSpotScript.RecheckNeighborReadiness();
            }
        }
    }

    // True if a truss slot index is either out of range (meaning there
    // is no truss expected at that position, e.g. the end of the rig)
    // or has already been rigged.
    public bool IsTrussIndexRigged(int _index)
    {
        if (_index < 0 || _index >= rigidTrussPieces.Length)
        {
            return true;
        }

        return rigidTrussPieces[_index] != null;
    }

    public bool AreNeighboringTrussSet(int _trussIndexA, int _trussIndexB)
    {
        bool aRigged = IsTrussIndexRigged(_trussIndexA);
        bool bRigged = IsTrussIndexRigged(_trussIndexB);

        Debug.Log("[RiggingManager] Checking neighbors - index " + _trussIndexA + " rigged: " + aRigged
            + ", index " + _trussIndexB + " rigged: " + bRigged);

        return aRigged && bRigged;
    }

    [Header("Debug/Testing")]
    // When true, bolt spots skip opening the drag-and-drop minigame
    // entirely and complete instantly the moment their neighbor/tool
    // gates pass. Toggleable live in Play mode. Leave off for normal
    // play.
    public bool debugBypassBoltMinigame = false;

    [Header("Truss Weld Joints")]
    // Left at infinity for now - once the weight-limit system is in,
    // these should be driven by the truss's rated capacity so an
    // overloaded joint genuinely snaps under real physics stress
    // instead of being blocked by a manual check.
    public float trussJointBreakForce = Mathf.Infinity;
    public float trussJointBreakTorque = Mathf.Infinity;

    // Called once a bolt spot finishes, connecting the two truss pieces
    // it was gating on with a FixedJoint anchored at the bolt spot's
    // world position. Requires both neighbor slots to already be
    // rigged (which FinishSetup only calls this after, since the bolt
    // spot could not have activated otherwise).
    public void WeldTrussNeighbors(int _indexA, int _indexB, Vector3 _weldWorldPosition)
    {
        if (_indexA < 0 || _indexA >= rigidTrussPieces.Length
            || _indexB < 0 || _indexB >= rigidTrussPieces.Length)
        {
            // One side is out of range - the edge of the rig, nothing
            // to weld against.
            return;
        }

        GameObject trussA = rigidTrussPieces[_indexA];
        GameObject trussB = rigidTrussPieces[_indexB];

        if (trussA == null || trussB == null)
        {
            Debug.LogWarning("WeldTrussNeighbors called before both truss slots were rigged: "
                + _indexA + ", " + _indexB);
            return;
        }

        Rigidbody rbA = trussA.GetComponent<Rigidbody>();
        Rigidbody rbB = trussB.GetComponent<Rigidbody>();

        if (rbA == null || rbB == null)
        {
            Debug.LogWarning("Truss pieces are missing a Rigidbody - cannot weld: " + trussA.name + ", " + trussB.name);
            return;
        }

        // Guard against welding the same pair twice (e.g. if
        // FinishSetup fires more than once for the same bolt spot) -
        // a duplicate FixedJoint on an already-connected pair adds
        // nothing but redundant constraint solving.
        FixedJoint[] existingJoints = trussA.GetComponents<FixedJoint>();

        foreach (FixedJoint _existing in existingJoints)
        {
            if (_existing.connectedBody == rbB)
            {
                Debug.LogWarning("[RiggingManager] Skipped duplicate weld - " + trussA.name
                    + " is already jointed to " + trussB.name);
                return;
            }
        }

        FixedJoint joint = trussA.AddComponent<FixedJoint>();
        joint.connectedBody = rbB;
        joint.anchor = trussA.transform.InverseTransformPoint(_weldWorldPosition);
        joint.autoConfigureConnectedAnchor = true;
        joint.breakForce = trussJointBreakForce;
        joint.breakTorque = trussJointBreakTorque;

        Debug.Log("[RiggingManager] Welded truss " + trussA.name + " to " + trussB.name + " at bolt spot");
    }

    [Header("Truss Weight Limit")]
    // Single total limit for the whole truss run for now. Split into
    // per-motor capacity later if needed.
    public float trussWeightLimit = 500f;

    // Which rig types actually hang weight off the truss, as opposed to
    // floor stands that do not load it. Editable here rather than
    // hardcoded so items can be added/removed without touching code.
    public List<eTypeRigSetup> weightBearingRigTypes = new List<eTypeRigSetup>
    {
        //eTypeRigSetup.lightingStand,
        //eTypeRigSetup.speakerStand,
        //eTypeRigSetup.videoScreenStand
    };

    float currentTrussWeight = 0f;

    public float CurrentTrussWeight
    {
        get
        {
            return currentTrussWeight;
        }
    }

    bool trussHasFailed = false;

    public bool TrussHasFailed
    {
        get
        {
            return trussHasFailed;
        }
    }

    // Called from iRiggable.RiggingObjectComplete whenever any piece of
    // gear finishes being rigged. Only tallies types listed in
    // weightBearingRigTypes - floor stands and other non-truss types
    // are ignored.
    public void RegisterTrussWeight(eTypeRigSetup _type, SO_ItemData _itemData)
    {
        if (trussHasFailed)
        {
            return;
        }

        if (!weightBearingRigTypes.Contains(_type))
        {
            return;
        }

        if (_itemData == null)
        {
            Debug.LogWarning("RegisterTrussWeight called with null ItemData for type " + _type);
            return;
        }

        // ASSUMPTION: SO_ItemData has a public float weight field. If
        // the actual field/property is named differently, this is the
        // only line that needs to change.
        currentTrussWeight += _itemData.weight;

        Debug.Log("[RiggingManager] Truss weight now " + currentTrussWeight + " / " + trussWeightLimit
            + " (added " + _itemData.weight + " from " + _type + ")");

        if (currentTrussWeight > trussWeightLimit)
        {
            TriggerTrussOverloadFailure();
        }
    }

    // Real structural consequence for overload: destroys every joint on
    // every truss piece (both the truss-to-truss welds from bolting and
    // the truss-to-motor connection from ConnectMotorsToTruss) and
    // re-enables gravity, so the whole assembly actually drops rather
    // than just being blocked from accepting more weight.
    void TriggerTrussOverloadFailure()
    {
        trussHasFailed = true;

        Debug.Log("[RiggingManager] TRUSS OVERLOADED - failing joints, truss is dropping");

        foreach (sTruss _truss in trussList)
        {
            if (_truss == null)
            {
                continue;
            }

            FixedJoint[] joints = _truss.GetComponents<FixedJoint>();

            foreach (FixedJoint _joint in joints)
            {
                Destroy(_joint);
            }

            Rigidbody rb = _truss.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        if (soUI != null)
        {
            soUI.InstructionsRiggingTrigger("The truss is overloaded and has failed!");
        }
    }

    // Right-click this component's header in the inspector during Play
    // mode to add test weight without needing lights/speakers/screens
    // fully wired into the rigging flow yet. Bypasses ItemData entirely
    // so it does not depend on SO_ItemData's actual weight field name.
    [ContextMenu("Debug - Add 100 Truss Weight")]
    void DebugAddTestWeight()
    {
        if (trussHasFailed)
        {
            Debug.Log("[RiggingManager] (DEBUG) Truss has already failed - ignoring.");
            return;
        }

        currentTrussWeight += 100f;

        Debug.Log("[RiggingManager] (DEBUG) Truss weight now " + currentTrussWeight + " / " + trussWeightLimit);

        if (currentTrussWeight > trussWeightLimit)
        {
            TriggerTrussOverloadFailure();
        }
    }

    // Right-click this component's header in the inspector during Play
    // mode, at the moment the truss overshoots/resets, to dump exact
    // position, joint, and constraint state for every truss and motor.
    [ContextMenu("Debug - Log Truss And Motor State")]
    void DebugLogTrussAndMotorState()
    {
        Debug.Log("[RiggingManager] (DEBUG) --- Motor state ---");

        foreach (sMotor _motor in motorList)
        {
            if (_motor == null)
            {
                continue;
            }

            Rigidbody motorRb = _motor.GetComponent<Rigidbody>();

            Debug.Log("[RiggingManager] (DEBUG) Motor " + _motor.name
                + " position = " + _motor.transform.position
                + ", rb.position = " + (motorRb != null ? motorRb.position.ToString() : "NO RIGIDBODY")
                + ", isKinematic = " + (motorRb != null ? motorRb.isKinematic.ToString() : "n/a")
                + ", constraints = " + (motorRb != null ? motorRb.constraints.ToString() : "n/a"));
        }

        Debug.Log("[RiggingManager] (DEBUG) --- Truss state ---");

        foreach (sTruss _truss in trussList)
        {
            if (_truss == null)
            {
                continue;
            }

            Rigidbody trussRb = _truss.GetComponent<Rigidbody>();

            FixedJoint[] joints = _truss.GetComponents<FixedJoint>();

            string jointSummary = joints.Length == 0 ? "NONE" : "";

            foreach (FixedJoint _joint in joints)
            {
                string connectedName = _joint.connectedBody != null ? _joint.connectedBody.name : "NULL";
                jointSummary += "[connectedBody=" + connectedName + ", anchor=" + _joint.anchor + "] ";
            }

            Debug.Log("[RiggingManager] (DEBUG) Truss " + _truss.name
                + " position = " + _truss.transform.position
                + ", rb.position = " + (trussRb != null ? trussRb.position.ToString() : "NO RIGIDBODY")
                + ", isKinematic = " + (trussRb != null ? trussRb.isKinematic.ToString() : "n/a")
                + ", useGravity = " + (trussRb != null ? trussRb.useGravity.ToString() : "n/a")
                + ", constraints = " + (trussRb != null ? trussRb.constraints.ToString() : "n/a")
                + ", joints = " + jointSummary);
        }
    }

    public void RigSet(eTypeRigSetup _type)
    {
        Debug.Log("Setting rig for " + _type);

        switch (_type)
        {
            case eTypeRigSetup.truss:
                {

                    //activeIndexTrussSetup++;
                    //completedTruss++;

                    ObjectiveStatus truss = Status.objectives[0];

                    truss.completedItems++;

                    //truss.Refresh();

                    /*if (activeIndexTrussSetup < trussSetupsList.Count)
                    {
                        trussSetupsList[activeIndexTrussSetup].SetActive(true);
                    }
                        

                    else
                    {
                        Debug.Log("Truss setup is complete");

                        //completedRigSets++;

                        // Turns off all objects during tutorial so you only do them one at a time vs all at once
                        *//*if (GameManager.gm.isDoingTut)
                        {
                            //StartBoltSetup();

                            soUI.InstructionsRiggingTrigger("Truss setup complete - Now bolt it together - you'll need a truss tool");
                        }
                        else
                        {
                            soUI.InstructionsRiggingTrigger("Truss setup complete");
                        }*//*

                    }*/

                    if (soAudio != null)
                        soAudio.TriggerSFX("RigItemPlaced");

                    break;
                }

            case eTypeRigSetup.bolts:
                {
                    //activeIndexBoltSetup++;

                    ObjectiveStatus truss = Status.objectives[0];

                    truss.completedItems++;

                    //truss.Refresh();

                    //Debug.Log("One piece bolted!");

                    /*if (activeIndexBoltSetup < boltingSetupsList.Count)
                    {
                       
                        boltingSetupsList[activeIndexBoltSetup].SetActive(true);
                    }
                        

                    else
                    {
                        Debug.Log("Bolt Setup is complete");

                        allBoltsComplete = true;

                        soUI.InstructionsRiggingTrigger("Bolt setup complete - Now time to setup the motors");

                        //ConnectTruss();

                        //StartMotorsSetup();
                    }*/

                    break;
                }

            case eTypeRigSetup.motor:
                {
                    activeIndexMotorSetup++;

                    ObjectiveStatus motor = Status.objectives[1];

                    motor.completedItems++;

                    //motor.Refresh();

                    Debug.Log("Motor rigged!");

                    if (activeIndexMotorSetup < motorSetupList.Count)
                    {
                        //motorSetupList[activeIndexMotorSetup].SetActive(true);
                    }


                    else
                    {
                        Debug.Log("Motor Setup is complete");

                        soUI.InstructionsRiggingTrigger("Motor setup complete - Now time to setup the motor controller");

                        ConnectMotorsToTruss();

                        //StartMotorControllerSetup();
                    }

                    if (soAudio != null)
                        soAudio.TriggerSFX("RigMotorPlaced");

                    break;
                }

            case eTypeRigSetup.motorController:
                {
                    ObjectiveStatus motor = Status.objectives[1];

                    motor.completedItems++;

                    //motor.Refresh();

                    //Debug.Log("Motor Controller Set");

                    break;
                }
        }

        //Debug.Log("Refreshing Progress");

        

        RefreshProgress();

        UpdateDepartmentUI();
    }

    /*
    void ConnectTruss()
    {
        Debug.Log("Connecting Truss to Motors");

        for (int i = 0; i < trussList.Count; i++)
        {
            Rigidbody rbTruss;
            rbTruss = trussList[i].GetComponent<Rigidbody>();

            FixedJoint joint;
            joint = trussList[i].gameObject.AddComponent<FixedJoint>();

            //if (i > 0)
            //{
                Rigidbody _parentRB;

            //_parentRB = trussList[i - 1].GetComponent<Rigidbody>();

            _parentRB = motorList[0].gameObject.GetComponent<Rigidbody>();

                //trussList[i].transform.parent = trussList[i-1].transform;

                joint.connectedBody = _parentRB;              
        }
    }
    */

    void ConnectMotorsToTruss()
    {
        Debug.Log("Connecting Motors to Truss");

        if (motorList.Count == 0)
        {
            Debug.LogWarning("ConnectMotorsToTruss called with an empty motorList - no truss pieces connected.");
            return;
        }

        foreach (sTruss _truss in trussList)
        {
            if (_truss == null)
            {
                continue;
            }

            Rigidbody rb = _truss.gameObject.GetComponent<Rigidbody>();

            if (rb == null)
            {
                continue;
            }

            rb.constraints = RigidbodyConstraints.None;

            // Connect to the nearest motor rather than always
            // motorList[0], so each motor supports the truss segment
            // physically closest to it instead of every piece being
            // yoked to a single motor regardless of position - which
            // was over-constraining the joint system whenever there is
            // more than one motor.
            sMotor nearestMotor = FindNearestMotor(_truss.transform.position);

            if (nearestMotor == null)
            {
                continue;
            }

            FixedJoint joint = _truss.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = nearestMotor.GetComponent<Rigidbody>();

            Debug.Log("[RiggingManager] Connected " + _truss.name + " to nearest motor " + nearestMotor.name);
        }
    }

    sMotor FindNearestMotor(Vector3 _position)
    {
        sMotor nearest = null;
        float nearestDist = float.MaxValue;

        foreach (sMotor _motor in motorList)
        {
            if (_motor == null)
            {
                continue;
            }

            float dist = Vector3.Distance(_position, _motor.transform.position);

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = _motor;
            }
        }

        return nearest;
    }

    // Gates gear that should only be mountable once the truss is
    // raised enough to work on (lights, and later speakers/screens).
    // Requires every motor to be at working height or above, since a
    // partially-raised run with some motors still on the ground is not
    // a safe/consistent working height across the whole truss.
    public bool AreAllMotorsAtOrAboveWorkingHeight()
    {
        if (motorList.Count == 0)
        {
            return false;
        }

        foreach (sMotor _motor in motorList)
        {
            if (_motor == null || !_motor.IsAtOrAboveWorkingHeight)
            {
                return false;
            }
        }

        return true;
    }

    // Light (and later speaker/screen) setup spots register themselves
    // here at Start() so their GameObject can be shown/hidden entirely
    // based on truss readiness, rather than just having their
    // collision silently rejected while sitting visible and active.
    List<sRiggingSetupSpot> overheadGearSpots;

    public void RegisterOverheadGearSpot(sRiggingSetupSpot _spot)
    {
        if (!overheadGearSpots.Contains(_spot))
        {
            overheadGearSpots.Add(_spot);
        }

        //_spot.SetOverheadGearActive(AreAllMotorsAtOrAboveWorkingHeight());
    }

    // Called by sMotor whenever its raise/lower stage changes. Shows or
    // hides every registered overhead gear spot to match current
    // readiness - so spots appear the moment the truss reaches working
    // height, and disappear again if it is lowered back below that,
    // rather than staying visible but silently rejecting placement.
    public void OnMotorStageChanged()
    {
        bool ready = AreAllMotorsAtOrAboveWorkingHeight();

        foreach (sRiggingSetupSpot _spot in overheadGearSpots)
        {
            if (_spot != null)
            {
                _spot.SetOverheadGearActive(ready);
                _spot.transform.position = _spot.transform.position + Vector3.down * 1.5f;
            }
        }
    }

    // This is used for tutorial purposes
    /*void StartBoltSetup()
    {
        boltingSetupsList[activeIndexBoltSetup].SetActive(true);
    }*/

    // This is used for tutorial purposes
    void StartMotorsSetup()
    {
        motorSetupList[activeIndexMotorSetup].SetActive(true);
    }

    // This is used for tutorial purposes
    void StartMotorControllerSetup()
    {
        motorControllerSetupList[activeIndexMotorControllerSetup].SetActive(true);
    }

    /*public void SetRiggingNumbers(int _truss, int _bolts, int _motors, int _speakers, int _screens)
    {
        numberOfTrussToRig = _truss;
        numberOfBoltingSpots = _bolts;
        numberOfMotorsToRig = _motors;
        numberOfSpeakeStandsToRig = _speakers;
        numberOfScreenStandsToRig = _screens;
    }*/

    // This toggles the motor on and decides which direction it will go
    void GoMotor(bool _goUp)
    {
        if (_goUp)
        {
            MotorsOn();
        }

        else
        {
            MotorsDown();
        }
    }

    // This actually moves the motors after they are turned on
    void MotorsOn()
    {
        if (!allBoltsComplete)
        {
            Debug.Log("[RiggingManager] Cannot raise - not all bolts are secured yet.");

            if (soUI != null)
            {
                soUI.InstructionsRiggingTrigger("Truss cannot be raised until all bolts are secured.");
            }

            return;
        }

        ValidateMotorSync();

        if (soAudio != null)
            soAudio.TriggerSFX("RigTrussRise");

        for (int i = 0; i < motorList.Count; i++)
        {
            motorList[i].StartMotorRaise();
        }
    }

    // Mirror of MotorsOn for the down control. Each motor drops back
    // one stage at a time (ceiling -> working height -> ground).
    void MotorsDown()
    {
        ValidateMotorSync();

        for (int i = 0; i < motorList.Count; i++)
        {
            motorList[i].StartMotorLower();
        }
    }

    // Truss pieces are welded into one rigid chain (bolts) and each
    // piece is jointed to its nearest motor. That only stays physically
    // consistent if every motor sharing a rigid run moves by the exact
    // same amount at the exact same rate - otherwise the weld joints
    // and the motor joints fight each other every physics step, which
    // shows up as erratic overshoot/snap-back during the raise. This
    // does not block the raise, it just surfaces a clear warning naming
    // the mismatched motor and field so it is easy to catch and fix in
    // the inspector.
    void ValidateMotorSync()
    {
        if (motorList.Count <= 1)
        {
            return;
        }

        sMotor reference = motorList[0];

        if (reference == null)
        {
            return;
        }

        for (int i = 1; i < motorList.Count; i++)
        {
            sMotor _motor = motorList[i];

            if (_motor == null)
            {
                continue;
            }

            if (!Mathf.Approximately(_motor.workingHeight, reference.workingHeight))
            {
                Debug.LogWarning("[RiggingManager] " + _motor.name + ".workingHeight (" + _motor.workingHeight
                    + ") does not match " + reference.name + ".workingHeight (" + reference.workingHeight
                    + ") - motors sharing a rigid truss run should raise identical amounts.");
            }

            if (!Mathf.Approximately(_motor.heightToRaise, reference.heightToRaise))
            {
                Debug.LogWarning("[RiggingManager] " + _motor.name + ".heightToRaise (" + _motor.heightToRaise
                    + ") does not match " + reference.name + ".heightToRaise (" + reference.heightToRaise + ").");
            }

            if (!Mathf.Approximately(_motor.motorRaiseTime, reference.motorRaiseTime))
            {
                Debug.LogWarning("[RiggingManager] " + _motor.name + ".motorRaiseTime (" + _motor.motorRaiseTime
                    + ") does not match " + reference.name + ".motorRaiseTime (" + reference.motorRaiseTime
                    + ") - mismatched raise duration will desync motors mid-raise even if heights match.");
            }
        }
    }

    void MotorsStop()
    {

    }

    public override float CalculateProgress()
    {
        base.CalculateProgress();

        Debug.Log("Calculating Rigging Progress");

        //float total = 0;
        //int objectives = 0;

        //// Motors
        //if (totalMotors > 0)
        //{
        //    total+= (float)completedMotors / totalMotors;
        //    objectives++;
        //}

        //// Truss
        //if (totalTruss > 0)
        //{
        //    total += (float)completedTruss / totalTruss;
        //    objectives++;
        //}

        //// Bolts
        //if(totalBoltSets > 0)
        //{
        //    total += (float)completedBoltSets / totalBoltSets;
        //    objectives++;
        //}

        //if(totalMotorControllers > 0)
        //{
        //    total += (float)completedMotorControllers / totalMotorControllers;
        //    objectives++;
        //}

        //if (objectives == 0)
        //    return 0f;



        return progress; // total / objectives;
    }
}