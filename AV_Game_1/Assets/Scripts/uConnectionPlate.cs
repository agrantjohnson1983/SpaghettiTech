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

    public void OnClickDisconnect(int _index)
    {
        Debug.Log("Disconnecting Input");

        
    }

    public void OnClick(int _index)
    {
        //inputChannelList[_index].TogglePlug();
        Debug.Log("Cable Plugged Into Connection Plate");
            // connect image, etc.
        if(connectionSource.ReturnPluggableAvailableList() != null)
        {
            connectionSource.ReturnPluggableAvailableList()[_index].GetComponent<iPluggable>().PlugConnect();
            //pluggable.PlugConnect();
            connectionSource.ConnectPlugJoint(connectionSource.ReturnPluggableAvailableList()[_index]);
        }
        

        else
        {
            Debug.Log("Pluggable null");
        }

        connectionSource.OnConnectionClick();

    }
}
