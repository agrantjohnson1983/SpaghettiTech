using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sConnectionSource : MonoBehaviour
{
    public ePlugType typeOfConnection;

    int numberOfPlugsOpen;

    bool isFull;

    bool connectionCanvasOpen = false;
    bool connectionAvailableCanvasOpen = false;

    // connection plate image - this shows the type of connection
    public Sprite connectionPlateImage;

    // prefabs for connection panel and connection avail panel
    public GameObject pConnectionPanel, pConnectionAvailablePanel;

    // this holds all the connections for receiving the source
    uConnectionPlate connectionPlate;

    // this panel shows the "connections available" which spawns an image you click and drag to the connection plate
    // and a line renderer follows
    uConnectionsAvailablePanel connectionsAvailablePlate;

    // the list of pluggables that collide with source
    [HideInInspector] public List<GameObject> pluggableList;

    Rigidbody rb;

    public Transform connectionsPlateTransforms;

    public SO_Text soText;


    // METER - this can be used for a variety of devices
    public float meterTotal = 100f;

    float meterCurrent;

    public Image meterUI;

    // Start is called before the first frame update
    void Start()
    {
        meterCurrent = meterTotal;

        UpdateMeterUI(meterCurrent);

        pluggableList = new List<GameObject>();

        SpawnConnectionsAvailablePanel();

        SpawnConnectionPlate();

        rb = GetComponent<Rigidbody>();
    }

    void UpdateMeterUI(float _currentPower)
    {
        meterUI.fillAmount = meterCurrent / meterTotal;
    }

    public void DrainPower(float _amount)
    {
        //Debug.Log("Power Drain called - draining by " + _amount);

        meterCurrent -= _amount;
        UpdateMeterUI(meterCurrent);
    }

    // This spawns the connection plate which all of the input channels
    void SpawnConnectionPlate()
    {
        //Debug.Log("Spawning Connection plate");

        // temp game object reference
        GameObject tempObj;

        // spawns a connection panel prefab in the connectin plate transform
        tempObj = Instantiate(pConnectionPanel, connectionsPlateTransforms);

        // gets a reference to the connection plate
        connectionPlate = tempObj.GetComponentInChildren<uConnectionPlate>();

        // Gives connectino plate a reference to this connection source
        connectionPlate.SetSource(this);

        // Turns off the connection plate
        connectionPlate.gameObject.transform.parent.gameObject.SetActive(false);

        if (connectionsAvailablePlate)
        {
            connectionsAvailablePlate.SetConnectionPlate(connectionPlate);
            connectionPlate.SetConnectionsAvailablePanel(connectionsAvailablePlate);
        }
            
        else
            Debug.LogWarning("No connections available plate");
    }


    // This spawns the connection available panel which shows connections that are avaiable when the have collided with the connection source
    void SpawnConnectionsAvailablePanel()
    {
        //Debug.Log("Spawning connections avail panel");

        GameObject tempObj;

        tempObj = Instantiate(pConnectionAvailablePanel, connectionsPlateTransforms);

        connectionsAvailablePlate = tempObj.GetComponent<uConnectionsAvailablePanel>();

        // Gives the connection available plate a reference to this connection source
        connectionsAvailablePlate.SetConnectionSource(this);

        if(connectionPlate)
        {

        }

        connectionsAvailablePlate.gameObject.SetActive(false);
    }


    // This handles adding a joint to the plug object when it collides with the connection source
    public void ConnectPlugJoint(GameObject _plugObject)
    {
        //Debug.Log("Adding hinge joint to " + _plugObject.name);

        HingeJoint joint;

        // adds joint to the plug object
        joint = _plugObject.AddComponent<HingeJoint>();

        // sets the joints connected body to this rigidbody
        joint.connectedBody = rb;
    }


    // This gets called when a plug connection is completed within the connection plate UI system
    public void ConnectPlug(int _index)
    {
        Debug.Log("Plug is connected at index of " + _index);

        // This sets the connectionSource of the plug to this.
        pluggableList[_index].GetComponent<iPluggable>().SetConnection(this.gameObject, 0f);
    }

    // When a connection is clicked from connection plate - turns connection plate off
    public void OnConnectionClick(int _index)
    {
        Debug.Log("Power is Connected to source");

        //pluggableList[_index].GetComponent<iPluggable>().SetConnection(this.gameObject);

        // TO DO - Set all the segments of a cable to yellow if half connected

        //connectionPlate.gameObject.SetActive(false);
    }

    // This Removes a pluggable object based in the index given
    public void DisconnectClick(int _index)
    {
        Debug.Log("Power is now disconnected");

        // temp GO ref
        GameObject tempObj;

        // sets temp GO as pluggable list obj
        tempObj = pluggableList[_index];

        // removes plug from pluggable list
        pluggableList.Remove(tempObj);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checks for a pluggable interaface
        if (collision.gameObject.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        {
            // Checks if the plug is input and also that the plug is NOT plugged in AND if the connection type is correct - Connection Source requires an input - 
            if(_pluggable.IsInput && !_pluggable.IsPluggedIn && _pluggable.CheckIfCorrectConnection(typeOfConnection) == true)
            {
                //Debug.Log("Input connection detected");

                // Checks for dupes in pluggable list
                bool _isThereADupe = false;

                // Checks if the pluggable list has anything in it - looking for dupes
                if (pluggableList.Count > 1 )
                {
                    //Debug.Log("Checking for dupes");

                    // iterates through the pluggable list - should the Count be -1?
                    for (int i = 0; i < pluggableList.Count; i++)
                    {
                        // Checks if the pluggable in the pluggable list is the same as the one collided with
                        if (pluggableList[i].GetComponent<iPluggable>() == _pluggable)
                        {
                            Debug.Log("Dupe pluggable found!");

                            // Sets bool to true if there was a dupe
                            _isThereADupe = true;
                        }
                    }

                    // if no dupe then can be added to pluggables list
                    if (_isThereADupe == false)
                    {
                        //Debug.Log("No dupe found - Adding Pluggable " + _pluggable.ToString() + " to pluggable available list");

                        // Connects joint to plug securing it's position 
                        ConnectPlugJoint(collision.gameObject);

                        // adds plug to list of plugs available
                        pluggableList.Add(collision.gameObject);

                        // gives the pluggable a reference to this GO which will get a ref to this source behavior
                        _pluggable.SetConnection(this.gameObject, 0f);

                        // sets plug to available for plugin
                        _pluggable.SetPlugAvailable(true);

                        // Sets the index in the pluggable.  Minus -1 to get correct array position.
                        _pluggable.SetIndex(pluggableList.Count-1);
                    }

                    else
                    {
                        //Debug.Log("Dupe was found - doing nothing");
                    }
                }

                else
                {
                    Debug.Log("First Pluggable " + _pluggable.ToString() + " added to List");

                    // Connects joint to plug securing it's position 
                    ConnectPlugJoint(collision.gameObject);

                    // creats new pluggable list
                    pluggableList = new List<GameObject>();

                    // adds plug to list of plugs available
                    pluggableList.Add(collision.gameObject);

                    // gives the pluggable a reference to this GO which will get a ref to this source behavior
                    _pluggable.SetConnection(this.gameObject, 0f);

                    // sets plug to available for plugin
                    _pluggable.SetPlugAvailable(true);

                    // Sets the index in the pluggable.  Minus -1 to get correct array position.
                    _pluggable.SetIndex(pluggableList.Count - 1);
                }
            }
            
            else
            {
                Debug.Log("Plug is an output or plug is plugged in or the plug type is incorrect!");

                if(_pluggable.CheckIfCorrectConnection(typeOfConnection) == false)
                {
                    // Spawns a text message saying wrong type of connection
                    if (soText != null)
                        soText.SpawnTextPopup(this.transform, "WRONG TYPE OF CONNECTION", 12);
                }

                else if(_pluggable.IsInput && _pluggable.CheckIfCorrectConnection(typeOfConnection) == true)
                {
                    // Spawns a text message saying wrong end of cable
                    if(soText != null)
                        soText.SpawnTextPopup(this.transform, "WRONG END OF CABLE", 12);
                }

                else
                {
                    // Not sure what would go here?
                }
            }

            //Debug.Log("Pluggable List count is: " + pluggableList.Count);
        }
/*
        // Checks for a collision with a player
        if(collision.gameObject.CompareTag("Player"))
        {
            if(pluggableList.Count <= 0)
            {
                Debug.Log("No pluggables in list");
                return;
            }

            // Checks the connection canvas is not open
            if(!connectionCanvasOpen && pluggableList.Count > 0)
            {
                //Debug.Log("Opening Connection Plate Canvas");

                // Sets bool to true to open
                connectionCanvasOpen = true;

                // Turns on the connection plate canvas object
                connectionPlate.gameObject.transform.parent.gameObject.SetActive(true);
            }

            // Checks to see if connections avail is not open
            if(!connectionAvailableCanvasOpen && pluggableList.Count > 0)
            {
                //Debug.Log("Opening Connection Available Canvas");

                // Sets bool to true to open
                connectionAvailableCanvasOpen = true;

                // Sets the connection avaiable plate to open
                connectionsAvailablePlate.gameObject.SetActive(true);

                // Checks that there's something in the pluggable list
                //if (pluggableList.Count > 0)
                //{
                    Debug.Log("Setting connections available list from pluggable list");
                    //connectionsAvailablePlate.DestroyAllButtons();

                    //Sets the connections abailable based on the pluggable list
                    connectionsAvailablePlate.SetConnectionsAvailable(pluggableList);
                //}
                    

                //else
                    //Debug.Log("Pluggable Available List is null");
            }            
        }*/
    }

    // Collision Exit even needed if we are using joints?

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (pluggableList.Count <= 0)
            {
                //Debug.Log("No pluggables in list");
                return;
            }

            // Checks the connection canvas is not open
            if (!connectionCanvasOpen && pluggableList.Count > 0)
            {
                Debug.Log("Opening Connection Plate Canvas");

                // Sets bool to true to open
                connectionCanvasOpen = true;

                // Turns on the connection plate canvas object
                connectionPlate.gameObject.transform.parent.gameObject.SetActive(true);
            }

            // Checks to see if connections avail is not open
            if (!connectionAvailableCanvasOpen && pluggableList.Count > 0)
            {
                //Debug.Log("Opening Connection Available Canvas");

                // Sets bool to true to open
                connectionAvailableCanvasOpen = true;

                // Sets the connection avaiable plate to open
                connectionsAvailablePlate.gameObject.SetActive(true);

                //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(false);

                // Checks that there's something in the pluggable list
                //if (pluggableList.Count > 0)
                //{
                Debug.Log("Setting connections available list from pluggable list");
                //connectionsAvailablePlate.DestroyAllButtons();

                //Sets the connections abailable based on the pluggable list
                connectionsAvailablePlate.SetConnectionsAvailable(pluggableList);
                //}


                //else
                //Debug.Log("Pluggable Available List is null");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Checks i the connection canvas is open AND if the connection plate doesn't have anything plugged in, otherwise will stay open
            if (connectionCanvasOpen && connectionPlate.ReturnHasAPlugPlugged() == false)
            {
                // Sets the connection canvas open to false
                connectionCanvasOpen = false;

                // Turns the connection plate off
                connectionPlate.gameObject.transform.parent.gameObject.SetActive(false);
            }

            // Checks to see if connections avail is open and if there are any
            if (connectionAvailableCanvasOpen)
            {
                //Debug.Log("Destroying all buttons and resetting connections available panel");

                // Sets the connectino available open to false
                connectionAvailableCanvasOpen = false;
                //connectionsAvailablePlate.DestroyAllButtons();

                // Resets the line renderer if the player walks out while it's still on
                //connectionsAvailablePlate.ResetLineRenderer();

                // Turns off the connection available plate
                connectionsAvailablePlate.gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Checks for a collision with the player
        
    }

    
    // This returns a list of the pluggable game objects
    public List<GameObject> ReturnPluggableAvailableList()
    {
        return pluggableList;
    }

    // This returns a reference to the connection plate
    public uConnectionPlate ReturnConnectionPlate()
    {
        return connectionPlate;
    }

    // This returns a reference to the connection available panel
    public uConnectionsAvailablePanel ReturnConnectionAvailablePanel()
    {
        return connectionsAvailablePlate;
    }
}
