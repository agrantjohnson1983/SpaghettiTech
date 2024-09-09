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

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            if(GameManager.gm.isDoingTut)
            tempObj.SetActive(false);
            
        }

        for (int i = 0; i < boltingLocations.Length; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(pBoltSetup, boltingLocations[i]);

            boltingSetupsList.Add(tempObj);

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

    public void RigSet(eTypeRigSetup _type)
    {
        switch(_type)
        {
            case eTypeRigSetup.truss:
                {

                    activeIndexTrussSetup++;

                    if(activeIndexTrussSetup < trussSetupsList.Count)
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

        foreach(sTruss _truss in trussList)
        {
            FixedJoint joint;

            Rigidbody rb;

            rb = _truss.gameObject.GetComponent<Rigidbody>();

            rb.constraints = RigidbodyConstraints.None;

            joint =_truss.gameObject.AddComponent<FixedJoint>();

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
        if(_goUp)
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
