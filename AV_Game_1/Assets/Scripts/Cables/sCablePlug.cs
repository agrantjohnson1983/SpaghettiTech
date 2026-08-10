using System.Collections;
using UnityEngine;


public class sCablePlug : sInteractive, iPluggable, iClickable
{
    public uTextPopupSpawn textPopup;

    //public GameObject canvasUI;

    //Collider collider;

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

    sConnectionSupply supply;
    sConnectionSource source;

    public GameObject plugObj
    {
        get
        {
            return this.gameObject;
        }

        set
        {
            //plugObj = value;
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

    public ePlugType TypePlug
    {   get
        {
            return _typePlug;
        }

        set
        {
            _typePlug = value;
        }
    }

    public float _unplugCooldownTime = 3f;
    public float UnplugCooldownTime { get { return _unplugCooldownTime; } set { _unplugCooldownTime = value; } }

    int _index;

    public int Index { get { return _index; } set { _index = value; } }

    public float disconnectForce = 50f;

    public ePlugType _typePlug;

    public GameObject connectionUI;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //cable = GetComponentInParent<sCable>();
        IsPluggedIn = false;

        IsAvailableToPlugIn = false;

        //collider = GetComponent<Collider>();

        cableSegmentHandler = GetComponentInParent<sCableSegmentHandler>();

        connectionUI.SetActive(false);
        //joint = GetComponent<ConfigurableJoint>();
    }

    // Checks if the plug type in argument is same as this cable plug - return true if so
    public bool CheckIfCorrectConnection(ePlugType _type)
    {
        if (_type == TypePlug)
        {
            //textPopup.SpawnTextPopup(this.transform, "CORRECT CONNECTION TYPE", 12);

            return true;
        }

        else
        {
            // Spawns a text popup displaying that the connection type is wrong
            textPopup.SpawnTextPopup(this.transform, "WRONG CONNECTION TYPE", 12);

            return false;
        }
    }

    // This gets called when a plug gets connected
    public void PlugConnect()
    {
        //Debug.Log(this.gameObject.name + " is plugged in and connected");

        // Sets plug to be plugged in
        IsPluggedIn = true;

        // Checks if the other end of the cable is plugged in
        if(cablePlugOtherEnd.ReturnIsPluggedIn() == true)
        {
            //Debug.Log("Both cables are plugged in");

            // Sets the cables to complete - turns them green
            cableSegmentHandler.ConnectionComplete();

            textPopup.SpawnTextPopup(this.gameObject.transform, "FULLY CONNECTED", 20);

            // Checks if there is a supply
            if (supply)
            {
                //Debug.Log("Toggling supply objects on!");

                // Toggles on supply objects
                supply.TogglePoweredObjects(true);

                // drains power
                cablePlugOtherEnd.source.DrainPower(supply.powerDrainAmount);

                // turns on connection UI
                connectionUI.SetActive(true);
            }

            // Checks if the other end has a supply connection
            else if(cablePlugOtherEnd.supply)
            {
                // Toggles on other end supply objects on
                cablePlugOtherEnd.supply.TogglePoweredObjects(true);

                // drains power
                if(source != null)
                    source.DrainPower(cablePlugOtherEnd.supply.powerDrainAmount);

                // turns on connection UI
                connectionUI.SetActive(true);
            }
        }

        else
        {
            //Debug.Log("One side of cable plugged in");

            // Sets cable to half plugged in - turns yellow
            cableSegmentHandler.HalfConnect();

            textPopup.SpawnTextPopup(this.gameObject.transform, "HALF PLUGGED IN", 12);
        }

        // Destroys any grab joints from playerw
        DestroyGrabJoint();

        // Resets the player grab - this might be buggy?
        //GameManager.gm.ReturnCurrentPlayer().ReturnGrabController().GrabReset();
        
        //cable.ConnectionComplete(this, _transform);
    }

    // This gets called when a plug gets disconnected - TODO - might need a cooldown coroutine so plug doesn't get plugged right back in
    public void PlugDisconnect()
    {
        // Sets is plugged in to false
        //IsPluggedIn = false;
        //IsAvailableToPlugIn = false;

        //SetPlugAvailable(true);

        // Turns off connection UI
        connectionUI.SetActive(false);

        // Checks if the other end is plugged in to know if cable is half or fully disconnected
        if (cablePlugOtherEnd.ReturnIsPluggedIn() == true)
        {
            Debug.Log("One side of cable plugged in");

            // Half connects the cable from being fully connected
            cableSegmentHandler.HalfConnect();

            textPopup.SpawnTextPopup(this.gameObject.transform, "HALF PLUGGED IN", 12);
        }

        else
        {
            Debug.Log("Disconnecting Cables");
            
            // Fully disconnects the cable
            cableSegmentHandler.Disconnect();

            textPopup.SpawnTextPopup(this.gameObject.transform, "DISCONNECTING CABLE", 12);
        }

        // Destroys the joint connecting the plug
        DestoryPlugJoint();

        // Sets the RB constraints to only freeze the rotation
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Shoots cable out - set to negative forward so it shoots backwards?
        rb.AddForce(this.gameObject.transform.forward + this.gameObject.transform.up * disconnectForce, ForceMode.Impulse);

        // Starts coroutine for unplug cooldown, so the plug doesn't get plugged in again immediatly and set as available
        StartCoroutine(UnplugCooldown(UnplugCooldownTime));
    }


    // This is used when unplugging a plug so that it doesn't immediatly get plugged back in
    IEnumerator UnplugCooldown(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);

        //Debug.Log("Unplug delay has completed");

        IsPluggedIn = false;
    }

    // This will destroy fixed joints connected to player
    public void DestroyGrabJoint()
    {
        if (this.gameObject.transform.parent.TryGetComponent<sCharacterGrabController>(out sCharacterGrabController _grabber))
            _grabber.GrabReset();

        if (TryGetComponent<FixedJoint>(out FixedJoint joint))
            Destroy(joint);
        else
            Debug.Log("Trying to destroy fixed joint, but there ain't nun");

        
    }

    // Destorys the plugs joint if there is one
    public void DestoryPlugJoint()
    {
        // Gets the reference on this gameObject to a hinge joint if there is one
        if (TryGetComponent<HingeJoint>(out HingeJoint joint))
            Destroy(joint);

        else
            Debug.Log("Trying to destroy hinge joint, but there ain't nun!");
    }

    // Returns a bool if the plug is plugged in or not
    public bool ReturnIsPluggedIn()
    {
        return IsPluggedIn;
    }

    // Sets the plug of the other end
    public void SetPlugOtherEnd(sCablePlug _cablePlug)
    {
        cablePlugOtherEnd = _cablePlug;
    }

    // This gets used to set the plug as available so it can be seen in the connections available panel
    public void SetPlugAvailable(bool _isAvailableToPlugIn)
    {
        // plugs cable in and sets it to a joint
        if(_isAvailableToPlugIn)
        {
            //Debug.Log("Setting Plug Available and Connected to Joint");

            IsAvailableToPlugIn = true;

            connectionUI.SetActive(true);

            //transform.parent = _sourceToConnectObj.transform;

            //rb.constraints = RigidbodyConstraints.FreezeAll;

            rb.velocity = Vector3.zero;

            //joint.connectedBody = _sourceToConnectObj.GetComponent<Rigidbody>();

            CanBeGrabbed = false;

            textPopup.SpawnTextPopup(this.gameObject.transform, "READY TO PLUG IN", 12);
        }

        // unplugs cable and shoots it out
        else
        {
            //Debug.Log("Plug Set to Not Available and Disonnecting");

            transform.parent = null;

            connectionUI.SetActive(false);

            //joint.connectedBody = null;

            //rb.AddForce(-this.gameObject.transform.forward * 10, ForceMode.Impulse);

            CanBeGrabbed = true;
        }
    }

    // This gets used to set the connection source/supply for the plug that gets connected
    public void SetConnection(GameObject _connectionToSet, float _powerDrainAmount)
    {

        if(_connectionToSet.TryGetComponent<sConnectionSource>(out sConnectionSource _source))
        {
            //Debug.Log("Setting connection source");
            source = _source;

            source.DrainPower(_powerDrainAmount);
        }

        else if(_connectionToSet.TryGetComponent<sConnectionSupply>(out sConnectionSupply _supply))
        {
            //Debug.Log("Setting connection supply");
            supply = _supply;



            PlugConnect();
        }

        else
        {
            Debug.Log("Attempted SetConnection on CablePlug didn't work - No source or supply found");
        }
    }

    public void OnClick()
    {
        //Debug.Log("Cable Plug was clicked");

        // Checks if plugged in
        if(IsPluggedIn)
        {
            //Debug.Log("Cable Plug was clicked that is plugged in");

            // Disconnects plug
            PlugDisconnect();

            // Checks is there is a Connection Source
            if (source)
            {
                Debug.Log("Source detected - unplugging panels");

                bool connectionAvailPanelOpen;

                bool connectionPlateOpen;

                // checks if the connection available panel is active
                connectionAvailPanelOpen = source.ReturnConnectionAvailablePanel().isActiveAndEnabled;

                // checks if the connection plate panel is active
                connectionPlateOpen = source.ReturnConnectionPlate().isActiveAndEnabled;

                // switches connection available panel on to disconnect if it's not already
                if (!connectionAvailPanelOpen)
                    source.ReturnConnectionAvailablePanel().gameObject.SetActive(true);

                if (!connectionPlateOpen)
                    source.ReturnConnectionPlate().gameObject.SetActive(true);

                // Disconnects plug from connection available panel and destroys the connection available button
                source.ReturnConnectionAvailablePanel().DisconnectPlug(Index);

                // Disconnects channel from connection plate.
                source.ReturnConnectionPlate().DisconnectChannel(Index);

                if (!connectionAvailPanelOpen)
                    source.ReturnConnectionAvailablePanel().gameObject.SetActive(false);

                if (!connectionPlateOpen)
                    source.ReturnConnectionPlate().gameObject.SetActive(false);


                //SetIndex(null);

                // Disconnects Plug from the Source
                source.DisconnectClick(Index);

                // Sets the source to null
                source = null;
            }

            // Checks if there is a Connection Supply
            else if (supply)
            {
                // Turns off the powered objects on the supply
                supply.TogglePoweredObjects(false);

                // Sets the supply to null
                supply = null;
            }

            else
            {
                Debug.Log("Plug is trying to disconnect but has no source or supply");
            }

            return;
        }

        else
        {
            Debug.Log("Cable Plug was clicked that is not plugged in");
        }

        // Checks if plug is available to plugin and connected to a source
        if(IsAvailableToPlugIn)
        {
            Debug.Log("Plug was clicked that is available to plugin");

            PlugDisconnect();

            if (source)
            {
                Debug.Log("Source detected - unplugging panels");

                bool connectionAvailPanelOpen;

                // checks if the connection available panel is open
                connectionAvailPanelOpen = source.ReturnConnectionAvailablePanel().isActiveAndEnabled;

                // switches connection available panel on to disconnect if it's not already
                if (!connectionAvailPanelOpen)
                    source.ReturnConnectionAvailablePanel().gameObject.SetActive(true);

                // Disconnects plug from connection available panel and destroys the connection available button
                source.ReturnConnectionAvailablePanel().DisconnectPlug(Index);

                // Turns the connection available panel back off before anyone notices
                if(!connectionAvailPanelOpen)
                    source.ReturnConnectionAvailablePanel().gameObject.SetActive(false);

                // Disconnects channel from connection plate.
                //source.ReturnConnectionPlate().DisconnectChannel(Index);

                //SetIndex(null);

                // Disconnects Plug from the Source
                source.DisconnectClick(Index);

                // Sets the source to null
                source = null;
            }

            else
            {
                Debug.Log("Disconnection plug that is available to plug in, but has no source to disconnect");
            }
        }

        else
        {
            Debug.Log("Plug was clicked that is not available to plugin");
        }

    }


    public void SetIndex(int _index)
    {
        //Debug.Log("Plug is getting index set to " + _index);

        Index = _index;
    }

    public override void OnSelect()
    {
        if (IsPluggedIn || !CanBeGrabbed)
            return;

        base.OnSelect();

        //Debug.Log("Plug was selected: " + this);
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
