using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uConnectionPlate : MonoBehaviour
{
    sConnectionSource connectionSource;

    uConnectionsAvailablePanel connectionsAvailablePanel;

    public GameObject pConnectionChannel;

    public int numberOfChannels;

    //public string[] channelNames;

    public Sprite connectedSprite, disconnectedSprite;

    List<sInputChannel> inputChannelList;

    bool hasAPlugPlugged = false;

    //List<iPluggable> pluggableList;
    // Start is called before the first frame update
    void Start()
    {
        inputChannelList = new List<sInputChannel>();

        // iterates through number of channels
        for (int i = 0; i < numberOfChannels; i++)
        {
            sInputChannel _tempInputChannel;

            // spawns an input channel and gets a reference to it
            _tempInputChannel = Instantiate(pConnectionChannel, this.transform).GetComponent<sInputChannel>();

            // using i to set channel names
            _tempInputChannel.SetChannel(i, connectedSprite, disconnectedSprite, i+1.ToString(), connectionsAvailablePanel);

            // adds channel to list
            inputChannelList.Add(_tempInputChannel);
        }
    }

    // Sets the connection source to argument given
    public void SetSource(sConnectionSource _source)
    {
        connectionSource = _source;
    }

    // Sets the connections available panel to the argument given
    public void SetConnectionsAvailablePanel(uConnectionsAvailablePanel _connectionAvail)
    {
        connectionsAvailablePanel = _connectionAvail;
    }
 
    // This gets called by an input channel when it is clicked and connected
    public void OnClickDisconnect(int _index)
    {
        Debug.Log("Disconnecting Input at Connection Plate");
        //connectionsAvailablePanel.Disconnect(_index);
    }

    public void ResetChannels()
    {

    }
    
    // Checks through the input channel list through the channels and looks for an index match and if it finds one it disconnects
    public void DisconnectChannel(int _index)
    {
        // Iterates through all the channels
        for (int i = 0; i < inputChannelList.Count; i++)
        {
            // Checks for an input channel component
            if(inputChannelList[i].TryGetComponent<sInputChannel>(out sInputChannel _channelToDisconnect))
            {
                //Debug.Log("Index match - Disconnecting Channel " + i + " at index " + _index);

                // Checks if index is a match
                if(_channelToDisconnect.ReturnIndex() == _index)

                    // If the index is a match the the plug gets toggled
                    _channelToDisconnect.ToggleConnection(false);

                // Unlocks the position freeze but keeps the rotation freeze on the plug object
                //connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                // Sends force shooting the cable upward when disconnected
                //connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse);

                // Disconnects the plug
                //connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<iPluggable>().PlugConnect();

                // Removes connection from the list
                //connectionSource.ReturnPluggableAvailableList().RemoveAt(_index);

                // Checks if anything is plugged in
                if(CheckIfAnythingPluggedIn() == false)
                {
                    Debug.Log("Nothing is connected now");

                    // Sets has a plug to false
                    hasAPlugPlugged = false;

                    // Hides the connection plate if there are no other plugs
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    // This gets called by an input channel when the line is connected to an available channel
    public void OnClickConnect(int _index)
    {
        //inputChannelList[_index].TogglePlug();
        //Debug.Log("Cable Plugged Into Connection Plate at index " +_index);

        hasAPlugPlugged = true;

        // connect image, etc.
        if(connectionSource.ReturnPluggableAvailableList() != null)
        {
            //Debug.Log("Plugging in at connection source");

            // This gives the plug a reference to the connection source
            connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<iPluggable>().SetConnection(connectionSource.gameObject);

            // This connects the plug script
            connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<iPluggable>().PlugConnect();

            // Destroys plug joint
            connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<sCablePlug>().DestoryPlugJoint();

            // This takes the pluggable object and moves it's location to the channels position
            connectionSource.ReturnPluggableAvailableList()[_index].transform.position = inputChannelList[ReturnChannelFromIndex(_index)].transform.position;

            // This locks the plug in place in the channel position
            connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        

        else
        {
            //Debug.Log("Pluggable list is null");
        }

        //connectionSource.OnConnectionClick();

    }

    public uConnectionsAvailablePanel ReturnConnectionAvailablePanel()
    {
        return connectionsAvailablePanel;
    }

    // Checks the input channel list and returns true if something is connected
    bool CheckIfAnythingPluggedIn()
    {
        // sets initial bool to false
        bool hasAConnection = false;

        // iterates through the channels
        for (int i = 0; i < inputChannelList.Count; i++)
        {
            // Checks the input list to see if there is a connected channel;
            if(inputChannelList[i].ReturnIsConnected() == true)
            {
                Debug.Log("Channel " + (i + 1) + " has a connection - remember to subtract 1 to get index position");

                // sets connection to true if a match is found
                hasAConnection = true;
            }
        }

        // returns if there is a connection
        return hasAConnection;
    }

    public int ReturnChannelFromIndex(int _index)
    {
        //Debug.Log("Checking for index of " + _index + " in channels");

        // Sets temp channel to -1 so if it returns negative then there's no match
        int _tempChannel = -1;

        // iterates through all the channels
        for (int i = 0; i < numberOfChannels; i++)
        {
            // checks to see if the channel's index matches the argument given
            if(inputChannelList[i].ReturnIndex() == _index)
            {
                //Debug.Log("Channel match found in index " + _index + " on channel " + i);

                // sets the temp channel to i
                _tempChannel = i;
            }
        }

        //Debug.Log("Returning temp channel of " + _tempChannel);
        
        // returns the temp channel
        return _tempChannel;
    }

    // This returns the bool to see if this connection plate has anything plugged into it
    public bool ReturnHasAPlugPlugged()
    {
        return hasAPlugPlugged;
    }

    public sConnectionSource ReturnConnectionSource()
    {
        return connectionSource;
    }
}
