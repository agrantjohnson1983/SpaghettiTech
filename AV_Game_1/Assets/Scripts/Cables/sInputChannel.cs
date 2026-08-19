using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class sInputChannel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int index = -1;

    int channelNumber;

    bool isConnected;

    public Image channelImage;

    public TMP_Text channelText;

    Sprite connectedImage, disconnectedImage;

    bool isBeingSelected = false;

    uConnectionPlate connectionPlate;
    uConnectionsAvailablePanel connectionAvailablePanel;

    // Start is called before the first frame update
    void Start()
    {
        isConnected = false;

        connectionPlate = GetComponentInParent<uConnectionPlate>();

        //connectionAvailablePanel = uConnectionsAvailablePanel.connectionAvailablePanel;
    }

    // This gets used to set a channel after it gets spawned
    public void SetChannel(int _channelNumber, Sprite _connectedSprite, Sprite _disconnectedSprite, string _channelName, uConnectionsAvailablePanel _connectionsAvailablePanel)
    {
        channelNumber = _channelNumber;

        connectedImage = _connectedSprite;

        disconnectedImage = _disconnectedSprite;

        channelText.text = _channelName;

        channelImage.sprite = disconnectedImage;

        connectionAvailablePanel = _connectionsAvailablePanel;
    }

    // This is used to set a channels index
    /*
    public void SetIndex(int _index)
    {
        Debug.Log("Setting Index of " + " to channel " + channelNumber);
        index = _index;
    }
    */

    // This returns a channels index
    public int ReturnIndex()
    {
        return index;
    }

    // This gets called when a input channel button gets clicked directly - this should be used for a disconnect - MIGHT not be needed anymore w/ mouse click on plugs
    public void OnClick()
    {
        // Checks if connected
        if(isConnected)
        {
            // Turns connection off if it's connected
            ToggleConnection(false);
        }

        //connectionPlate.OnClick(index);
    }

    private void Update()
    {
        // This is used to Detect Mouse up-click after the mouse is already held and line rendered.  It connects the connection and plug.
        
        if(Input.GetMouseButtonUp(0) && isBeingSelected)
        {
            // TO DO : mouse up connects the input channel with connection available

            //Debug.Log("Mouse Click Release When a Plug is Selected - Toggling Plug");
            isBeingSelected = false;

            ToggleConnection(true);
        }
    }

    // This handles the "selecting" of input channels
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Pointer Enter Triggered on channel ");

        // Sets a channel to green if a cursor is highlighted over it
        if(connectionAvailablePanel.ReturnIsClickingConnectionAvailable() == true && !isConnected)
        {
            channelImage.color = Color.green;

            isBeingSelected = true;

            connectionAvailablePanel.ChangeLineColor(Color.green);
        }
    }

    
    // This gets called when the mouse exits the button - Just used for deselecting
    public void OnPointerExit(PointerEventData eventData)
    {
        // When cursor fully exits and the channel is not connected
        if(eventData.fullyExited && !isConnected)
        {
            //Debug.Log("Pointer Exit Triggered");

            channelImage.color = Color.white;

            isBeingSelected = false;

            connectionAvailablePanel.ChangeLineColor(Color.magenta);
        } 
    }

    public void OnDisable()
    {
        if(!isConnected)
        {
            channelImage.color = Color.white;

            isBeingSelected = false;
        }
    }

    // This is used to turn on and off a channel
    public void ToggleConnection(bool _isConnected)
    {
        isConnected = _isConnected;

        if (!isConnected)
        {
            Debug.Log("Toggling plug to disconnected");

            // changes the image to disconnected image
            channelImage.sprite = disconnectedImage;

            // changes the image color to green to show it is available
            channelImage.color = Color.white;

            index = -1;
            // This disconnects button from connection plate
            //connectionPlate.OnClickDisconnect(index);

            //connectionPlate.DisconnectChannel(index);

            // This disconnects the other button that is the connection available
            //connectionAvailablePanel.DisconnectLine(index);
        }

        else
        {
            //Debug.Log("Toggling plug to connected");

            // changes the image to connected image
            channelImage.sprite = connectedImage;

            //changes the image color to green to show it is connected
            channelImage.color = Color.yellow;

            // Sets the index based on the connection available panels temp one
            index = connectionAvailablePanel.ReturnTempIndex();

            // This connects button from connection plate which the connects with the iPluggable
            connectionPlate.OnClickConnect(index);

            // This will set the connection available button
            connectionAvailablePanel.SetButtonConnected(index);

            // Disconnects the source which removes the pluggable object list
            connectionPlate.ReturnConnectionSource().DisconnectClick(index);

            // Sets the index to -1 so it doesn't get reconnected
            //index = -1;

            // This sets the temp connection objects in the connection panel - used for the line renderer system
            //connectionAvailablePanel.SetTempConnectionObjects(this.gameObject, connectionAvailablePanel.ReturnButtonConnectedFromIndex(index));

            // This sets a connection line between the available connection button and connected channel button
            //connectionAvailablePanel.SetConnectionLine(index);
        }  
    }

    public bool ReturnIsConnected()
    {
        return isConnected;
    }
}
