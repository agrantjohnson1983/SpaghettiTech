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

    public void SetRigging(GameObject _riggingObject, GameObject _setupObject, Vector3 _offset);
        

    public void RiggingObjectComplete(GameObject _riggedObject, eTypeRigSetup _type)
    {
        sRiggingManager.riggingMangerGlobal.RigSet(_type);

        // If this piece carries item/weight data, tally it toward the
        // truss weight limit. RegisterTrussWeight itself filters by
        // rig type, so this is a no-op for anything that is not a
        // weight-bearing gear item (e.g. truss, bolts, motor).
        if (_riggedObject.TryGetComponent(out iLoadable _loadable))
        {
            sRiggingManager.riggingMangerGlobal.RegisterTrussWeight(_type, _loadable.ItemData);
        }

        Enabled = false;
        //_riggedObject.GetComponent<Collider>().enabled 
    }

    void CompleteRig()
    {

    }
}