using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sConnectionSupply : MonoBehaviour
{
    public ePlugType connectionType;

    public GameObject[] connectionObjects;

    bool isConnected = false;

    bool isPluggedIn = false;

    Rigidbody rb;

    public float powerDrainAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Turns off connection objects at start
        if(connectionObjects != null)
        TogglePoweredObjects(false);
    }

    public bool TryConnectPLug(GameObject _pluggableObj)
    {
        iPluggable _pluggable;

        _pluggable = _pluggableObj.GetComponent<iPluggable>();

        // Checks plug type and if input or output.  Supply requires an OUTPUT.  Also checks if the type is correct.
        if(_pluggable.TypePlug == connectionType && !_pluggable.IsInput && !isConnected)
        {
            Debug.Log("Setting connection for " + this.gameObject.name);

            // Changes is connected to TRUE
            isConnected = true;

            // Sets the connectinon supply in the pluggable
            _pluggable.SetConnection(this.gameObject, powerDrainAmount);

            // Connects the pluggable game object by a joint
            ConnectPlugJoint(_pluggableObj);

            //_plugCable.PlugConnect()

            // Toggles objects on
            //TogglePoweredObjects(isConnected);
        }

        else
        {
            // Changes is connected to FASE if the plug type is wrong or if the wrong side of cable tries to connect
            Debug.Log("Wrong connection type or wrong end of calbe trying to connect");
            isConnected = false;
        }

        return isConnected;
    }

    // This gets called when a plug gets connected and turns on/off connection objects based on the argument given
    public void TogglePoweredObjects(bool _hasPower)
    {
        for (int i = 0; i < connectionObjects.Length; i++)
        {
            connectionObjects[i].SetActive(_hasPower);
        }
    }

    // This handles adding a joint to the plug object when it collides with the connection source
    public void ConnectPlugJoint(GameObject _plugObject)
    {
        Debug.Log("Adding fixed joint to " + _plugObject.name);

        HingeJoint joint;

        joint = _plugObject.AddComponent<HingeJoint>();

        joint.connectedBody = rb;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checks for a plug interface and if the cable is not plugged in
        if (collision.gameObject.TryGetComponent<iPluggable>(out iPluggable _pluggable) && !isPluggedIn)
        {
            Debug.Log("Cable Plug Detected");

            // This will try to connect the plug and return a bool if it was able to connect or not.  Sets is plugged in based on result.
            isPluggedIn = TryConnectPLug(collision.gameObject);

            if (isPluggedIn)
            {
                Debug.Log("Cable Plugged In to " + this.gameObject.name);

                // Sets the connection supply on the plug
                //_pluggable.SetConnection(this.gameObject);
            }

            else
            {
                Debug.Log(_pluggable + " did not work for " + this.gameObject.name);
            }
        }
    }
}
