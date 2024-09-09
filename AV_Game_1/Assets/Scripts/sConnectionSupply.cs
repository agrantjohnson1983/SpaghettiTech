using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sConnectionSupply : MonoBehaviour
{
    public GameObject[] poweredObjects;

    bool isConnected = false;

    //bool isPluggedIn = false;

    // Start is called before the first frame update
    void Start()
    {
        if(poweredObjects != null)
        TogglePoweredObjects(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool ConnectPower(sCablePlug _plugCable)
    {
        // Checks plug type and if input or output
        if(_plugCable.typePlug == ePlugType.power && _plugCable.IsInput)
        {
            isConnected = true;

            //_plugCable.PlugConnect()

            TogglePoweredObjects(isConnected);
        }

        else
        {
            isConnected = false;
        }

        return isConnected;
    }

    public void TogglePoweredObjects(bool _hasPower)
    {
        for (int i = 0; i < poweredObjects.Length; i++)
        {
            poweredObjects[i].SetActive(_hasPower);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(TryGetComponent<sCablePlug>(out sCablePlug _plugCable) && !isPluggedIn)
        {
            Debug.Log("Cable Plug Detected");

            isPluggedIn = ConnectPower(_plugCable);
        }
    }
    */
}
