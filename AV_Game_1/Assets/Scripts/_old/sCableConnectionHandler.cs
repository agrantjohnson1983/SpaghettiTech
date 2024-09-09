using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCableConnectionHandler : MonoBehaviour
{
    public GameObject inputConnection, outputConnection;

    sConnectionSource connectionSource;
    sConnectionSupply powerSupply;

    bool inputConnected = false;
    bool outputConnected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectInput(sConnectionSource _connectionSource)
    {
        Debug.Log("Input Connected to Connection Handler");

        connectionSource = _connectionSource;

        inputConnected = true;

        if(outputConnected)
        {
            ConnectionComplete();
        }
    }

    public void ConnectOutput(sConnectionSupply _powerSupply)
    {
        Debug.Log("Output Connected to Connection Handler");

        powerSupply = _powerSupply;

        outputConnected = true;

        if (inputConnected)
            ConnectionComplete();
    }

    void ConnectionComplete()
    {
        Debug.Log("Connection is now complete and signal can fully flow!");

        powerSupply.TogglePoweredObjects(true);
    }
}
