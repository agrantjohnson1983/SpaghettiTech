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

    public List<GameObject> pluggableAvailableList;

    Rigidbody rb;

    public Transform connectionsPlateTransforms;

    //FixedJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        pluggableAvailableList = new List<GameObject>();
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
    public void OnConnectionClick()
    {
        Debug.Log("Power is Connected");

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
                // Checks for dupes in pluggable list
                bool _isThereADupe = false;

                if (pluggableAvailableList.Count > 0)
                {
                    for (int i = 0; i < pluggableAvailableList.Count - 1; i++)
                    {
                        if (pluggableAvailableList[i].GetComponent<iPluggable>() == _pluggable)
                        {
                            Debug.Log("Dupe found!");
                            _isThereADupe = true;
                        }
                    }

                    // if no dupe then can be added to pluggables list
                    if (_isThereADupe == false)
                    {
                        Debug.Log("Adding Pluggable" + _pluggable.ToString() + "to List");
                        ConnectPlugJoint(collision.gameObject);
                        pluggableAvailableList.Add(collision.gameObject);
                        _pluggable.SetPlugAvailable(this.gameObject, true);
                    }
                }

                else
                {
                    Debug.Log("First Pluggable " + _pluggable.ToString() + " added to List");
                    ConnectPlugJoint(collision.gameObject);
                    pluggableAvailableList.Add(collision.gameObject);
                    _pluggable.SetPlugAvailable(this.gameObject, true);
                }
            }
            
            else
            {
                Debug.Log("Plug is an output!");
            }
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
                connectionsAvailablePlate.SetConnectionsAvailable(pluggableAvailableList.ToArray());
            }            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        {
            if(pluggableAvailableList != null)
            {
                // Checks for dupes in pluggable list - removes a dupe in this case
                bool _isThereADupe = false;

                for (int i = 0; i < pluggableAvailableList.Count - 1; i++)
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
        }

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
                Debug.Log("Destorying all buttons and resetting connections available panel");

                connectionAvailableCanvasOpen = false;
                connectionsAvailablePlate.DestroyAllButtons();
                connectionsAvailablePlate.gameObject.SetActive(false);  
            }
        }
    }

    public List<GameObject> ReturnPluggableAvailableList()
    {
        return pluggableAvailableList;
    }
}
