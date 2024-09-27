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

    //List<iPluggable> pluggableList;
    // Start is called before the first frame update
    void Start()
    {
        inputChannelList = new List<sInputChannel>();

        for (int i = 0; i < numberOfChannels; i++)
        {
            sInputChannel _tempInputChannel;

            _tempInputChannel = Instantiate(pConnectionChannel, this.transform).GetComponent<sInputChannel>();

            // using i to set channel names
            _tempInputChannel.SetChannel(i, connectedSprite, disconnectedSprite, i+1.ToString(), connectionsAvailablePanel);

            //

            inputChannelList.Add(_tempInputChannel);
        }
    }

    public void SetSource(sConnectionSource _source)
    {
        connectionSource = _source;
    }

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
    

    // Checks through the input channel list through the channels and looks for an index match and if it finds one it disconnects
    public void Disconnect(int _index)
    {
        // Iterates through all the channels
        for (int i = 0; i < numberOfChannels; i++)
        {
            // Checks for an input channel component and then checks to see if the input channel has the same index as the agrument parameter
            if(inputChannelList[i].TryGetComponent<sInputChannel>(out sInputChannel _channelToDisconnect) && _channelToDisconnect.ReturnIndex() == _index)
            {
                Debug.Log("Index match - Disconnecting Channel " + i + " at index " + _index);

                // If the index is a match the the plug gets toggled
                _channelToDisconnect.TogglePlug();

                // Disconnects the plug
                connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<iPluggable>().PlugConnect();
            }
        }
    }

    // This gets called when the line is connected to an available channel
    public void OnClickConnect(int _index)
    {
        //inputChannelList[_index].TogglePlug();
        Debug.Log("Cable Plugged Into Connection Plate at index " +_index);

        connectionsAvailablePanel.SetButtonConnected(_index);
        // connect image, etc.
        if(connectionSource.ReturnPluggableAvailableList() != null)
        {
            Debug.Log("Plugging in at connection source");

            // This connects the plug script
            connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<iPluggable>().PlugConnect();
            
            // This 
            //connectionSource.ConnectPlugJoint(connectionSource.ReturnPluggableAvailableList()[_index]);
        }
        

        else
        {
            Debug.Log("Pluggable list is null");
        }

        //connectionSource.OnConnectionClick();

    }

    public uConnectionsAvailablePanel ReturnConnectionAvailablePanel()
    {
        return connectionsAvailablePanel;
    }
}
