using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sConnectionSource : MonoBehaviour
{
    //public Transform[] plugInLocations;

    //public sPlug[] plugs;

    public ePlugType typeOfConnection;

    int numberOfPlugsOpen;

    bool isFull;

    bool connectionCanvasOpen = false;
    bool connectionAvailableCanvasOpen = false;

    public Sprite connectionPlateImage;

    public GameObject pConnectionPanel, pConnectionAvailablePanel;

    uConnectionPlate connectionPlate;

    uConnectionsAvailablePanel connectionsAvailablePlate;

    public List<GameObject> pluggableList;

    Rigidbody rb;

    public Transform connectionsPlateTransforms;

    public uTextPopupSpawn uTextPopup;

    public float totalPower = 100f;

    float currentPower;

    public Image powerUI;

    // Start is called before the first frame update
    void Start()
    {
        currentPower = totalPower;

        UpdatePowerUI(currentPower);

        pluggableList = new List<GameObject>();
        //numberOfPlugsOpen = plugInLocations.Length;

        SpawnConnectionPlate();

        SpawnConnectionsAvailablePanel();

        rb = GetComponent<Rigidbody>();

        //joint = GetComponent<FixedJoint>();
    }

    void UpdatePowerUI(float _currentPower)
    {
        powerUI.fillAmount = currentPower / totalPower;
    }

    // This spawns the connection plate which all of the input channels
    void SpawnConnectionPlate()
    {
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
    }


    // This spawns the connection available panel which shows connections that are avaiable when the have collided with the connection source
    void SpawnConnectionsAvailablePanel()
    {
        GameObject tempObj;

        tempObj = Instantiate(pConnectionAvailablePanel, connectionsPlateTransforms);

        connectionsAvailablePlate = tempObj.GetComponent<uConnectionsAvailablePanel>();

        // Gives the connection available plate a reference to this connection source
        connectionsAvailablePlate.SetConnectionSource(this);

        connectionsAvailablePlate.gameObject.SetActive(false);

        // Gives the connection plate a reference to the connection available panel
        connectionPlate.SetConnectionsAvailablePanel(connectionsAvailablePlate);

        // Gives the connection available plate a reference to the connection plate
        connectionsAvailablePlate.SetConnectionPlate(connectionPlate);
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
        pluggableList[_index].GetComponent<iPluggable>().SetConnection(this.gameObject);
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
                        _pluggable.SetConnection(this.gameObject);

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
                    _pluggable.SetConnection(this.gameObject);

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
                    uTextPopup.SpawnTextPopup(this.transform, "WRONG TYPE OF CONNECTION", 12);
                }

                else if(_pluggable.IsInput && _pluggable.CheckIfCorrectConnection(typeOfConnection) == true)
                {
                    // Spawns a text message saying wrong end of cable
                    uTextPopup.SpawnTextPopup(this.transform, "WRONG END OF CABLE", 12);
                }

                else
                {
                    // Not sure what would go here?
                }
            }

            //Debug.Log("Pluggable List count is: " + pluggableList.Count);
        }

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
        }
    }

    // Collision Exit even needed if we are using joints?

    
    private void OnCollisionExit(Collision collision)
    {
        // Checks for a collision with the player
        if (collision.gameObject.CompareTag("Player"))
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
