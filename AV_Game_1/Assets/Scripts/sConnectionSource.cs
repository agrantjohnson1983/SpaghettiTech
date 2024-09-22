using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sConnectionSource : MonoBehaviour
{
    //public Transform[] plugInLocations;

    //public sPlug[] plugs;

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

    //FixedJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        pluggableList = new List<GameObject>();
        //numberOfPlugsOpen = plugInLocations.Length;

        SpawnConnectionPlate();
        SpawnConnectionsAvailablePanel();

        rb = GetComponent<Rigidbody>();

        //joint = GetComponent<FixedJoint>();
    }

    void SpawnConnectionPlate()
    {
        GameObject tempObj;

        tempObj = Instantiate(pConnectionPanel, connectionsPlateTransforms);

        connectionPlate = tempObj.GetComponent<uConnectionPlate>();

        connectionPlate.SetSource(this);

        connectionPlate.gameObject.SetActive(false);
    }

    void SpawnConnectionsAvailablePanel()
    {
        GameObject tempObj;

        tempObj = Instantiate(pConnectionAvailablePanel, connectionsPlateTransforms);

        connectionsAvailablePlate = tempObj.GetComponent<uConnectionsAvailablePanel>();

        connectionsAvailablePlate.SetConnectionSource(this);

        connectionsAvailablePlate.gameObject.SetActive(false);

        connectionPlate.SetConnectionsAvailablePanel(connectionsAvailablePlate);

        connectionsAvailablePlate.SetConnectionPlate(connectionPlate);
    }

    public void ConnectPlugJoint(GameObject _plugObject)
    {
        Debug.Log("Adding fixed joint to " + _plugObject.name);

        FixedJoint joint;

        joint = _plugObject.AddComponent<FixedJoint>();

        joint.connectedBody = rb;
    }

    // When a connection is clicked from connection plate - turns connection plate off
    public void OnConnectionClick(int _index)
    {
        Debug.Log("Power is Connected to source");

        //pluggableList[_index]

        // TO DO - Set all the segments of a cable to yellow if half connected

        //connectionPlate.gameObject.SetActive(false);
    }

    public void DisconnectClick()
    {
        Debug.Log("Power is now disconnected");

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        {
            if(_pluggable.IsInput)
            {
                Debug.Log("Input connection detected");

                // Checks for dupes in pluggable list
                bool _isThereADupe = false;

                if (pluggableList.Count > 1)
                {
                    Debug.Log("Checking for dupes");

                    for (int i = 0; i < pluggableList.Count; i++)
                    {
                        if (pluggableList[i].GetComponent<iPluggable>() == _pluggable)
                        {
                            Debug.Log("Dupe found!");
                            _isThereADupe = true;
                        }
                    }

                    // if no dupe then can be added to pluggables list
                    if (_isThereADupe == false)
                    {
                        Debug.Log("No dupe found - Adding Pluggable " + _pluggable.ToString() + " to pluggable available list");

                        // Connects joint to plug securing it's position 
                        ConnectPlugJoint(collision.gameObject);

                        // adds plug to list of plugs available
                        pluggableList.Add(collision.gameObject);

                        // sets plug to available for plugin
                        _pluggable.SetPlugAvailable(this.gameObject, true);
                    }

                    else
                    {
                        Debug.Log("Dupe was found - doing nothing");
                    }
                }

                else
                {
                    Debug.Log("First Pluggable " + _pluggable.ToString() + " added to List");

                    // Connects joint to plug securing it's position 
                    ConnectPlugJoint(collision.gameObject);

                    // adds plug to list of plugs available
                    pluggableList = new List<GameObject>();
                    pluggableList.Add(collision.gameObject);

                    // sets plug to available for plugin
                    _pluggable.SetPlugAvailable(this.gameObject, true);
                }
            }
            
            else
            {
                Debug.Log("Plug is an output!");
            }

            Debug.Log("Pluggable List count is: " + pluggableList.Count);
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            // Checks the connection canvas and turns it on
            if(!connectionCanvasOpen)
            {
                Debug.Log("Opening Connection Plate Canvas");
                connectionCanvasOpen = true;
                connectionPlate.gameObject.SetActive(true);
            }

            // Checks to see if connections avail is open and if there are any
            if(!connectionAvailableCanvasOpen)
            {
                Debug.Log("Opening Connection Available Canvas");
                connectionAvailableCanvasOpen = true;
                connectionsAvailablePlate.gameObject.SetActive(true);

                if (pluggableList != null)
                {
                   // Debug.Log("Destroying all buttons and then opening connection available canvas and sending connection available list");
                    //connectionsAvailablePlate.DestroyAllButtons();
                    connectionsAvailablePlate.SetConnectionsAvailable(pluggableList);
                }
                    

                else
                    Debug.Log("Pluggable Available List is null");
            }            
        }
    }

    // Collision Exit even needed if we are using joints?

    
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        //{
            /*
            if(pluggableAvailableList != null)
            {
                // Checks for dupes in pluggable list - removes a dupe in this case
                bool _isThereADupe = false;

                for (int i = 0; i < pluggableAvailableList.Count; i++)
                {
                    if (pluggableAvailableList[i] == collision.gameObject)
                    {
                        Debug.Log("Dupe found on exit!");
                        _isThereADupe = true;
                    }
                }

                // if  dupe then can be removed from pluggables list
                if (_isThereADupe == true)
                {
                    Debug.Log("Removing Dupe");
                    pluggableAvailableList.Remove(collision.gameObject);
                }
            }
            */
        //}

        if (collision.gameObject.CompareTag("Player"))
        {
            if (connectionCanvasOpen)
            {
                connectionCanvasOpen = false;
                connectionPlate.gameObject.SetActive(false);
            }

            // Checks to see if connections avail is open and if there are any
            if (connectionAvailableCanvasOpen)
            {
                //Debug.Log("Destroying all buttons and resetting connections available panel");

                connectionAvailableCanvasOpen = false;
                //connectionsAvailablePlate.DestroyAllButtons();
                connectionsAvailablePlate.gameObject.SetActive(false);
            }
        }
    }

    

    public List<GameObject> ReturnPluggableAvailableList()
    {
        return pluggableList;
    }
}
