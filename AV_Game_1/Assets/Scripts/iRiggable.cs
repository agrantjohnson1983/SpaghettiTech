using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iRiggable
{
    eTypeRigSetup TypeRig
    {
        get;
        set;
    }
    Transform TransformRig
    {
        get;
        set;
    }

    public bool Enabled
    {
        get;
        set;
    }

    public bool IsSet
    {
        get;
        set;
    }
    
    public void SetRigging(GameObject _riggingObject, GameObject _setupObject, Vector3 _offset)
    {
        //hasBeenRigged = true;

        Debug.Log("Setting rigging");

        _riggingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _riggingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        _riggingObject.GetComponent<Rigidbody>().useGravity = false;

        _riggingObject.transform.position = _setupObject.transform.position + _offset;
        _riggingObject.transform.rotation = _setupObject.transform.rotation;

        IsSet = true;
    }

    public void RiggingObjectComplete(GameObject _riggedObject, eTypeRigSetup _type)
    {
        sRiggingManager.riggingManger.RigSet(_type);

        Enabled = false;
        //_riggedObject.GetComponent<Collider>().enabled 
    }

    void CompleteRig()
    {

    }
}
