using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlugType { power, NONE }
public class sCablePlug : sInteractive, iPluggable
{
    Collider collider;

    sCablePlug cablePlugOtherEnd;

    sCableSegmentHandler cableSegmentHandler;
    //sCable cable;

    public Sprite _connectionSprite;

    public bool _isInput;

    bool _isPluggedIn;

    bool _isAvailableToPlugIn;

    public bool IsAvailableToPlugIn
    {
        get
        {
            return _isAvailableToPlugIn;
        }

        set
        {
            _isAvailableToPlugIn = value;
        }
    }

    //ConfigurableJoint joint;

    public GameObject plugObj
    {
        get
        {
            return this.gameObject;
        }

        set
        {
            plugObj = value;
        }
    }

    public Sprite connectionSprite
    {
        get
        {
            return _connectionSprite;
        }
        set
        {
            _connectionSprite = value;
        }
    }

    public bool IsPluggedIn
    {
        get
        {
            return _isPluggedIn;
        }
        set
        {
            _isPluggedIn = value;
        }
            
    }

    public bool IsInput
    {
        get
        {
            return _isInput;
        }

        set
        {
            _isInput = value;
        }
    }

    public ePlugType typePlug;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //cable = GetComponentInParent<sCable>();
        IsPluggedIn = false;

        IsAvailableToPlugIn = false;

        collider = GetComponent<Collider>();

        cableSegmentHandler = GetComponentInParent<sCableSegmentHandler>();

        //joint = GetComponent<ConfigurableJoint>();
    }


    public void PlugConnect()
    {
        Debug.Log("Cable plug is connected and disabling cable");

        IsPluggedIn = true;

        //this.transform.parent = _transform;

        //collider.enabled = false;

        if(cablePlugOtherEnd.ReturnIsPluggedIn() == true)
        {
            Debug.Log("Both cables are plugged in");

            cableSegmentHandler.ConnectionComplete();
        }

        GameManager.gm.ReturnCurrentPlayer().ReturnGrabController().GrabReset();
        //cable.ConnectionComplete(this, _transform);
    }

    public bool ReturnIsPluggedIn()
    {
        return IsPluggedIn;
    }

    public void SetPlugOtherEnd(sCablePlug _cablePlug)
    {
        cablePlugOtherEnd = _cablePlug;
    }

    public void SetPlugAvailable(GameObject _sourceToConnectObj, bool _isAvailableToPlugIn)
    {
        // plugs cable in and sets it to a joint
        if(_isAvailableToPlugIn)
        {
            Debug.Log("Setting Plug Available and Connected to Joint");

            IsAvailableToPlugIn = true;

            transform.parent = _sourceToConnectObj.transform;

            //rb.constraints = RigidbodyConstraints.FreezeAll;

            rb.velocity = Vector3.zero;

            //joint.connectedBody = _sourceToConnectObj.GetComponent<Rigidbody>();
        }

        // unplugs cable and shoots it out
        else
        {
            Debug.Log("Plug Set to Not Available and Disonnecting");

            transform.parent = null;

            //joint.connectedBody = null;

            rb.AddForce(_sourceToConnectObj.transform.forward * 10, ForceMode.Impulse);
        }
    }

    /*
    public void SendInputConnection(sPowerSource _powerSource)
    {
        //cable.ConnectSourceInput(_powerSource);
    }

    public void SendOututConnection(sPowerSupply _powerSupply)
    {
        //cable.ConnectSupplyOutput(_powerSupply);
    }
    */

    
}
