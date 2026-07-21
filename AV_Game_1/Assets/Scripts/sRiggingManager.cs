using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eTypeRigSetup { truss, bolts, motor, motorController, speakerStand, videoScreenStand, lightingStand }
public class sRiggingManager : MonoBehaviour
{
    public static sRiggingManager riggingManger;

    public SO_EventsUI soUI;

    //public bool isDoingTut = false;

    int numberOfTrussToRig;
    int numberOfBoltingSpots;
    int numberOfMotorsToRig;
    int numberOfSpeakeStandsToRig;
    int numberOfScreenStandsToRig;

    int activeIndexTrussSetup = 0;
    int activeIndexBoltSetup = 0;
    int activeIndexMotorSetup = 0;
    int activeIndexMotorControllerSetup = 0;

    bool trussDone = false;
    bool motorsDone = false;
    bool speakerStandsDone = false;
    bool screenStandsDone = false;

    public Transform[] trussSetupLocations;
    public Transform[] boltingLocations;
    public Transform[] motorSetupLocations;
    public Transform[] motorControllerSetupLocation;
    public Transform[] speakerSetupLocations;
    public Transform[] screenStandSetupLocations;

    public GameObject pTrussSetup;
    public GameObject pBoltSetup;
    public GameObject pMotorSetup;
    public GameObject pMotorControllerSetup;
    public GameObject pSpeakerSetup;
    public GameObject pScreenSetup;

    List<GameObject> trussSetupsList;
    List<GameObject> boltingSetupsList;
    List<GameObject> motorSetupList;
    List<GameObject> motorControllerSetupList;

    public List<sTruss> trussList;

    //public List<> boltList;
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

    //public List<>


    //public List<sMotor> motorList;

    private void OnEnable()
    {
        soUI.motorControlTrigger.AddListener(GoMotor);
        soUI.motorControlStop.AddListener(MotorsStop);
    }

    private void OnDisable()
    {
        soUI.motorControlTrigger.RemoveListener(GoMotor);
        soUI.motorControlStop.RemoveListener(MotorsStop);

    }

    private void Awake()
    {
        riggingManger = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        boltingSetupsList = new List<GameObject>();
        trussSetupsList = new List<GameObject>();
        motorSetupList = new List<GameObject>();
        motorControllerSetupList = new List<GameObject>();

        trussList = new List<sTruss>();
        motorList = new List<sMotor>();
        motorControllerList = new List<sMotorController>();

        rigidTrussPieces = new GameObject[trussSetupLocations.Length];

        //SetRiggingNumbers(3, 2, 2, 0, 0);
        SpawnSetupObjects();
    }

    // This will spawn all the setup objects for each game
    void SpawnSetupObjects()
    {
        for (int i = 0; i < trussSetupLocations.Length; i++)
        {
            GameObject tempObj;

            tempObj = Instantiate(pTrussSetup, trussSetupLocations[i]);

            trussSetupsList.Add(tempObj);

            // Tell this spot which truss slot index it represents, so it
            // can register itself with the manager once rigged.
            if (tempObj.TryGetComponent(out sRiggingSetupSpot trussSpotScript))
            {
                trussSpotScript.setupIndex = i;
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

            // Assumes a linear rig layout where bolt spot i joins truss
            // slots i and i + 1. If the rig layout is not a straight
            // line matching this array order, this mapping needs to
            // change to whatever actually determines adjacency.
            if (tempObj.TryGetComponent(out sRiggingSetupSpot boltSpotScript))
            {
                boltSpotScript.neighborTrussIndexA = i;
                boltSpotScript.neighborTrussIndexB = i + 1;
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

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if (GameManager.gm.isDoingTut)
                tempObj.SetActive(false);
        }

        for (int i = 0; i < motorControllerSetupLocation.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pMotorControllerSetup, motorControllerSetupLocation[i]);

            motorControllerSetupList.Add(tempObj);

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if (GameManager.gm.isDoingTut)
                tempObj.SetActive(false);
        }

        for (int i = 0; i < speakerSetupLocations.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pSpeakerSetup, speakerSetupLocations[i]);
        }

        for (int i = 0; i < screenStandSetupLocations.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pScreenSetup, screenStandSetupLocations[i]);
        }

        trussSetupsList[activeIndexTrussSetup].SetActive(true);

    }

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
        return IsTrussIndexRigged(_trussIndexA) && IsTrussIndexRigged(_trussIndexB);
    }

    public void RigSet(eTypeRigSetup _type)
    {
        switch (_type)
        {
            case eTypeRigSetup.truss:
                {

                    activeIndexTrussSetup++;

                    if (activeIndexTrussSetup < trussSetupsList.Count)
                        trussSetupsList[activeIndexTrussSetup].SetActive(true);

                    else
                    {
                        Debug.Log("Truss setup is complete");



                        // Turns off all objects during tutorial so you only do them one at a time vs all at once
                        if (GameManager.gm.isDoingTut)
                        {
                            StartBoltSetup();
                            soUI.InstructionsRiggingTrigger("Truss setup complete - Now bolt it together - you'll need a truss tool");
                        }
                        else
                        {
                            soUI.InstructionsRiggingTrigger("Truss setup complete");
                        }

                    }

                    break;
                }

            case eTypeRigSetup.bolts:
                {
                    activeIndexBoltSetup++;

                    Debug.Log("One piece bolted!");

                    if (activeIndexBoltSetup < boltingSetupsList.Count)
                        boltingSetupsList[activeIndexBoltSetup].SetActive(true);

                    else
                    {
                        Debug.Log("Bolt Setup is complete");

                        soUI.InstructionsRiggingTrigger("Bolt setup complete - Now time to setup the motors");

                        //ConnectTruss();

                        StartMotorsSetup();
                    }

                    break;
                }

            case eTypeRigSetup.motor:
                {
                    activeIndexMotorSetup++;

                    Debug.Log("Motor rigged!");

                    if (activeIndexMotorSetup < motorSetupList.Count)
                        motorSetupList[activeIndexMotorSetup].SetActive(true);

                    else
                    {
                        Debug.Log("Motor Setup is complete");

                        soUI.InstructionsRiggingTrigger("Motor setup complete - Now time to setup the motor controller");

                        ConnectMotorsToTruss();

                        StartMotorControllerSetup();
                    }

                    break;
                }

            case eTypeRigSetup.motorController:
                {

                    Debug.Log("Motor Controller Set");

                    break;
                }

            case eTypeRigSetup.speakerStand:
                {

                    break;
                }

            case eTypeRigSetup.videoScreenStand:
                {

                    break;
                }
        }
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

        foreach (sTruss _truss in trussList)
        {
            FixedJoint joint;

            Rigidbody rb;

            rb = _truss.gameObject.GetComponent<Rigidbody>();

            rb.constraints = RigidbodyConstraints.None;

            joint = _truss.gameObject.AddComponent<FixedJoint>();

            joint.connectedBody = motorList[0].GetComponent<Rigidbody>();


        }
    }

    // This is used for tutorial purposes
    void StartBoltSetup()
    {
        boltingSetupsList[activeIndexBoltSetup].SetActive(true);
    }

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

    public void SetRiggingNumbers(int _truss, int _bolts, int _motors, int _speakers, int _screens)
    {
        numberOfTrussToRig = _truss;
        numberOfBoltingSpots = _bolts;
        numberOfMotorsToRig = _motors;
        numberOfSpeakeStandsToRig = _speakers;
        numberOfScreenStandsToRig = _screens;
    }

    // This toggles the motor on and decides which direction it will go
    void GoMotor(bool _goUp)
    {
        if (_goUp)
        {
            MotorsOn();
        }

        else
        {
            // down
        }
    }

    // This actually moves the motors after they are turned on
    void MotorsOn()
    {
        for (int i = 0; i < motorList.Count; i++)
        {
            motorList[i].StartMotorRaise();
        }
    }

    void MotorsStop()
    {

    }
}