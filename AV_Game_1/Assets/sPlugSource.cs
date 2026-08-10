using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is for simple source connections, like microphone, light, etc to provide realtime "source"
public class sPlugSource : MonoBehaviour
{
    public ePlugType typeOfPlug;
    public Transform plugConnectTransform;

    private void ConnectPlug()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<iPluggable>(out iPluggable _pluggable))
        {
            Debug.Log("Detected pluggable : " + _pluggable);

            if(_pluggable.TypePlug == typeOfPlug && !_pluggable.IsInput)
            {
                Debug.Log("Plug source is attempting plug in to output of " + typeOfPlug);
                _pluggable.PlugConnect();

                other.gameObject.transform.parent = plugConnectTransform;
                other.gameObject.transform.localPosition = Vector3.zero;
                other.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
